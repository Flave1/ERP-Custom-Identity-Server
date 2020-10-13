using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Workflow
{
    public partial class cor_workflowdetailsaccess : GeneralEntity
    {
        [Key]
        public int WorkflowDetailsAccessId { get; set; }

        public int WorkflowDetailId { get; set; }

        public int OfficeAccessId { get; set; } 

        public virtual cor_workflowdetails cor_workflowdetails { get; set; }
    }
}
