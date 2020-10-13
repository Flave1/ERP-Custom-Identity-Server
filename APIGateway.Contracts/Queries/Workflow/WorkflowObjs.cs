using APIGateway.Contracts.Response.Workflow;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Workflow
{
	public class GetWorkflowByOperationQuery : IRequest<WorkflowRespObj>
	{
		public GetWorkflowByOperationQuery() { }
		public int OperationId { get; set; }
		public GetWorkflowByOperationQuery(int operationId)
		{
			OperationId = operationId;
		}

		public class GetSingleWorkflowQuery : IRequest<WorkflowRespObj> {
			public GetSingleWorkflowQuery() { }
			public int WorkflowId { get; set; }
			public GetSingleWorkflowQuery(int workflowId)
			{
				WorkflowId = workflowId;
			}
		}

		public class GetWorkflowdetailQuery : IRequest<WorkflowDetailsRespObj>
		{
			public GetWorkflowdetailQuery() { }
			public int WorkflowId { get; set; }
			public GetWorkflowdetailQuery(int workflowId)
			{
				WorkflowId = workflowId;
			}
		}

		
	}
}
