using System; 
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using System.Linq; 
using System.Collections.Generic; 
using MailKit.Net.Pop3;
using Polly.Retry;
using Polly;
using System.Net.Sockets; 
using APIGateway.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using AutoMapper;
using APIGateway.Contracts.Commands.Email;
using GOSLibraries.Enums;
using Microsoft.AspNetCore.Hosting;
using GODP.APIsContinuation.Repository.Interface.Admin;
using System.Net.Http;
using GOSLibraries.GOS_Error_logger.Service; 

namespace APIGateway.MailHandler.Service
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly AsyncRetryPolicy _asyncRetryPolicy;
        private const int maxRetryTimes = 1;
        private readonly DataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILoggerService _logger;
        private readonly IAdminRepository _admin;
        public EmailService(IEmailConfiguration emailConfiguration,
            ILoggerService logger, DataContext dataContext,
            IMediator mediator, IMapper mapper, IWebHostEnvironment web,
            IAdminRepository admin)
        {
            _emailConfiguration = emailConfiguration;
            _dataContext = dataContext;
            _mediator = mediator;
            _mapper = mapper;
            _env = web;
            _logger = logger;
            _admin = admin;
            _asyncRetryPolicy = Policy.Handle<SocketException>()

                .WaitAndRetryAsync(maxRetryTimes, times =>

                TimeSpan.FromSeconds(times * 2));
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }

                return emails;
            }
        }
        public async Task Send(EmailMessage emailMessage)
        {
            try
            {
                if (_emailConfiguration.ShouldSendEmail || emailMessage.SendIt)
                {
                    var mailsetting = await _admin.GetAllEmailConfigAsync();
                    if (emailMessage.FromAddresses.Count() == 0)
                    {
                        emailMessage.SentBy = mailsetting.Count() > 0 ? mailsetting.FirstOrDefault().SenderEmail : _emailConfiguration.SmtpUsername;
                        var caption = mailsetting.Count() > 0 ? mailsetting.FirstOrDefault().MailCaption : _emailConfiguration.SmtpUsername;
                        emailMessage.FromAddresses.Add(new EmailAddress { Name = caption, Address = _emailConfiguration.SmtpUsername, });
                    }
                    var mimeMsg = new MimeMessage();
                    mimeMsg.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                    mimeMsg.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                    mimeMsg.Subject = emailMessage.Subject;




                    mimeMsg.Body = new TextPart("html")
                    {
                        Text = emailMessage.Content.Replace("{", "").Replace("}", "")
                    };

                    #region CHECK IT
                    //try
                    //{
                    //    //var certificate = new X509Certificate2(@"C:\Inetpub\wwwroot\dotnetcore\wwwroot\Templates\EmailTemplate\crafty-run-289803-a2ed89fee6ac.p12",
                    //    //    "notasecret", X509KeyStorageFlags.MachineKeySet);

                    //    var certificate = new X509Certificate2(_env.WebRootPath + Path.DirectorySeparatorChar.ToString()
                    //      + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
                    //      + Path.DirectorySeparatorChar.ToString() + "crafty-run-289803-a2ed89fee6ac.p12",
                    //        "notasecret", X509KeyStorageFlags.MachineKeySet);

                    //    var credential = new ServiceAccountCredential(new ServiceAccountCredential
                    //        .Initializer("admin-405@crafty-run-289803.iam.gserviceaccount.com")
                    //    {

                    //        Scopes = new[] { "https://mail.google.com/" },
                    //        User = "sales@gossoftware.co.uk"

                    //    }.FromCertificate(certificate));

                    //    _logger.Information($"Email About to send before token confirmation '{mimeMsg.Subject}'");

                    //    CancellationToken token = new CancellationToken();
                    //    bool result = await credential.RequestAccessTokenAsync(token);
                    //    using (var client = new SmtpClient())
                    //    {
                    //        client.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                    //        _logger.Information($"Email about to connect gmail server '{mimeMsg.Subject}'");
                    //        client.Connect("smtp.gmail.com", 587);
                    //        _logger.Information($"Email connected gmail server '{mimeMsg.Subject}'");
                    //        // use the OAuth2.0 access token obtained above
                    //        var oauth2 = new SaslMechanismOAuth2("favour.emmanuel@godp.co.uk", credential.Token.AccessToken);
                    //        client.Authenticate(oauth2);
                    //        _logger.Information($"Logged into gmail server '{mimeMsg.Subject}'");
                    //        client.Send(mimeMsg);
                    //        _logger.Information($"Email Sent '{mimeMsg.Subject}'");
                    //        client.Disconnect(true);
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    _logger.Information("Men it threw error");
                    //    _logger.Information(ex.ToString());
                    //    throw ex;
                    //}
                    #endregion
                    using (var client = new SmtpClient())
                    {
                        client.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                        client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);

                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        _logger.Information($"Removed Auth'{mimeMsg.Subject}'");

                        if (mailsetting.Count() > 0)
                        {
                            try
                            {
                                client.Authenticate(mailsetting.FirstOrDefault().SenderEmail, mailsetting.FirstOrDefault().SenderPassword);
                            }
                            catch (Exception)
                            {
                                _logger.Information($"Could not Authenticate email '{mimeMsg.Subject}'");
                                client.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                            }

                        }
                        else
                        {
                            client.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                        }
                        _logger.Information($"About to send'{mimeMsg.Subject}'");
                        await client.SendAsync(mimeMsg);

                        _logger.Information($"Email Sent '{mimeMsg.Subject}'");
                        await client.DisconnectAsync(true);
                    }

                }

            }
            catch (HttpRequestException ex)
            {
                // var errorId = ErrorID.Generate(4);
                _logger.Information($"Error Message{ ex?.Message}");
                // throw ex;
            }
        }
        public async Task<bool> SaveUpdateEmailAsync(EmailMessage model)
        {
            if (model.EmailMessageId > 0)
            {
                var item = await _dataContext.EmailMessage.FindAsync(model.EmailMessageId);
                if (item != null)
                {
                    item.EmailStatus = (int)EmailStatus.Read;   
                    _dataContext.Entry(item).CurrentValues.SetValues(model);
                }
            }
            else
                await _dataContext.EmailMessage.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateCurrentUserEmailAsync(EmailAddress model)
        {
            if (model.EmailAddressId > 0)
            {
                var item = await _dataContext.EmailAddress.FindAsync(model.EmailAddressId);
                if (item != null)
                {
                    _dataContext.Entry(item).CurrentValues.SetValues(model);
                }
            }
            else
                await _dataContext.EmailAddress.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public async Task<List<EmailMessage>> GetAllEmailForAUserAsync(string email, string userid)
        {
            return await _dataContext.EmailMessage.Where(d => d.ReceivedBy.Trim().ToLower() == email.Trim().ToLower() || d.ReceiverUserId == userid).ToListAsync(); 
        }
        public async Task<List<EmailAddress>> GetAllEmailAddressForAUserAsync(string email)
        {
            return await _dataContext.EmailAddress.Where(d => d.Address.Trim().ToLower() == email.Trim().ToLower() && d.Action == (int)EmailAction.Receiver).ToListAsync(); 
        }
        public  Task<bool> DeleteEmailAsync(EmailMessage model)
        {
            throw new NotImplementedException();
        }
        public async Task<EmailMessage> GetSingEmailAsync(int EmailMessageId)
        {
            return await _dataContext.EmailMessage.FirstOrDefaultAsync(d => d.Deleted == false && d.EmailMessageId == EmailMessageId);
        }
        public async Task SendEmailAsync(EmailMessage model)
        {
            if(model != null)
            {
                var email = _mapper.Map<SendEmailCommand>(model);
                await _mediator.Send(email);
            }
        }
        public async Task<List<EmailAddress>> GetToAndFromEmailAddressesAsync(int EmailId)
        {
            return await _dataContext.EmailAddress.Where(s => s.EmailMessageId == EmailId).ToListAsync();
        }
        public async Task<EmailMessage> GetSingleEmailAsync(int emailId)
        {
            return await _dataContext.EmailMessage.FindAsync(emailId);
        }
        public async Task<EmailAddress> GetSingleEmailAddressAsync(string email, int emailId)
        {
            return await _dataContext.EmailAddress.FirstOrDefaultAsync(s => s.EmailMessageId == emailId && 
            s.Address.Trim().ToLower() == email.Trim().ToLower() && s.Action == (int)EmailAction.Receiver);
        }
        public async Task<EmailMessage> BuildAndSaveEmail(SendEmailCommand request)
        {

            var emailObject = new EmailMessage();
            EmailAddress receiver = new EmailAddress(); 
            

            var mailsetting = await  _admin.GetAllEmailConfigAsync();

            if (request.FromAddresses.Count() > 0)
            {
                emailObject.SentBy = request.ToAddresses.FirstOrDefault().Address;
                emailObject.FromAddresses = _mapper.Map<List<EmailAddress>>(request.FromAddresses);
            }
            else
            {
                emailObject.SentBy = mailsetting.Count() > 0 ? mailsetting.FirstOrDefault().SenderEmail : _emailConfiguration.SmtpUsername; 
                var caption = mailsetting.Count() > 0 ? mailsetting.FirstOrDefault().MailCaption : _emailConfiguration.SmtpUsername; 
                emailObject.FromAddresses.Add(new EmailAddress { Name = caption, Address = _emailConfiguration.SmtpUsername,});
            }
            emailObject.ToAddresses = _mapper.Map<List<EmailAddress>>(request.ToAddresses);

            if (!string.IsNullOrEmpty(request.UserIds))
            {
                request.IdentificationId = request.UserIds.Split(',').ToList();
            }
            if(emailObject.ToAddresses.Count() > 0)
            {
                var index = 0;
                foreach (var receiverPerson in request.ToAddresses)
                {
                    emailObject.Module = request.Module;
                    emailObject.EmailStatus = (int)EmailStatus.Received;
                    emailObject.DateSent = DateTime.Now;
                    emailObject.Content = request.Content;
                    emailObject.Subject = request.Subject;
                    if(!string.IsNullOrEmpty(request.UserIds)) emailObject.ReceiverUserId = request.IdentificationId[index] ?? string.Empty;
                    emailObject.ReceivedBy = receiverPerson.Address;
                    index = index + 1;
                    if (request.SaveIt) await SaveUpdateEmailAsync(emailObject);
                }
            }
           
            return emailObject;
        }
    }
}
