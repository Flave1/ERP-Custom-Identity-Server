namespace GODP.APIsContinuation.DomainObjects.Company
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_city : GeneralEntity
    {
         
        [Key]
        public int CityId { get; set; }

        [Required]
        [StringLength(10)]
        public string CityCode { get; set; }

        [Required]
        [StringLength(250)]
        public string CityName { get; set; }

        public int StateId { get; set; }

        public virtual cor_state cor_state { get; set; }

        //public virtual ICollection<credit_loancustomer> credit_loancustomer { get; set; }
    }
}
