using GODP.APIsContinuation.DomainObjects.Staff;
using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Workflow
{


    public partial class cor_workflowlevelstaff : GeneralEntity
    {
        [Key]
        public int WorkflowLevelStaffId { get; set; }

        public int StaffId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; } 

        public virtual cor_staff cor_staff { get; set; }

        public virtual cor_workflowgroup cor_workflowgroup { get; set; }

        public virtual cor_workflowlevel cor_workflowlevel { get; set; }
    }
}
