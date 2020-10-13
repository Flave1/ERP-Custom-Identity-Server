using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DomainObjects.Workflow
{
    public class cor_workflowtask : GeneralEntity
    {
        [Key]
        public int WorkflowTaskId { get; set; }
        public int StaffId { get; set; }
        public int TargetId { get; set; }
        public string Comment { get; set; }
        public int OperationId { get; set; }
        public bool DefferedExecution { get; set; }
        public int WorkflowId { get; set; }
        public string StaffEmail { get; set; }
        public int StaffAccessId { get; set; } 
        public string StaffRoles { get; set; }
        public int WorkflowTaskStatus { get; set; }
        public int ApprovalStatus { get; set; }
        public bool IsMailSent { get; set; }
        public int Position { get; set; }
        public DateTime Date { get; set; }
        public string WorkflowToken { get; set; }
    }
}
