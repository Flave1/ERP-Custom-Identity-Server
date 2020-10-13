using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
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
    public class GetAllOperationTypesHandler : IRequestHandler<GetAllOperationTypesQuery, WorkflowOperationTypeRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public GetAllOperationTypesHandler(IWorkflowRepository repository)
        {
            _repo = repository;
        }
        public async Task<WorkflowOperationTypeRespObj> Handle(GetAllOperationTypesQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllOperationTypesAsync();
            return new WorkflowOperationTypeRespObj
            {
                    WorkflowOperationTypes = list.Select(x => new WorkflowOperationTypeObj
                    {
                    ModuleId = x.ModuleId,
                    OperationTypeId = x.OperationTypeId,
                    OperationTypeName = x.OperationTypeName,
                    
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
