using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Common;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetAllWorkflowLevelQueryHandler : IRequestHandler<GetAllWorkflowLevelQuery, WorkflowLevelRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ICommonRepository _commonRepository;
        private readonly RoleManager<cor_userrole> _roleManager;
        public GetAllWorkflowLevelQueryHandler(ICommonRepository commonRepository, IWorkflowRepository workflowRepository, RoleManager<cor_userrole> roleManager)
        {
            _repo = workflowRepository;
            _roleManager = roleManager;
            _commonRepository = commonRepository;
        }
        public async Task<WorkflowLevelRespObj> Handle(GetAllWorkflowLevelQuery request, CancellationToken cancellationToken)
        {
            var WKFgroups = await _repo.GetAllWorkflowGroupAsync();
            var JobTitles = await _commonRepository.GetAllJobTitleAsync();
            var list = await _repo.GetAllWorkflowLevelAsync();
            return new WorkflowLevelRespObj
            {
                WorkflowLevels = list.Select(x => new WorkflowLevelObj
                {
                    Active = x.Active,
                    CanModify = x.CanModify,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn, 
                    LimitAmount = x.LimitAmount,
                    Position = x.Position,
                    RequiredLimit = x.RequiredLimit,
                    JobTitleId = x?.RoleId ?? string.Empty,
                    JobTitleName = JobTitles.FirstOrDefault(c => c.JobTitleId.ToString().ToLower().Trim() == x.RoleId.Trim().ToLower())?.Name ?? string.Empty,
                    UpdatedBy = x?.UpdatedBy ?? string.Empty,
                    UpdatedOn = x?.UpdatedOn,
                    WorkflowGroupId = x.WorkflowGroupId,
                    WorkflowGroupName = WKFgroups.FirstOrDefault(c => c.WorkflowGroupId == x.WorkflowGroupId)?.WorkflowGroupName ?? string.Empty,
                    WorkflowLevelId = x.WorkflowLevelId,
                    WorkflowLevelName = x.WorkflowLevelName ?? string.Empty
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No record found"
                    }
                }
            };
        }
    }
}
