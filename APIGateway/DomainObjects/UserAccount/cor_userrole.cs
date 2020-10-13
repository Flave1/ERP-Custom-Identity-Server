using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 


namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    public partial class cor_userrole : IdentityRole
    {


        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }


        //public virtual ICollection<cor_useraccountrole> cor_useraccountrole { get; set; }


        public virtual ICollection<cor_userroleactivity> cor_userroleactivity { get; set; }
    }
}
