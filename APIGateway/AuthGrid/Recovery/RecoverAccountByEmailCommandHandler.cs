using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Response.Recovery;
using APIGateway.Extensions;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using GODP.APIsContinuation.DomainObjects.UserAccount;
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
    public class RecoverAccountByEmailCommand : IRequest<RecoveryResp>
    {
        public string Email { get; set; }
        public class RecoverAccountByEmailCommandHandler : IRequestHandler<RecoverAccountByEmailCommand, RecoveryResp>
        {
            private async Task RecoveryMail(string email,string token)
            {
                var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
                         + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
                         + Path.DirectorySeparatorChar.ToString() + "loginDetails.html";

                var builder = new BodyBuilder(); 
                 
                using (StreamReader SourceReader = File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                var path = $"{_uRIs.MainClient}#/auth/change/password?email={email}&token={token}";
                var sm = new SendEmailCommand();
                sm.Subject = $"Account Recovery";
                sm.Content = $"Please click <a href='{path}'> here </a> to change password";
                sm.SendIt = true;

                string messageBody = string.Format(builder.HtmlBody, sm.Content.Replace("{", "").Replace("}", ""), "PPPPPP");


                var toList = new List<EmailAddress>();
                var to = new EmailAddress
                {
                    Address = email,
                };
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
            private readonly UserManager<cor_useraccount> _userManager;
            public RecoverAccountByEmailCommandHandler(IBaseURIs uRIs, IEmailService email, IWebHostEnvironment webHostEnvironment, UserManager<cor_useraccount> userManager)
            {
                _uRIs = uRIs;
                _email = email;
                _env = webHostEnvironment;
                _userManager = userManager;
            }
            public async Task<RecoveryResp> Handle(RecoverAccountByEmailCommand request, CancellationToken cancellationToken)
            {
                var response = new RecoveryResp { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string ecodedToken = CustomEncoder.Base64Encode(token);
                    await RecoveryMail(request.Email, ecodedToken);
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = "Link to reset password has been sent to your email";
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
