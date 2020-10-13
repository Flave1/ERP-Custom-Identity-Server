namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class cor_useraccess : GeneralEntity
    {
        [Key]
        public int UserAccessLevelId { get; set; }

        public string UserId { get; set; }

        public int AccessLevelId { get; set; } 
        [ForeignKey("UserId")]
        [NotMapped]
        public virtual cor_useraccount cor_useraccount { get; set; }
    }
}
