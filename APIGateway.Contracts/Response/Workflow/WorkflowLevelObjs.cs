using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Workflow
{
    public class WorkflowLevelObj
    {
        public int WorkflowLevelId { get; set; }

        public string WorkflowLevelName { get; set; }

        public int WorkflowGroupId { get; set; }

        public int Position { get; set; }

        public string JobTitleId { get; set; }

        public bool RequiredLimit { get; set; }

        public decimal? LimitAmount { get; set; }

        public bool? CanModify { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //..................
        public string WorkflowGroupName { get; set; }
        public string JobTitleName { get; set; }
        public int ExcelLineNumber { get; set; }
    }

    public class WorkflowLevelRegRespObj
    {
        public int WorkflowLevelId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowLevelRespObj
    {
        public List<WorkflowLevelObj> WorkflowLevels { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
