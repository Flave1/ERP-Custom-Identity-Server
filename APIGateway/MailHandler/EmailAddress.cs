using GODPAPIs.Contracts.GeneralExtension;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.MailHandler
{
    public class EmailAddress : GeneralEntity
    {
        [Key]
        public int EmailAddressId { get; set; }
        public int EmailMessageId { get; set; }
        public int Action { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ReceiverUserId { get; set; }
        public int SingleUserStatus { get; set; }
        public byte[] Attachment { get; set; }
    }
}
