using APIGateway.Contracts.Response.Workflow;
using GODPAPIs.Contracts.RequestResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Workflow
{
    public class AddUpdateWorkflowLevelStaffCommand : IRequest<WorkflowlevelStaffRegRespObj>
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
    }

    public class DeleteWorkflowLevelStaffCommand : IRequest<DeleteRespObj>
    {
        public List<int> WorkflowLevelStaffIds { get; set; }
    }
}
