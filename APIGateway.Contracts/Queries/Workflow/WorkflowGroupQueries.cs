using APIGateway.Contracts.Response.Workflow;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Workflow
{
    public class GetWorkflowGroupQuery : IRequest<WorkflowGroupRespObj>
    {
        public GetWorkflowGroupQuery() { }
        public int WorkflowGroupId { get; set; }
        public GetWorkflowGroupQuery(int workflowGroupId)
        {
            WorkflowGroupId = workflowGroupId;
        }
    }

    public class GetAllOperationTypesQuery : IRequest<WorkflowOperationTypeRespObj> { }
    public class GetAllOperationQuery : IRequest<OperationRespObj> { }

    public class GetAllWorkflowGroupQuery : IRequest<WorkflowGroupRespObj> { }
}
