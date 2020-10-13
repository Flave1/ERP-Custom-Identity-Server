namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class cor_userroleactivity : GeneralEntity
    {
        [Key]
        public int UserRoleActivityId { get; set; }

        public string RoleId { get; set; } 

        public int ActivityId { get; set; }

        public bool? CanEdit { get; set; }

        public bool? CanAdd { get; set; }

        public bool? CanView { get; set; }

        public bool? CanDelete { get; set; }

        public bool? CanApprove { get; set; } 
    }
}
