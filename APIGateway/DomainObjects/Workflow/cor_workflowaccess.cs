using GODP.APIsContinuation.DomainObjects.Operation;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Workflow
{
    public partial class cor_workflowaccess : GeneralEntity
    {
        [Key]
        public int WorkflowCompanyAccessId { get; set; }

        public int WorkflowId { get; set; }

        public int OperationId { get; set; }

        public int OfficeAccessId { get; set; } 
        public virtual cor_operation cor_operation { get; set; }

        public virtual cor_workflow cor_workflow { get; set; }
    }
}
