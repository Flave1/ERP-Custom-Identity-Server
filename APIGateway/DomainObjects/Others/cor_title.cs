namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_title : GeneralEntity
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TitleId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
         
    }
}
