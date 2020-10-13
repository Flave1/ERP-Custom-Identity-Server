namespace GODP.APIsContinuation.DomainObjects.Currency
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System; 
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_currencyrate : GeneralEntity
    {
        [Key]
        public int CurrencyRateId { get; set; }

        public int CurrencyId { get; set; }

        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public double? BuyingRate { get; set; }

        public double? SellingRate { get; set; }

        public virtual cor_currency cor_currency { get; set; }
    }
}
