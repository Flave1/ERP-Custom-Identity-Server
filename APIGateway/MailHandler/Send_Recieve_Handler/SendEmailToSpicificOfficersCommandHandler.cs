using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Response.Email;
using APIGateway.Data;
using APIGateway.MailHandler.Service;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.UserAccount;
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
    public class SendEmailToSpicificOfficersCommandHandler : IRequestHandler<SendEmailToSpicificOfficersCommand, SendEmailRespObj>
    {
		private readonly IEmailService _email;
		private readonly UserManager<cor_useraccount> _userManager;
		private readonly IHttpContextAccessor _accessor;
		private readonly ILoggerService _logger; 
		private readonly IWebHostEnvironment _env;
		private readonly IMapper _mapper;
		private readonly DataContext _dataContext;
		private readonly RoleManager<cor_userrole> _roleManager;
		
		public SendEmailToSpicificOfficersCommandHandler(
			IEmailService emailService, 
			UserManager<cor_useraccount> userManager,
			IHttpContextAccessor httpContextAccessor,
			ILoggerService loggerService,  
			IWebHostEnvironment webHostEnvironment,
			DataContext dataContext,
			RoleManager<cor_userrole> roleManager,
			IMapper mapper)
		{
			_mapper = mapper;
			_email = emailService;
			_roleManager = roleManager;
			_dataContext = dataContext;
			_accessor = httpContextAccessor;
			_userManager = userManager;
			_logger = loggerService; 
			_env = webHostEnvironment;
		}
        public async Task<SendEmailRespObj> Handle(SendEmailToSpicificOfficersCommand request, CancellationToken cancellationToken)
        {
			var response = new SendEmailRespObj { ResponseStatus = (int)EmailStatus.Failed, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
			try
			{
				var userId = _accessor.HttpContext.User?.FindFirst(f => f.Type == "userId")?.Value;
				var user = await _userManager.FindByIdAsync(userId); 
				if (string.IsNullOrEmpty(request.Content))
				{
					response.Status.Message.FriendlyMessage = "No Message Specified";
					return response;
				} 
				_logger.Information(request.ActivitIds.ToString());

				var activities = _dataContext.cor_userroleactivity.Where(q => request.ActivitIds.Contains(q.ActivityId)).ToList();
				var userRoles = _roleManager.Roles.Where(w => activities.Select(q => q.RoleId).Contains(w.Id)).ToList();

				//var identityRoles = (from a in _dataContext.UserRoles
				//		   join b in userRoles on a.RoleId equals b.Id
				//		   ).ToList();

				var identityRoles = _dataContext.UserRoles.Where(q => userRoles.Select(w => w.Id).Contains(q.RoleId)).ToList();

				var usersBasedOnIdentityRoles = _userManager.Users.Where(q => identityRoles.Select(e => e.UserId).Contains(q.Id)).ToList();
				SendEmailCommand req = _mapper.Map<SendEmailCommand>(request);

				if(usersBasedOnIdentityRoles.Count() == 0)
				{
					response.Status.Message.FriendlyMessage = "No Receiver Found";
					return response;
				}
				req.ToAddresses = new List<EmailAddressCommand>();
				
				foreach(var staff in usersBasedOnIdentityRoles)
				{
					var to = new EmailAddressCommand
					{
						Address = staff.Email,
						Name = staff.UserName,
					};
					req.ToAddresses.Add(to);
				} 

				var em = await _email.BuildAndSaveEmail(req);

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
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "flave");
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
					em.Content = string.Format(builder.HtmlBody, request.Content.Replace("{", "").Replace("}", ""), "flave");
				}
				em.Module = (int)Modules.CENTRAL;
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
