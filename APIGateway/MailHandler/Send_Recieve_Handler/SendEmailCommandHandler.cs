using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Response.Email;
using APIGateway.Data;
using APIGateway.MailHandler.Service;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.MailHandler.Send_Recieve_Handler
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, SendEmailRespObj>
    {
		private readonly IEmailService _email;
		private readonly UserManager<cor_useraccount> _userManager;
		private readonly IHttpContextAccessor _accessor;
		private readonly ILoggerService _logger; 
		private readonly IWebHostEnvironment _env;
		private readonly IAdminRepository _adminRepo;

		public SendEmailCommandHandler(
			IEmailService emailService,
			IAdminRepository adminRepository,
			UserManager<cor_useraccount> userManager,
			IHttpContextAccessor httpContextAccessor,
			ILoggerService loggerService,  
			IWebHostEnvironment webHostEnvironment)
		{
			_email = emailService;
			_accessor = httpContextAccessor;
			_userManager = userManager;
			_logger = loggerService; 
			_env = webHostEnvironment;
			_adminRepo = adminRepository;
		}
        public async Task<SendEmailRespObj> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
			var response = new SendEmailRespObj { ResponseStatus = (int)EmailStatus.Failed, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
			try
			{ 
				
				if (string.IsNullOrEmpty(request.Content))
				{
					response.Status.Message.FriendlyMessage = "No Message Specified";
					return response;
				} 
				if (!request.ToAddresses.Any())
				{
					response.Status.Message.FriendlyMessage = "No Receiver Specified";
					return response;
				}
				foreach(var recr in request.ToAddresses)
				{
					var useraccount = await _userManager.FindByEmailAsync(recr.Address);
					if(useraccount != null)
					{
						request.UserIds = useraccount.Id;
					}
				} 

				var em = await _email.BuildAndSaveEmail(request);
				em.SendIt = request.SendIt;
				   
				if (request.Template == (int)EmailTemplate.Default)
				{
					var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
						  + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
						  + Path.DirectorySeparatorChar.ToString() + "default.html";

					var builder = new BodyBuilder();
					using (StreamReader SourceReader = File.OpenText(pathToFile))
					{
						builder.HtmlBody = SourceReader.ReadToEnd();
					}
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "PPPPPP");
				}

				if (request.Template == (int)EmailTemplate.LoginDetails)
				{
					var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
						  + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
						  + Path.DirectorySeparatorChar.ToString() + "loginDetails.html";

					var builder = new BodyBuilder();
					using (StreamReader SourceReader = File.OpenText(pathToFile))
					{
						builder.HtmlBody = SourceReader.ReadToEnd();
					}
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "PPPPPP");
				}

				if (request.Template == (int)EmailTemplate.OTP)
				{
					var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
						  + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
						  + Path.DirectorySeparatorChar.ToString() + "otp.html";

					var builder = new BodyBuilder();
					using (StreamReader SourceReader = File.OpenText(pathToFile))
					{
						builder.HtmlBody = SourceReader.ReadToEnd();
					}
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "PPPPPP");
				}

				if (request.Template == (int)EmailTemplate.Login)
				{
					var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
						  + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
						  + Path.DirectorySeparatorChar.ToString() + "login.html";

					var builder = new BodyBuilder();
					using (StreamReader SourceReader = File.OpenText(pathToFile))
					{
						builder.HtmlBody = SourceReader.ReadToEnd();
					}
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "PPPPPP");
				}
				
				if (request.Template == (int)EmailTemplate.Advert)
				{
					var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
						  + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
						  + Path.DirectorySeparatorChar.ToString() + "advert.html";

					var builder = new BodyBuilder();
					using (StreamReader SourceReader = File.OpenText(pathToFile))
					{
						builder.HtmlBody = SourceReader.ReadToEnd();
					}
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "PPPPPP");
				}

				if (request.SendIt) await _email.Send(em);

				response.ResponseStatus = (int)EmailStatus.Sent;
				response.Status.Message.FriendlyMessage = "Email sent";
				return response;
			}
			catch (Exception ex)
			{
				var errorCode = ErrorID.Generate(4);
				_logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
				response.Status.IsSuccessful = false; 
				response.Status.Message.FriendlyMessage = "Error occured!! Unable to process request";
				response.Status.Message.MessageId = errorCode;
				response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
				return response;
			}
        }
		
    }
}
