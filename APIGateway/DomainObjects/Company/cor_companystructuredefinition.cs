namespace GODP.APIsContinuation.DomainObjects.Ifrs
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    public partial class cor_companystructuredefinition : GeneralEntity
    {
        [Key]
        public int StructureDefinitionId { get; set; }

        [Required]
        [StringLength(50)]
        public string Definition { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public int StructureLevel { get; set; }
         
        public bool IsMultiCompany { get; set; }

        public int? OperatingLevel { get; set; }
    }
}
