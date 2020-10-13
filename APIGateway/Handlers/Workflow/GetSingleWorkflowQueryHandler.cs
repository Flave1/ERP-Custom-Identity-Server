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
using static APIGateway.Contracts.Queries.Workflow.GetWorkflowByOperationQuery;

namespace APIGateway.Handlers.Workflow
{
    public class GetSingleWorkflowQueryHandler : IRequestHandler<GetSingleWorkflowQuery, WorkflowRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ICompanyRepository _compRepo;
        public GetSingleWorkflowQueryHandler(IWorkflowRepository workflowRepository, ICompanyRepository companyRepository)
        {
            _repo = workflowRepository;
            _compRepo = companyRepository;
        }
        public async Task<WorkflowRespObj> Handle(GetSingleWorkflowQuery request, CancellationToken cancellationToken)
        {
            var WorkflowList = await _repo.GetAllWorkflowAsync();
            var WorkflowDetailList = await _repo.GetAllWorkflowdetailsAsync();
            var WorkflowAccessList = await _repo.GetAllWorkflowAccessAsync();
            var WorkflowDetailAccessList = await _repo.GetAllWorkflowdetailAccessAsync();
            var respItemList = new List<WorkflowObj>();

            var data = (from a in WorkflowList
                        join b in WorkflowDetailList on a.WorkflowId equals b.WorkflowId
                        into detls
                        from b in detls.DefaultIfEmpty()
                        where a.WorkflowId == request.WorkflowId && a.Deleted == false
                        orderby a.WorkflowName
                        select new WorkflowObj
                        {
                            WorkflowId = a.WorkflowId,
                            WorkflowName = a.WorkflowName,
                            WorkflowAccessId = a.WorkflowAccessId,

                            OperationId = a.OperationId,
                            WorkflowDetails = detls.Select(x => new WorkflowDetailObj
                            {
                                WorkflowDetailId = x.WorkflowDetailId,
                                WorkflowGroupId = x.WorkflowGroupId,
                                WorkflowLevelId = x.WorkflowLevelId,
                                AccessId = x.AccessId,
                                Position = x.Position,
                            }).ToList()
                        }).FirstOrDefault();
            if (data != null)
            {
                data.WorkflowAccessIds = WorkflowAccessList.Where(x => x.WorkflowId == data.WorkflowId).Select(l => (int)l.OfficeAccessId).ToArray();
                if (data.WorkflowDetails.Count > 0)
                {
                    foreach (var item in data.WorkflowDetails)
                    {
                        item.AccessOfficeIds = WorkflowDetailAccessList.Where(x => x.WorkflowDetailId == item.WorkflowDetailId).Select(l => (int)l.OfficeAccessId).ToArray();
                    }
                }
                respItemList.Add(data);
            }
            return new WorkflowRespObj
            {
                Workflows = respItemList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = respItemList.Count() > 0 ? null : "Search Complete! No Record Found"
                    }
                }
            };

        }
    }
}
