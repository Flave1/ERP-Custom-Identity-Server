using APIGateway.Contracts.Commands.Email;
using APIGateway.Data;
using APIGateway.Extensions;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.Response.Admin;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid.Recovery
{
    public class ChangeOldPasswordCommand : IRequest<StaffRegRespObj>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string UserName { get; set; }
        public class ChangeOldPasswordCommandHandler : IRequestHandler<ChangeOldPasswordCommand, StaffRegRespObj>
        {
            private readonly UserManager<cor_useraccount> _context;
            private readonly DataContext _dataContext;
            private readonly IEmailService _email;
            public ChangeOldPasswordCommandHandler(UserManager<cor_useraccount> context, DataContext dataContext, IEmailService emailService)
            {
                _email = emailService;
                _context = context;
                _dataContext = dataContext;
            }

            private async Task SendStaffAccountDetailMail(string email, string name)
            {
                var Subject = $"Password Update Successful";
                var Content = $"Password has successfuly being changed";

                var addre = new EmailAddress
                {
                    Address = email,
                    Name = name,
                };
                var adds = new List<EmailAddress>();
                adds.Add(addre);
                var em = new EmailMessage
                {
                    Subject = Subject,
                    Content = Content,
                    ToAddresses = adds,
                    FromAddresses = new List<EmailAddress>(),

                };
                em.SendIt = true;
                em.Module = (int)Modules.CENTRAL;
                await _email.Send(em);
            }
            public async Task<StaffRegRespObj> Handle(ChangeOldPasswordCommand request, CancellationToken cancellationToken)
            {
                var response = new StaffRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    var user = await _context.FindByNameAsync(request.UserName);

                    var token = await _context.GeneratePasswordResetTokenAsync(user);
                    using (var trans = await _dataContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var passwordResetted = await _context.ResetPasswordAsync(user, token, request.NewPassword);
                            if (!passwordResetted.Succeeded)
                            {
                                await trans.RollbackAsync();
                                response.Status.Message.FriendlyMessage = passwordResetted.Errors.FirstOrDefault().Description;
                                return response;
                            }
                            user.EnableAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
                            user.IsActive = true;
                            user.IsQuestionTime = false;
                            user.NextPasswordChangeDate = DateTime.UtcNow.AddDays(_dataContext.ScrewIdentifierGrid.FirstOrDefault(r => r.Module == (int)Modules.CENTRAL)?.PasswordUpdateCycle ?? 365);
                            var accountResetted = await _context.UpdateAsync(user);
                            if (!accountResetted.Succeeded)
                            {
                                await trans.RollbackAsync();
                                response.Status.Message.FriendlyMessage = passwordResetted.Errors.FirstOrDefault().Description;
                                return response;
                            }
                            await SendStaffAccountDetailMail(user.Email, user.UserName);
                            await trans.CommitAsync();
                            response.Status.Message.FriendlyMessage = $"Password update Successful <b/> Account login details sent to staff email";
                            response.Status.IsSuccessful = true;
                            return response;
                        }
                        catch (Exception e)
                        {
                            await trans.RollbackAsync();
                            response.Status.Message.FriendlyMessage = $"Error Occured during processing request: {e?.Message}";
                            response.Status.Message.TechnicalMessage = e.ToString();
                        }
                        finally { await trans.DisposeAsync(); }
                    }

                }
                catch (Exception e)
                {
                    response.Status.Message.FriendlyMessage = $"Error Occured during processing request: {e?.Message}";
                    response.Status.Message.TechnicalMessage = e.ToString();
                }
                return response;
            }
        }
    }

}
