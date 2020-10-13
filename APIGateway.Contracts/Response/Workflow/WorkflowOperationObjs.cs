using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Workflow
{
    public class WorkflowOperationObj
    {
        public int OperationId { get; set; }

        public string OperationName { get; set; }

        public int OperationTypeId { get; set; }

        public bool? EnableWorkflow { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }
         
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime? UpdatedOn { get; set; }

        //.....................
        public string OperationTypeName { get; set; } 
        //.......................
    }

    

 
    public class WorkflowOperationRespObj
    {
        public List<WorkflowOperationObj> WorkflowOperations { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowOperationRegRespObj
    {
        public int WorkflowOperationId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowOperationTypeObj
    { 
        public int OperationTypeId { get; set; } 
        public int ModuleId { get; set; } 
        public string OperationTypeName { get; set; } 
        public bool? Active { get; set; } 
        public bool? Deleted { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime? UpdatedOn { get; set; }
    }

    public class WorkflowOperationTypeRespObj
    {
        public List<WorkflowOperationTypeObj> WorkflowOperationTypes { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
