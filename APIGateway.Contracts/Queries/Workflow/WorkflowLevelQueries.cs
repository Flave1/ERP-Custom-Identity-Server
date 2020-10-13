using APIGateway.Contracts.Response.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Workflow
{
    public class GetAllWorkflowLevelQuery : IRequest<WorkflowLevelRespObj> { }

    public class GetWorkflowLevelByIdQuery : IRequest<WorkflowLevelRespObj>
    {
        public int WorkflowLevelId { get; set; }
        public GetWorkflowLevelByIdQuery() { }
        public GetWorkflowLevelByIdQuery(int workflowLevelId) { WorkflowLevelId = workflowLevelId; }
    }

    public class GetWorkflowLevelsByWorkflowGroupQuery : IRequest<WorkflowLevelRespObj>
    {
        public int WorkflowGroupId { get; set; }
        public GetWorkflowLevelsByWorkflowGroupQuery() { }
        
        public GetWorkflowLevelsByWorkflowGroupQuery(int workflowGroupId) { WorkflowGroupId = workflowGroupId; }
    }

}
