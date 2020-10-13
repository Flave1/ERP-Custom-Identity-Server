using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.DomainObjects.Workflow;

using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class AddUpdateWorkflowLevelStaffCommandHandler : IRequestHandler<AddUpdateWorkflowLevelStaffCommand, WorkflowlevelStaffRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly IWorkflowRepository _repo;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddUpdateWorkflowLevelStaffCommandHandler(ILoggerService loggerService, IWorkflowRepository repository,
            UserManager<cor_useraccount> userManger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerService;
            _repo = repository;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<WorkflowlevelStaffRegRespObj> Handle(AddUpdateWorkflowLevelStaffCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);
                var WKFlevelStaff = new cor_workflowlevelstaff
                {
                    StaffId = request.StaffId,
                    Active = request.Active,
                    CreatedBy = request.CreatedBy,
                    CreatedOn = DateTime.Now,
                    Deleted = false,
                    WorkflowLevelId = request.WorkflowLevelId,
                    UpdatedBy = user.UserName,
                    UpdatedOn =  request.UpdatedOn,
                    WorkflowGroupId = request.WorkflowGroupId, 
                    WorkflowLevelStaffId = request.WorkflowLevelStaffId,
                };
                if(request.WorkflowLevelStaffId > 0)
                {
                    WKFlevelStaff.WorkflowLevelStaffId = request.WorkflowLevelStaffId;
                    WKFlevelStaff.UpdatedOn = DateTime.Now; 
                }
                await _repo.AddUpdateWorkflowLevelStaffAsync(WKFlevelStaff);
                var actionTaken = request.WorkflowGroupId > 0 ? "updated" : "added";
                return new WorkflowlevelStaffRegRespObj
                {
                    WorkflowLevelStaffId = WKFlevelStaff.WorkflowLevelStaffId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Successfully {actionTaken}",
                        }
                    }
                };

            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdateWorkflowLevelStaffCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new WorkflowlevelStaffRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateWorkflowLevelStaffCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }
    }
}
