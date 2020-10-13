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
    public class GetWorkflowLevelByIdQueryHandler : IRequestHandler<GetWorkflowLevelByIdQuery, WorkflowLevelRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly ICommonRepository _commonRepository;
        public GetWorkflowLevelByIdQueryHandler(ICommonRepository commonRepository, IWorkflowRepository workflowRepository, RoleManager<cor_userrole> roleManager)
        {
            _commonRepository = commonRepository;
            _repo = workflowRepository;
            _roleManager = roleManager;
        }
        public async Task<WorkflowLevelRespObj> Handle(GetWorkflowLevelByIdQuery request, CancellationToken cancellationToken)
        {
            var WKFgroups = await _repo.GetAllWorkflowGroupAsync();
            var JobTitles = await _commonRepository.GetAllJobTitleAsync();
            var item = await _repo.GetWorkflowLevelAsync(request.WorkflowLevelId);
            var itemRespList = new List<WorkflowLevelObj>();
            if(item != null)
            {
                var respItem = new WorkflowLevelObj()
                {
                    Active = item.Active,
                    CanModify = item.CanModify,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    Deleted = item.Deleted,
                    LimitAmount = item.LimitAmount,
                    Position = item.Position,
                    RequiredLimit = item.RequiredLimit,
                    JobTitleId = item.RoleId,
                    JobTitleName = JobTitles.FirstOrDefault(c => c.JobTitleId.ToString() == item.RoleId)?.Name ?? string.Empty,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedOn = item.UpdatedOn,
                    WorkflowGroupId = item.WorkflowGroupId,
                    WorkflowGroupName = WKFgroups.FirstOrDefault(c => c.WorkflowGroupId == item.WorkflowGroupId).WorkflowGroupName,
                    WorkflowLevelId = item.WorkflowLevelId,
                    WorkflowLevelName = item.WorkflowLevelName
                };
                itemRespList.Add(respItem);
            }
            return new WorkflowLevelRespObj
            {
                WorkflowLevels = itemRespList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = item != null ? null : "Search Complete! No record found"
                    }
                }
            };
        }
    }
}
