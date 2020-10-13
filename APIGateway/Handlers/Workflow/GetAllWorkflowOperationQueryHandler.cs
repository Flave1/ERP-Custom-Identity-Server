using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GOSLibraries.GOS_API_Response; 
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetAllWorkflowOperationQueryHandler : IRequestHandler<GetAllWorkflowOperationQuery, WorkflowOperationRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public GetAllWorkflowOperationQueryHandler(IWorkflowRepository repository)
        {
            _repo = repository;
        }
        public async  Task<WorkflowOperationRespObj> Handle(GetAllWorkflowOperationQuery request, CancellationToken cancellationToken)
        {
            var wkfOperationTypeList = await _repo.GetAllOperationTypesAsync();
            var list = await _repo.GetAllOperationAsync();
            return new WorkflowOperationRespObj
            {
                WorkflowOperations = list.Select(x => new WorkflowOperationObj
                {
                    EnableWorkflow = x.EnableWorkflow,
                    OperationId = x.OperationId,
                    OperationName = x.OperationName,
                    OperationTypeId = x.OperationTypeId,
                    OperationTypeName = wkfOperationTypeList.FirstOrDefault(d => d.OperationTypeId == x.OperationTypeId)?.OperationTypeName,

                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No Record found"
                    }
                }
            };
        }
    }
}
