using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Response.Recovery;
using APIGateway.Data;
using APIGateway.Extensions;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.URI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace APIGateway.AuthGrid.Recovery
{
    public class ChangePasswordCommand : IRequest<RecoveryResp>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, RecoveryResp>
        {
            private async Task RecoveryMail(string email)
            {
                var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
                         + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
                         + Path.DirectorySeparatorChar.ToString() + "loginDetails.html";

                var builder = new BodyBuilder(); 
                 
                using (StreamReader SourceReader = File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                var path = $"{_uRIs.MainClient}#/auth/login";
                var sm = new SendEmailCommand();
                sm.Subject = $"Account Recovery";
                sm.Content = $"Account recovery was successful. <br> click <a href='{path}'> here </a> to login into your account";
                sm.SendIt = true;

                string messageBody = string.Format(builder.HtmlBody, sm.Content.Replace("{", "").Replace("}", ""), "PPPPPP");
 
                EmailMessage em = new EmailMessage
                {
                    Subject = sm.Subject,
                    Content = messageBody,
                    FromAddresses = new List<EmailAddress>(), 
                    SendIt = true,  
                };
                
                em.ToAddresses = new List<EmailAddress>();
                em.ToAddresses.Add(new EmailAddress { Address = email, Name = email });
               
                await _email.Send(em);
            }

            private readonly IBaseURIs _uRIs;
            private readonly IEmailService _email;
            private readonly IWebHostEnvironment _env;
            private readonly DataContext _dataContext;
            private readonly UserManager<cor_useraccount> _userManager;
            public ChangePasswordCommandHandler(IBaseURIs uRIs, IEmailService email, DataContext dataContext,
                IWebHostEnvironment webHostEnvironment, UserManager<cor_useraccount> userManager)
            {
                _uRIs = uRIs;
                _email = email;
                _userManager = userManager;
                _env = webHostEnvironment;
                _dataContext = dataContext;
            }
            public async Task<RecoveryResp> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var response = new RecoveryResp { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);
                    if(user != null)
                    {
                        var decodedToken = CustomEncoder.Base64Decode(request.Token);
                        var passChanged = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
                        if (!passChanged.Succeeded)
                        {
                            response.Status.Message.FriendlyMessage = passChanged.Errors.FirstOrDefault().Description;
                            return response;
                        }
                        user.IsQuestionTime = false;
                        user.EnableAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
                        user.NextPasswordChangeDate = DateTime.UtcNow.AddDays(_dataContext.ScrewIdentifierGrid.FirstOrDefault(r => r.Module == (int)Modules.CENTRAL)?.PasswordUpdateCycle ?? 365);
                        var updated = await _userManager.UpdateAsync(user);
                        if (!updated.Succeeded)
                        {
                            response.Status.Message.FriendlyMessage = updated.Errors.FirstOrDefault().Description;
                            return response;
                        }
                        await RecoveryMail(request.Email);
                    }
                    else
                    {
                        response.Status.Message.FriendlyMessage = "Unidentified Email";
                        return response;
                    }
                    
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = "Password has successfully been changed";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Status.Message.FriendlyMessage = "Unable to process request";
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    return response;
                }
            }
        }
    }
   
}
