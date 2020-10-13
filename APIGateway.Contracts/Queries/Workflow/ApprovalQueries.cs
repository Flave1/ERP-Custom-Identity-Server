using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Workflow
{
	public class GetAnApproverWorkflowTaskQuery : IRequest<WorkflowTaskRespObj> { } 
}
