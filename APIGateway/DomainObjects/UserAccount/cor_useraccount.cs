namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    using GODP.APIsContinuation.DomainObjects.Staff;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class cor_useraccount : IdentityUser
    {

        public int StaffId { get; set; }

        public bool IsFirstLoginAttempt { get; set; } 

        public DateTime? NextPasswordChangeDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]  
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [StringLength(256)]
        public string SecurityAnswer { get; set; }
        public int QuestionId { get; set; }
        public bool IsQuestionTime { get; set; }
        public DateTime EnableAt { get; set; }
    }
}
