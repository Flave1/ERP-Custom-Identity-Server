using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Email
{
	public class EmailMessageObj 
	{
		public EmailMessageObj()
		{
			ToAddresses = new List<EmailAddressObj>();
			FromAddresses = new List<EmailAddressObj>();
			Attachments = new List<byte[]>();
		} 
		public int EmailMessageId { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public int EmailStatus { get; set; }
		public string SentBy { get; set; }
		public string ReceivedBy { get; set; }
		public DateTime DateSent { get; set; }
		public DateTime DateRead { get; set; }
		public DateTime DateViewed { get; set; }
		public string StatusName { get; set; }
		public List<EmailAddressObj> ToAddresses { get; set; }
		public List<EmailAddressObj> FromAddresses { get; set; }
		public List<byte[]> Attachments { get; set; }
	}

	public class EmailAddressObj
	{
		public int EmailAddressId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public IFormFile Attachment { get; set; }
	}

	public class SendEmailRespObj
	{
		public int ResponseStatus { get; set; }
		public APIResponseStatus Status { get; set; }
	}

	public class EmailRespObj
	{
		public List<EmailMessageObj> Emails { get; set; }
		public int EmailCount { get; set; }
		public APIResponseStatus Status { get; set; }
	}

	public class EmailConfigRespObj
	{
		public List<EmailConfigObj> EmailConfigs { get; set; }
		public APIResponseStatus Status { get; set; }
	}
	public class EmailConfigObj
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
