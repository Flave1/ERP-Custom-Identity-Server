namespace GODP.APIsContinuation.DomainObjects.Company
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class cor_companystructure : GeneralEntity
    { 

        [Key]
        public int CompanyStructureId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public int? StructureTypeId { get; set; }

        public int? CountryId { get; set; }

        public string Address { get; set; }

        public int? HeadStaffId { get; set; }

        public int? ParentCompanyID { get; set; }

        [StringLength(250)]
        public string Parent { get; set; }

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
 
        public byte[] Logo { get; set; }

        [StringLength(50)]
        public string LogoType { get; set; }
         
        public int? City { get; set; }
         
        public int? State { get; set; }

        public int? CurrencyId { get; set; }

        public int? ReportCurrencyId { get; set; }

        [StringLength(10)]
        public string ApplyRegistryTemplate { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        public bool IsMultiCompany { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int? Subsidairy_Level { get; set; }

        [StringLength(50)]
        public string RegistryTemplate { get; set; }
        public string FSTemplateName { get; set; }
        public string FSTemplate { get; set; }


        //public virtual ICollection<fin_subgeneralledger> fin_subgeneralledger { get; set; }


        //public virtual ICollection<fin_translation_gl> fin_translation_gl { get; set; }
    }
}
