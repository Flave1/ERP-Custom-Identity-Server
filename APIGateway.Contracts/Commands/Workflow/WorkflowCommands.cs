using APIGateway.Contracts.Response.Workflow;
using GODPAPIs.Contracts.RequestResponse;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Workflow
{
    public class AddUpdateWorkflowCommand : IRequest<WorkflowRespObj>
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

        public List<WorkflowDetailCommand> Details { get; set; }
    }

    public class WorkflowDetailCommand
    {
        public int WorkflowDetailId { get; set; }

        public int WorkflowId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }

        public int AccessId { get; set; }

        public int[] AccessOfficeIds { get; set; }

        public int Position { get; set; }

    }

    public class WorkflowDetailAccessCommand : IRequest<bool>
    {
        public int WorkflowDetailId { get; set; }

        public int WorkflowId { get; set; }

        public int WorkflowGroupId { get; set; }

        public int WorkflowLevelId { get; set; }

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

    public class DeleteWorkflowCommand : IRequest<DeleteRespObj>
    {
        public List<int> WorkflowIds { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class UpdateWorkflowOperationCommand : IRequest<WorkflowOperationRegRespObj>
    {
        public List<wkflowOperation> WkflowOperations { get; set; }
    }
    public class wkflowOperation
    {
        public int OperationId { get; set; }

        public string OperationName { get; set; }

        public int OperationTypeId { get; set; }

        public bool? EnableWorkflow { get; set; }
    }
}