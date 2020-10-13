namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODP.APIsContinuation.DomainObjects.Company;
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_publicholiday : GeneralEntity
    {
        [Key]
        public int PublicHolidayId { get; set; }

        public int CountryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public virtual cor_country cor_country { get; set; }
    }
}
