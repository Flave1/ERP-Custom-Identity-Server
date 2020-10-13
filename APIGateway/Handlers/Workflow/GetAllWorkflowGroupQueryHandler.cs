using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.RequestResponse.Workflow; 
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetAllWorkflowGroupQueryHandler : IRequestHandler<GetAllWorkflowGroupQuery, WorkflowGroupRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public GetAllWorkflowGroupQueryHandler(IWorkflowRepository workflowRepository)
        {
            _repo = workflowRepository;
        }
        public async Task<WorkflowGroupRespObj> Handle(GetAllWorkflowGroupQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllWorkflowGroupAsync();
            return new WorkflowGroupRespObj
            {
                WorkflowGroups = list.Select(x => new WorkflowGroupObj
                {
                    Active = x.Active,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn,
                    WorkflowGroupId = x.WorkflowGroupId,
                    WorkflowGroupName = x.WorkflowGroupName,
                    
                }).ToList(),
                Status = new APIResponseStatus
                {
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No record found"
                    }
                }
            };
        }
    }
}
