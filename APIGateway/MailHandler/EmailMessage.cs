using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace APIGateway.MailHandler
{
	public class EmailMessage : GeneralEntity
	{
		public EmailMessage()
		{
			ToAddresses = new List<EmailAddress>();
			FromAddresses = new List<EmailAddress>();
			//Attachments = new List<MemoryStream>();
		}
		[Key]
		public int EmailMessageId { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public int EmailStatus { get; set; }
		public string SentBy { get; set; }
		public string ReceivedBy { get; set; }
		public string ReceiverUserId { get; set; }
		public int Module { get; set; }
		public DateTime DateSent { get; set; }
		public bool SendIt { get; set; }
		[NotMapped]
		public List<EmailAddress> ToAddresses { get; set; }
		[NotMapped]
		public List<EmailAddress> FromAddresses { get; set; }
		//public List<MemoryStream> Attachments { get; set; }

	}
}
