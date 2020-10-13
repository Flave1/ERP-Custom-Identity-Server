namespace GODP.APIsContinuation.DomainObjects.Company
{
    using GODP.APIsContinuation.DomainObjects.Staff;
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_state : GeneralEntity
    {
        public cor_state()
        {
            cor_city = new HashSet<cor_city>();
            cor_staff = new HashSet<cor_staff>();
        }

        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(10)]
        public string StateCode { get; set; }

        [Required]
        [StringLength(250)]
        public string StateName { get; set; }

        public int CountryId { get; set; }

        
        public virtual ICollection<cor_city> cor_city { get; set; }

        public virtual cor_country cor_country { get; set; }

        
        public virtual ICollection<cor_staff> cor_staff { get; set; }
    }
}
