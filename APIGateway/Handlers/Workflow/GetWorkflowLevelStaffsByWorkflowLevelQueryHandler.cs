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
    public class GetWorkflowLevelStaffsByWorkflowLevelQueryHandler : IRequestHandler<GetWorkflowLevelStaffsByWorkflowLevelQuery, WorkflowlevelStafRespObj>
    {
        private readonly IWorkflowRepository _repoRepo;
        private readonly IAdminRepository _adminRepo;
        public GetWorkflowLevelStaffsByWorkflowLevelQueryHandler(IWorkflowRepository workflowRepository)
        {
            _repoRepo = workflowRepository;
        }
        public async Task<WorkflowlevelStafRespObj> Handle(GetWorkflowLevelStaffsByWorkflowLevelQuery request, CancellationToken cancellationToken)
        {
            var staffList = await _adminRepo.GetAllStaffAsync();
            var wkfLevelList = await _repoRepo.GetAllWorkflowLevelAsync();
            var wkfLevelStafflist = await _repoRepo.GetWorkflowLevelStaffsByWorkflowLevelAsync(request.WorkflowLevelId);
            //var dsd = (from a in wkfLevelStafflist
            //           join b in staffList on a.StaffId equals b.StaffId
            //           join c in wkfLevelList on a.WorkflowLevelId equals c.WorkflowLevelId )
            return new WorkflowlevelStafRespObj
            {
                WorkflowlevelStaffs = wkfLevelStafflist.Select(x => new WorkflowlevelStaffObj
                {
                    WorkflowLevelId = x.WorkflowLevelId,
                    Active = x.Active,  
                    StaffId = x.StaffId,
                    WorkflowLevelStaffId = x.WorkflowLevelStaffId,
                    StaffName = staffList.FirstOrDefault(z => z.StaffId == x.StaffId).FirstName + "" + staffList.FirstOrDefault(z => z.StaffId == x.StaffId).LastName

                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = wkfLevelStafflist.Count() > 0 ? null : " Search Complete! No record found"
                    }
                }
            };
        }
    }
}
