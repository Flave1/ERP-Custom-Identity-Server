namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_maritalstatus : GeneralEntity
    {
        public cor_maritalstatus()
        {
            //credit_loancustomer = new HashSet<credit_loancustomer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaritalStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        //public virtual ICollection<credit_loancustomer> credit_loancustomer { get; set; }
    }
}
