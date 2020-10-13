using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Workflow
{
    public partial class cor_workflowlevel : GeneralEntity
    {


        [Key]
        public int WorkflowLevelId { get; set; }

        [Required]
        [StringLength(250)]
        public string WorkflowLevelName { get; set; }

        public int WorkflowGroupId { get; set; }

        public int Position { get; set; }

        public string RoleId { get; set; }

        public bool RequiredLimit { get; set; }

        public decimal? LimitAmount { get; set; }

        public bool? CanModify { get; set; } 

        public virtual cor_workflowgroup cor_workflowgroup { get; set; }
        public virtual ICollection<cor_workflowlevelstaff> cor_workflowlevelstaff { get; set; }

        public virtual ICollection<cor_workflowtrail> cor_workflowtrail { get; set; }
    }
}
