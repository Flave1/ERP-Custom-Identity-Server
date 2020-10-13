using GODP.APIsContinuation.DomainObjects.Staff;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Workflow
{
    public partial class cor_workflowtrail  : GeneralEntity
    {
        [Key]
        public int WorkflowTrailId { get; set; }

        public int TargetId { get; set; }

        public int cor_staffId { get; set; }
        public int OperationId { get; set; }

        public int WorkflowId { get; set; }

        public int? FromWorkflowLevelId { get; set; }

        public int? ToWorkflowLevelId { get; set; }

        public int ApprovalStatusId { get; set; }

        public int RequestStaffId { get; set; }

        public int? ResponseStaffId { get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime? ActualArrivalDate { get; set; }

        public DateTime? ResponseDate { get; set; } 

        //public virtual cor_approvalstatus cor_approvalstatus { get; set; }

        public virtual cor_workflowlevel cor_workflowlevel { get; set; }
    }
}
