using APIGateway.Repository.Interface.Workflow;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static APIGateway.Contracts.Queries.Workflow.GetWorkflowByOperationQuery;

namespace APIGateway.Handlers.Workflow
{

    public class GetWorkflowdetailQueryHandler : IRequestHandler<GetWorkflowdetailQuery, WorkflowDetailsRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public GetWorkflowdetailQueryHandler(IWorkflowRepository repository)
        {
            _repo = repository;
        }
        public async Task<WorkflowDetailsRespObj> Handle(GetWorkflowdetailQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetWorkflowdetailsAsync(request.WorkflowId);
            return new WorkflowDetailsRespObj
            {
                Workflowdetails = list.Select(x => new WorkflowdetailObj
                {
                    Active = x.Active,
                    OperationId = x.OperationId,
                    WorkflowId = x.WorkflowId,
                    AccessId = x.AccessId,
                    CreatedBy = x.CreatedBy,
                    Position = x.Position,
                    WorkflowDetailId = x.WorkflowDetailId,
                    OfficeAccessIds = x.OfficeAccessIds,
                    WorkflowGroupId = x.WorkflowGroupId,
                    WorkflowLevelId = x.WorkflowLevelId,
                    
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete ! No record found"
                    }
                }
            };


        }
    }
}
