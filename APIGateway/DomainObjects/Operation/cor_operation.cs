using GODP.APIsContinuation.DomainObjects.Workflow;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Operation
{

    public partial class cor_operation : GeneralEntity
    {
        public cor_operation()
        {
            cor_workflowaccess = new HashSet<cor_workflowaccess>();
            cor_workflow = new HashSet<cor_workflow>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OperationId { get; set; }

        [Required]
        [StringLength(250)]
        public string OperationName { get; set; }

        public int OperationTypeId { get; set; }

        public bool? EnableWorkflow { get; set; }

        
        //public virtual ICollection<cor_approvalgroupmapping> cor_approvalgroupmapping { get; set; }

        
        //public virtual ICollection<cor_approvaltrail> cor_approvaltrail { get; set; }

        public virtual cor_operationtype cor_operationtype { get; set; }

        
        public virtual ICollection<cor_workflowaccess> cor_workflowaccess { get; set; }

        
        public virtual ICollection<cor_workflow> cor_workflow { get; set; }
    }
}
