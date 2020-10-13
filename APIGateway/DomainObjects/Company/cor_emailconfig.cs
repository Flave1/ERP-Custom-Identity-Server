using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DomainObjects.Company
{
    public class cor_emailconfig : GeneralEntity
    {
		[Key]
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
