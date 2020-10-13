using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Operation
{


    public partial class cor_operationtype : GeneralEntity
    {
        public cor_operationtype()
        {
            cor_operation = new HashSet<cor_operation>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OperationTypeId { get; set; }

        public int ModuleId { get; set; }

        [Required]
        [StringLength(250)]
        public string OperationTypeName { get; set; }
        
        public virtual ICollection<cor_operation> cor_operation { get; set; }
    }
}
