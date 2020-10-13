using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetWorkflowLevelStaffQueryHandler : IRequestHandler<GetWorkflowLevelStaffQuery, WorkflowlevelStafRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly IAdminRepository _adminRepo;
        public GetWorkflowLevelStaffQueryHandler(IAdminRepository adminRepository, IWorkflowRepository repository)
        {
            _repo = repository;
            _adminRepo = adminRepository;
        }
        public async Task<WorkflowlevelStafRespObj> Handle(GetWorkflowLevelStaffQuery request, CancellationToken cancellationToken)
        {
            var wkfGroupList = await _repo.GetAllWorkflowGroupAsync();
            var staffList = await _adminRepo.GetAllStaffAsync();
            var wkfLevel = await _repo.GetAllWorkflowLevelAsync();

            var item = await _repo.GetWorkflowLevelStaffAsync(request.WorkflowLevelStaffId);
            var respItemList = new List<WorkflowlevelStaffObj>();
            if(item != null)
            {
                var respItem = new WorkflowlevelStaffObj()
                {
                    Active = item.Active,
                    WorkflowLevelStaffId = item.WorkflowLevelStaffId,
                    CreatedBy = item?.CreatedBy ?? string.Empty,
                    StaffName = staffList.FirstOrDefault(c => c.StaffId == item.StaffId)?.FirstName ?? string.Empty + " " + staffList.FirstOrDefault(c => c.StaffId == item.StaffId)?.LastName ?? string.Empty,
                    StaffId = item.StaffId,
                    WorkflowGroupId = item.WorkflowGroupId,
                    WorkflowGroupName = wkfGroupList.FirstOrDefault(c => c.WorkflowGroupId == item.WorkflowGroupId)?.WorkflowGroupName ?? string.Empty,
                    WorkflowLevelId = item.WorkflowLevelId,
                    WorkflowLevelName = wkfLevel.FirstOrDefault(c => c.WorkflowLevelId == item.WorkflowLevelId)?.WorkflowLevelName ?? string.Empty,
                };
                respItemList.Add(respItem);
            }
            return new WorkflowlevelStafRespObj
            {
                WorkflowlevelStaffs = respItemList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = item != null ? null : "Search Complete ! No Record Found",

                    }
                }
            };
        }
    }
}
