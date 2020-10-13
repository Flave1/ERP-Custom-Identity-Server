using APIGateway.Contracts.Queries.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetAnApproverWorkflowTaskQueryHnadler : IRequestHandler<GetAnApproverWorkflowTaskQuery, WorkflowTaskRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly IAdminRepository _adminRepo;
        public GetAnApproverWorkflowTaskQueryHnadler(IWorkflowRepository workflowRepository, UserManager<cor_useraccount> userManager,
            IHttpContextAccessor httpContextAccessor, IAdminRepository adminRepository)
        {
            _accessor = httpContextAccessor;
            _repo = workflowRepository;
            _adminRepo = adminRepository;
            _userManager = userManager;
        }
        public async Task<WorkflowTaskRespObj> Handle(GetAnApproverWorkflowTaskQuery request, CancellationToken cancellationToken)
        {
            var loggedInUserId = _accessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
            var loggedInUser = await _userManager.FindByIdAsync(loggedInUserId);
            var currentStaff = await _adminRepo.GetStaffAsync(loggedInUser.StaffId);

            var list = await _repo.GetAnApproverWorkflowTaskAsync(loggedInUser.Email, currentStaff.AccessLevel??0); 

            return new WorkflowTaskRespObj
            {
                workflowTasks = list.Select(s => new WorkflowTaskObj
                {
                    ApprovalStatus = s.ApprovalStatus,
                    Comment = s.Comment,
                    DefferedExecution = s.DefferedExecution,
                    OperationId = s.OperationId,
                    StaffAccessId = s.StaffAccessId,
                    StaffEmail = s.StaffEmail,
                    StaffId = s.StaffId,
                    StaffRoles = s.StaffRoles,
                    TargetId = s.TargetId,
                    WorkflowId = s.WorkflowId,
                    WorkflowTaskId = s.WorkflowTaskId,
                    WorkflowTaskStatus = s.WorkflowTaskStatus,
                    WorkflowToken = s.WorkflowToken, 
                }).ToList(),
                Status = new APIResponseStatus { IsSuccessful = true,
                    Message = new APIResponseMessage { FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No Record Found" }
                }
            };
        }
    }
}
