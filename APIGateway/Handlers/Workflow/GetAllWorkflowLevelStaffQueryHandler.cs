using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetAllWorkflowLevelStaffQueryHandler : IRequestHandler<GetAllWorkflowLevelStaffQuery, WorkflowlevelStafRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly IAdminRepository _adminRepo;
        private readonly ICompanyRepository _companyRepository;
        public GetAllWorkflowLevelStaffQueryHandler(ICompanyRepository companyRepository, IAdminRepository adminRepository, IWorkflowRepository repository)
        {
            _companyRepository = companyRepository;
            _repo = repository;
            _adminRepo = adminRepository;
        }
        private async Task<int> GetStaffAccessLevel(int staffId)
        {
            var staffList = await _adminRepo.GetAllStaffAsync();
            var level =  staffList.FirstOrDefault(d => d.StaffId == staffId)?.AccessLevel ?? 0;
            return level;
        }
        public async Task<WorkflowlevelStafRespObj> Handle(GetAllWorkflowLevelStaffQuery request, CancellationToken cancellationToken)
        {
            var wkfGroupList = await _repo.GetAllWorkflowGroupAsync();
            var staffList = await _adminRepo.GetAllStaffAsync();
            var wkfLevel = await _repo.GetAllWorkflowLevelAsync();
            var Definition = await _companyRepository.GetAllCompanyStructureDefinitionAsync();
            
            var list = await _repo.GetAllWorkflowLevelStaffAsync();
            return new WorkflowlevelStafRespObj
            {
                WorkflowlevelStaffs = list.Select(x => new WorkflowlevelStaffObj
                {
                    Active = x.Active,
                    WorkflowLevelStaffId = x.WorkflowLevelStaffId,
                    CreatedBy = x?.CreatedBy,
                    StaffName = staffList.FirstOrDefault(c => c.StaffId == x.StaffId)?.FirstName +" "+ staffList.FirstOrDefault(c => c.StaffId == x.StaffId)?.LastName,
                    StaffId = x.StaffId,
                    WorkflowGroupId = x.WorkflowGroupId,
                    WorkflowGroupName = wkfGroupList.FirstOrDefault(c => c.WorkflowGroupId == x.WorkflowGroupId)?.WorkflowGroupName,
                    WorkflowLevelId = x.WorkflowLevelId,
                    WorkflowLevelName = wkfLevel.FirstOrDefault(c => c.WorkflowLevelId == x.WorkflowLevelId)?.WorkflowLevelName,
                    AccessLevel = Definition.FirstOrDefault(o => o.StructureDefinitionId == GetStaffAccessLevel(x.StaffId)?.Result)?.Definition



                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete ! No Record Found",
                        
                    }
                }
            };
        }
    }
}
