using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
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
    public class GetWorkflowByOperationQueryHandler : IRequestHandler<GetWorkflowByOperationQuery, WorkflowRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ICompanyRepository _compRepo;                                      
        public GetWorkflowByOperationQueryHandler(IWorkflowRepository workflowRepository, ICompanyRepository companyRepository)
        {
            _repo = workflowRepository;
            _compRepo = companyRepository;
        }
        public async Task<WorkflowRespObj> Handle(GetWorkflowByOperationQuery request, CancellationToken cancellationToken)
        {
            var compStrDefList = await _compRepo.GetAllCompanyStructureDefinitionAsync();
            var list = await _repo.GetWorkflowByOperationAsync(request.OperationId);
            var operations = await _repo.GetAllOperationAsync();
            var respItemList = new List<WorkflowLevelObj>();

            return new WorkflowRespObj
            {
                Workflows = list.Select(x => new WorkflowObj
                {
                    WorkflowId = x.WorkflowId,
                    WorkflowName = x?.WorkflowName,
                    WorkflowAccessId = x.WorkflowAccessId,
                    OperationId = x.OperationId,
                    OperationName = operations.FirstOrDefault(d => d.OperationId == x.OperationId)?.OperationName,
                    AccessName = compStrDefList.FirstOrDefault(s => s.StructureDefinitionId == x.WorkflowAccessId)?.Definition
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
