 
using GODP.APIsContinuation.DomainObjects.Company; 
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.DomainObjects.Workflow;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Staff
{
    public partial class cor_staff : GeneralEntity
    {
     
        [Key]
        public int StaffId { get; set; }

        [StringLength(50)]
        public string StaffCode { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        public int JobTitle { get; set; }

        [StringLength(100)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        public byte[] Photo { get; set; }

        public decimal? StaffLimit { get; set; }

        public int? AccessLevel { get; set; }

        public int? StaffOfficeId { get; set; }

        //public virtual ICollection<cor_passwordhistory> cor_passwordhistory { get; set; }
        public virtual ICollection<cor_workflowlevelstaff> cor_workflowlevelstaff { get; set; } 
        public virtual cor_state cor_state { get; set; }
        public virtual cor_country cor_country { get; set; } 
         
    }
}
