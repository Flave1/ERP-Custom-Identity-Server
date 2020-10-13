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
    public class GetWorkflowGroupQueryHandler : IRequestHandler<GetWorkflowGroupQuery, WorkflowGroupRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public GetWorkflowGroupQueryHandler(IWorkflowRepository repository)
        {
            _repo = repository;
        }
        public async Task<WorkflowGroupRespObj> Handle(GetWorkflowGroupQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetWorkflowGroupAsync(request.WorkflowGroupId);

            var respItem = new WorkflowGroupObj
            {
                WorkflowGroupId = item.WorkflowGroupId,
                Active = item.Active,
                CreatedBy = item.CreatedBy,
                Deleted = item.Deleted,
                UpdatedBy = item.UpdatedBy,
                UpdatedOn = item.UpdatedOn, 
                WorkflowGroupName = item.WorkflowGroupName,
            };

            

            var respItemList = new List<WorkflowGroupObj>() { respItem };
            return new WorkflowGroupRespObj
            {
                WorkflowGroups = respItemList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = item != null ? null : "Search Complete! No Record Found" 
                    }
                }
            };
        }
    }
}
