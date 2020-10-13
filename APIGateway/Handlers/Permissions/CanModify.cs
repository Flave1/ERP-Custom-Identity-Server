using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Admin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Permissions
{
    public class CanModifyQuery : IRequest<bool>
    {
        public class CanModifyQueryHandler : IRequestHandler<CanModifyQuery, bool>
        {
            private readonly IWorkflowRepository _repo;
            private readonly UserManager<cor_useraccount> _userManager;
            private readonly IHttpContextAccessor _accessor;
            private readonly IAdminRepository _adminRepo;
            public CanModifyQueryHandler(
                IWorkflowRepository workflowRepository, 
                UserManager<cor_useraccount> userManager,
                IHttpContextAccessor httpContextAccessor,
                IAdminRepository adminRepository)
            {
                _repo = workflowRepository;
                _userManager = userManager;
                _adminRepo = adminRepository;
                _accessor = httpContextAccessor;
            }
            public async Task<bool> Handle(CanModifyQuery request, CancellationToken cancellationToken)
            {
                var userId =  _accessor.HttpContext.User?.FindFirst(s => s.Type == "userId").Value;
                var user = await _userManager.FindByIdAsync(userId);
                var StaffDetails = await _adminRepo.GetStaffAsync(user.StaffId);
                 
                var _WorkflowLevel = await _repo.GetAllWorkflowLevelAsync();
                var _WorkflowGroup = await _repo.GetAllWorkflowGroupAsync();
                var _WorkflowLevelStaff = await _repo.GetAllWorkflowLevelStaffAsync();

                var result = (from a in _WorkflowLevel
                              join b in _WorkflowGroup on a.WorkflowGroupId equals b.WorkflowGroupId
                              join c in _WorkflowLevelStaff on a.WorkflowLevelId equals c.WorkflowLevelId
                              where a.Deleted == false && a.WorkflowGroupId == b.WorkflowGroupId && c.StaffId == StaffDetails.StaffId
                              select a).FirstOrDefault();
                if(result == null)
                {
                    return false;
                }
                return (bool)result.CanModify;
            }
        }
    }
}
