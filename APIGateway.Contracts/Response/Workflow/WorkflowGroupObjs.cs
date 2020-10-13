using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.RequestResponse.Workflow
{
   public class WorkflowGroupObj
    { 
        public int WorkflowGroupId { get; set; }
        public string WorkflowGroupName { get; set; } 
        public bool? Active { get; set; } 
        public bool? Deleted { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime? UpdatedOn { get; set; }
        public int ExcelLineNumber { get; set; }

        //public virtual ICollection<cor_workflowlevel> cor_workflowlevel { get; set; } 
        //public virtual ICollection<cor_workflowlevelstaff> cor_workflowlevelstaff { get; set; }
    }
    public class WorkflowGroupRegRespObj
    {
        public int WorkflowGroupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class WorkflowGroupRespObj
    {
        public List<WorkflowGroupObj> WorkflowGroups { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowGroupOperationTypeResp
    {
        public List<WorkflowGroupOperationTypeObj> WorkflowGroupOperationTypes { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowGroupOperationTypeObj
    { 
        public int OperationTypeId { get; set; } 
        public int ModuleId { get; set; } 
        public string OperationTypeName { get; set; } 
  
    }

    public class OperationObj
    { 
        public int OperationId { get; set; } 
        public string OperationName { get; set; }

        public int OperationTypeId { get; set; }
        public string OperationTypeName { get; set; }

        public bool EnableWorkflow { get; set; }

        public bool? Active { get; set; }
    }

    public class OperationRespObj
    {
        public List<OperationObj> Operations { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WFOperationReObj
    {
        public int TargetId { get; set; }
        public int OperationId { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
    }
}
