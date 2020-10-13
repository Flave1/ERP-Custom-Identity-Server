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
    public class GetAllOperationQueryHandler : IRequestHandler<GetAllOperationQuery, OperationRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public GetAllOperationQueryHandler(IWorkflowRepository repository)
        {
            _repo = repository;
        }
        public async Task<OperationRespObj> Handle(GetAllOperationQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllOperationAsync();
            var parentList = await _repo.GetAllOperationTypesAsync();
            return new OperationRespObj
            {
                Operations = list.Select(x => new OperationObj
                {
                    Active = x.Active,
                    EnableWorkflow = (bool)x.EnableWorkflow,
                    OperationId = x.OperationId,
                    OperationName = x.OperationName,
                    OperationTypeId = x.OperationTypeId,
                    OperationTypeName = parentList.FirstOrDefault(d => d.OperationTypeId == x.OperationTypeId)?.OperationTypeName
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0? null : "Search Complete ! No record found"
                    }
                }
            };


        }
    }
}
