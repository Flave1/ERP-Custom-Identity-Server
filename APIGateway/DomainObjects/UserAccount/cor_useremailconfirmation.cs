using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DomainObjects.UserAccount
{
    public class cor_useremailconfirmation  : GeneralEntity
    {
        public int cor_useremailconfirmationId { get; set; }
        public string UserId { get; set; }
        public string ConfirnamationTokenCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
