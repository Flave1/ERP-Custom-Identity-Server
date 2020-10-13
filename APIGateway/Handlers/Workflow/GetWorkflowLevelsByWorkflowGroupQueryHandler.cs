using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
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
    public class GetWorkflowLevelsByWorkflowGroupQueryHandler : IRequestHandler<GetWorkflowLevelsByWorkflowGroupQuery, WorkflowLevelRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly RoleManager<cor_userrole> _roleManager;
        public GetWorkflowLevelsByWorkflowGroupQueryHandler(IWorkflowRepository workflowRepository, RoleManager<cor_userrole> roleManager)
        {
            _repo = workflowRepository;
            _roleManager = roleManager;
        }
        public async Task<WorkflowLevelRespObj> Handle(GetWorkflowLevelsByWorkflowGroupQuery request, CancellationToken cancellationToken)
        {
            var WKFgroups = await _repo.GetAllWorkflowGroupAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var list = await _repo.GetWorkflowLevelsByWorkflowGroupAsync(request.WorkflowGroupId);
            return new WorkflowLevelRespObj
            {
                WorkflowLevels = list.Select(x => new WorkflowLevelObj
                {
                    Active = x.Active,
                    CanModify = x.CanModify,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Deleted = x.Deleted,
                    LimitAmount = x.LimitAmount,
                    Position = x.Position,
                    RequiredLimit = x.RequiredLimit,
                    JobTitleId = x.RoleId,
                    JobTitleName = roles.FirstOrDefault(c => c.Id == x.RoleId).Name,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn,
                    WorkflowGroupId = x.WorkflowGroupId,
                    WorkflowGroupName = WKFgroups.FirstOrDefault(c => c.WorkflowGroupId == x.WorkflowGroupId).WorkflowGroupName,
                    WorkflowLevelId = x.WorkflowLevelId,
                    WorkflowLevelName = x.WorkflowLevelName
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
