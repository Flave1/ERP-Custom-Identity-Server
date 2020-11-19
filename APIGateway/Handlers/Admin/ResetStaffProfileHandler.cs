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

namespace APIGateway.Handlers.Admin
{
    public class ResetStaffProfileCommand : IRequest<StaffRegRespObj>
    {
        public string UserId { get; set; }
        public class ResetStaffProfileCommandHandler : IRequestHandler<ResetStaffProfileCommand, StaffRegRespObj>
        {
            private readonly UserManager<cor_useraccount> _context;
            private readonly DataContext _dataContext;
            private readonly IEmailService _email;
            public ResetStaffProfileCommandHandler(UserManager<cor_useraccount> context, DataContext dataContext, IEmailService emailService)
            {
                _email = emailService;
                _context = context;
                _dataContext = dataContext;
            }

            private async Task SendStaffAccountDetailMail(string email, string name, string password)
            { 
                var Subject = $"Account Reset Successful";
                var Content = $"Below is your account login details <br/> " +
                    $"<b>Username : {name} <br/>" +
                    $"<b>Password : {password} <br/>" +
                    $"Please be sure to change your password on first login";
                 
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
            public async Task<StaffRegRespObj> Handle(ResetStaffProfileCommand request, CancellationToken cancellationToken)
            {
                var response = new StaffRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    var user = await _context.FindByIdAsync(request.UserId);
                    var randomPass = RandomCharacters.GeneratePassword();
                    var token = await _context.GeneratePasswordResetTokenAsync(user);
                    using (var trans = await _dataContext.Database.BeginTransactionAsync())
                    {
                        try
                        {                            
                            if (user != null)
                            {  
                                var passwordResetted = await _context.ResetPasswordAsync(user, token, randomPass);
                                if (!passwordResetted.Succeeded)
                                {
                                    await trans.RollbackAsync();
                                    response.Status.Message.FriendlyMessage = passwordResetted.Errors.FirstOrDefault().Description;
                                    return response;
                                }  
                                user.EnableAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
                                user.IsActive = true;
                                user.IsQuestionTime = false;
                                user.NextPasswordChangeDate = DateTime.UtcNow.AddDays(_dataContext.ScrewIdentifierGrid.FirstOrDefault(r => r.Module == (int)Modules.CENTRAL)?.PasswordUpdateCycle??365);
                                var accountResetted = await _context.UpdateAsync(user);
                                if (!accountResetted.Succeeded)
                                {
                                    await trans.RollbackAsync();
                                    response.Status.Message.FriendlyMessage = passwordResetted.Errors.FirstOrDefault().Description;
                                    return response;
                                } 
                                await SendStaffAccountDetailMail(user.Email, user.UserName, randomPass);
                            }
                            else
                            {
                                await trans.RollbackAsync();
                                response.Status.Message.FriendlyMessage = "Unable to find this user";
                                return response;
                            }
                            await trans.CommitAsync();
                            response.Status.Message.FriendlyMessage = $"Account reset Successful <b/> Account login details sent to staff email";
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
