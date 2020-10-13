namespace GODP.APIsContinuation.DomainObjects.Company
{ 
    using GODP.APIsContinuation.DomainObjects.Others;
    using GODP.APIsContinuation.DomainObjects.Staff;
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_country : GeneralEntity
    {
        public cor_country()
        {
            cor_company = new HashSet<cor_company>();
            cor_publicholiday = new HashSet<cor_publicholiday>();
            cor_staff = new HashSet<cor_staff>();
            cor_state = new HashSet<cor_state>(); 
        }

        [Key]
        public int CountryId { get; set; }

        [Required]
        [StringLength(10)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(250)]
        public string CountryName { get; set; }

        
        public virtual ICollection<cor_company> cor_company { get; set; }

        
        public virtual ICollection<cor_publicholiday> cor_publicholiday { get; set; }

        
        public virtual ICollection<cor_staff> cor_staff { get; set; }

        
        public virtual ICollection<cor_state> cor_state { get; set; }

        
        //public virtual ICollection<credit_loancustomer> credit_loancustomer { get; set; }
    }
}
