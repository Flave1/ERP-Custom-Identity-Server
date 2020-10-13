using GODP.APIsContinuation.DomainObjects.Operation;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Workflow
{
    public partial class cor_workflow : GeneralEntity
    {
        public cor_workflow()
        {
            cor_workflowaccess = new HashSet<cor_workflowaccess>();
            cor_workflowdetails = new HashSet<cor_workflowdetails>();
        }

        [Key]
        public int WorkflowId { get; set; }

        [Required]
        [StringLength(250)]
        public string WorkflowName { get; set; }

        public int OperationId { get; set; }

        public int WorkflowAccessId { get; set; }

        public int ApprovalStatusId { get; set; } 
         

        public virtual cor_operation cor_operation { get; set; }

        public virtual ICollection<cor_workflowaccess> cor_workflowaccess { get; set; }

        public virtual ICollection<cor_workflowdetails> cor_workflowdetails { get; set; }
    }
}
