using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GODP.APIsContinuation.DomainObjects.Workflow
{


    public partial class cor_workflowdetails : GeneralEntity
    {
       
        [Key]
        public int WorkflowDetailId { get; set; }

        public int WorkflowId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }
        public int OperationId { get; set; }

        public int AccessId { get; set; }

        [StringLength(50)]
        public string OfficeAccessIds { get; set; }

        public int Position { get; set; } 

        public virtual cor_workflow cor_workflow { get; set; }

        public virtual ICollection<cor_workflowdetailsaccess> cor_workflowdetailsaccess { get; set; }
    }
}
