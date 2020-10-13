using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.RequestResponse.Workflow
{
    public class WorkflowObj
    {
        public int WorkflowId { get; set; }

        public string WorkflowName { get; set; }

        public int OperationId { get; set; }

        public string OperationName { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }

        public int WorkflowAccessId { get; set; }

        public string AccessName { get; set; }

        public int Position { get; set; }

        public int[] WorkflowAccessIds { get; set; }
         
        public List<WorkflowDetailObj> WorkflowDetails { get; set; }
    }

    public class WorkflowRespObj
    {
        public List<WorkflowObj> Workflows { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class WorkflowDetailObj
    {
        public int WorkflowDetailId { get; set; }

        public int WorkflowId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }

        public int AccessId { get; set; }

        public int[] AccessOfficeIds { get; set; }

        public int Position { get; set; }
    }

    public class GoForApprovalRespObj
    {
        public bool HasWorkflowAccess { get; set; }
        public bool EnableWorkflow { get; set; }
        public bool ApprovalProcessStarted { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class StaffApprovalRegRespObj
    {
        public int ResponseId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowTaskObj
    {
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
        public string WorkflowToken { get; set; }
    }

    public class WorkflowTaskRespObj
    {
        public List<WorkflowTaskObj> workflowTasks { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowDetailsRespObj
    {
        public List<WorkflowdetailObj> Workflowdetails { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public partial class WorkflowdetailObj
    {

        public int WorkflowDetailId { get; set; }

        public int WorkflowId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }
        public int OperationId { get; set; }

        public int AccessId { get; set; }

        public string OfficeAccessIds { get; set; }

        public int Position { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }

}
