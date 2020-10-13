using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Workflow
{
    public class WorkflowlevelStaffObj
    {
        public int WorkflowLevelStaffId { get; set; }

        public int StaffId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; } 
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        //..............
        public string WorkflowLevelName { get; set; } 
        public string StaffName { get; set; }
        public string StaffCode { get; set; }
        public string AccessLevel { get; set; }
        public string WorkflowGroupName { get; set; }
        public int ExcelLineNumber { get; set; }
    }
    public class WorkflowlevelStafRespObj
    {
        public List<WorkflowlevelStaffObj> WorkflowlevelStaffs { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class WorkflowlevelStaffRegRespObj
    {
        public int WorkflowLevelStaffId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
