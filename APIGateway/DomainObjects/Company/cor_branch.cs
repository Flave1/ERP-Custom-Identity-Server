using GODP.APIsContinuation.DomainObjects.Account;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Company
{

    public partial class cor_branch : GeneralEntity
    {
        public cor_branch()
        { 
            cor_department = new HashSet<cor_department>();
        }

        [Key]
        public int BranchId { get; set; }

        [Required]
        [StringLength(10)]
        public string BranchCode { get; set; }

        [Required]
        [StringLength(250)]
        public string BranchName { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; } 

        public virtual cor_company cor_company { get; set; }

         


        public virtual ICollection<cor_department> cor_department { get; set; }
    }
}
