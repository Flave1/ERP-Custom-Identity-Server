using GODP.APIsContinuation.DomainObjects.Account;  
using GODP.APIsContinuation.DomainObjects.Currency;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Company
{

    public partial class cor_company : GeneralEntity
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Address1 { get; set; }

        [StringLength(500)]
        public string Address2 { get; set; }

        [StringLength(250)]
        public string Telephone { get; set; }

        [StringLength(250)]
        public string Fax { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        [StringLength(50)]
        public string TaxId { get; set; }

        public int? NoOfEmployees { get; set; }

        [StringLength(250)]
        public string WebSite { get; set; }

        [Column(TypeName = "image")]
        public byte[] Logo { get; set; }

        [StringLength(50)]
        public string LogoType { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        public int CountryId { get; set; }

        public int CurrencyId { get; set; }

        [StringLength(10)]
        public string ApplyRegistryTemplate { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        public bool? IsMultiCompany { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int? Subsidairy_Level { get; set; }

        [StringLength(50)]
        public string RegistryTemplate { get; set; }


        //public virtual ICollection<cor_applicationdate> cor_applicationdate { get; set; }


        //public virtual ICollection<cor_approvalgroup> cor_approvalgroup { get; set; }


        //public virtual ICollection<cor_approvaltrail> cor_approvaltrail { get; set; }


        public virtual ICollection<cor_branch> cor_branch { get; set; }


        //public virtual ICollection<cor_chartofaccount> cor_chartofaccount { get; set; }

        public virtual cor_country cor_country { get; set; }

        public virtual cor_currency cor_currency { get; set; }
    }
}
