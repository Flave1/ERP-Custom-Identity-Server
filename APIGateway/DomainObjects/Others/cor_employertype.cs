namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_employertype : GeneralEntity
    {
         

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployerTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        //public virtual ICollection<credit_loancustomer> credit_loancustomer { get; set; }
    }
}
