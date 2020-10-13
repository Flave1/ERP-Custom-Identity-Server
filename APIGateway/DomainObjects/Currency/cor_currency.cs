namespace GODP.APIsContinuation.DomainObjects.Currency
{
    using GODP.APIsContinuation.DomainObjects.Company;
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_currency : GeneralEntity
    {
        public cor_currency()
        {
            cor_company = new HashSet<cor_company>();
            cor_currencyrate = new HashSet<cor_currencyrate>();
        }

        [Key]
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(10)]
        public string CurrencyCode { get; set; }

        [Required]
        [StringLength(250)]
        public string CurrencyName { get; set; }

        public bool? BaseCurrency { get; set; }

        public bool? INUSE { get; set; }

        
        public virtual ICollection<cor_company> cor_company { get; set; }

        
        public virtual ICollection<cor_currencyrate> cor_currencyrate { get; set; }

        
        //public virtual ICollection<credit_collateralcustomer> credit_collateralcustomer { get; set; }

        
        //public virtual ICollection<credit_loanapplication> credit_loanapplication { get; set; }

        
        //public virtual ICollection<fin_transaction> fin_transaction { get; set; }
    }
}
