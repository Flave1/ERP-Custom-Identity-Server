namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    using GODPAPIs.Contracts.GeneralExtension;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class cor_userroleadditionalactivity : GeneralEntity
    {
        [Key]
        public int UserRoleAdditionalActivityId { get; set; }

        public string UserId { get; set; }

        public int ActivityId { get; set; }

        public bool? CanEdit { get; set; }

        public bool? CanAdd { get; set; }

        public bool? CanView { get; set; }

        public bool? CanDelete { get; set; }

        public bool? CanApprove { get; set; }

        public virtual cor_activity cor_activity { get; set; } 
        [NotMapped]
        public virtual cor_useraccount cor_useraccount { get; set; }
    }
}
