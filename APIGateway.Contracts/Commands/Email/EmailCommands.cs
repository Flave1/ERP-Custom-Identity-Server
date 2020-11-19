using APIGateway.Contracts.Response.Email;
using MediatR;
using System.Collections.Generic;

namespace APIGateway.Contracts.Commands.Email
{

	public class SendEmailToSpicificOfficersCommand : IRequest<SendEmailRespObj>
	{
		public SendEmailToSpicificOfficersCommand()
		{
			ToAddresses = new List<EmailAddressCommand>();
			FromAddresses = new List<EmailAddressCommand>();
			//Attachments = new List<byte[]>();
		}
		public string Subject { get; set; }
		public string Content { get; set; }
		public List<EmailAddressCommand> ToAddresses { get; set; }
		public List<EmailAddressCommand> FromAddresses { get; set; }
		//public List<byte[]> Attachments { get; set; }
		public bool SendIt { get; set; }
		public bool SaveIt { get; set; }
		public string UserId { get; set; }
		public int Template { get; set; }
		public List<int> ActivitIds { get; set; }
		public string CallBackUri { get; set; }
	}

	public class SendEmailCommand : IRequest<SendEmailRespObj>
    {
		public SendEmailCommand()
		{
			ToAddresses = new List<EmailAddressCommand>();
			FromAddresses = new List<EmailAddressCommand>();
			//Attachments = new List<byte[]>();
		} 
		public string Subject { get; set; }
		public string Content { get; set; } 
		public List<EmailAddressCommand> ToAddresses { get; set; }
		public List<EmailAddressCommand> FromAddresses { get; set; }
		//public List<byte[]> Attachments { get; set; }
		public bool SendIt { get; set; } 
		public string UserIds { get; set; }
		public List<string> IdentificationId { get; set; }
		public int Module { get; set; }
		public bool SaveIt { get; set; }
		public int Template { get; set; }
		public string CallBackUri { get; set; }
	}

	public class EmailAddressCommand
	{
		public string Name { get; set; }
		public string Address { get; set; }
		//public IFormFile Attachment { get; set; }
	}

	public class AddUpdateEmailConfigCommand : IRequest<SendEmailRespObj>
	{
		public int EmailConfigId { get; set; }
		public string SmtpClient { get; set; }
		public string SenderEmail { get; set; }
		public string SenderUserName { get; set; }
		public string BaseFrontend { get; set; }
		public bool EnableSSL { get; set; }
		public int SMTPPort { get; set; }
		public string MailCaption { get; set; }
		public string SenderPassword { get; set; }
		public bool SendNotification { get; set; }
	}
}
