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
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class AddUpdateWorkflowLevelCommandHandler : IRequestHandler<AddUpdateWorkflowLevelCommand, WorkflowLevelRegRespObj>
    {
        private readonly IWorkflowRepository _repo; 
        private readonly ILoggerService _logger;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddUpdateWorkflowLevelCommandHandler(IWorkflowRepository workflowRepository, ILoggerService loggerService, 
           IHttpContextAccessor httpContextAccessor, UserManager<cor_useraccount> userManager)
        {
            _logger = loggerService;
            _repo = workflowRepository; 
            _userManger = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<WorkflowLevelRegRespObj> Handle(AddUpdateWorkflowLevelCommand request, CancellationToken cancellationToken)
        {
            var response = new WorkflowLevelRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId")?.Value ?? string.Empty;
                var user = await _userManger.FindByIdAsync(currentUserId);
                if(request.WorkflowLevelId < 1)
                {
                    var wkItem = await _repo.GetWorkflowLevelAsync(request.WorkflowLevelId);
                    if(wkItem != null)
                    {
                        if (wkItem.WorkflowGroupId == request.WorkflowGroupId && wkItem.WorkflowLevelName == request.WorkflowLevelName && wkItem.Position == request.Position)
                        { 
                            response.Status.Message.FriendlyMessage = "Workflow Level with same group and position already exist";
                            return response;
                        }
                        if(wkItem.WorkflowLevelName.Trim().ToLower() == request.WorkflowLevelName.Trim().ToLower())
                        {
                            response.Status.Message.FriendlyMessage = $"Workflow Level  {request.WorkflowLevelName} already exist";
                            return response;
                        }
                    }
                }
                 
                var item = new cor_workflowlevel
                {
                    WorkflowGroupId = request.WorkflowGroupId,
                    Active = request.Active,
                    CanModify = request.CanModify,
                    RequiredLimit = request.RequiredLimit,
                    CreatedBy = user.UserName,
                    Deleted = false,
                    LimitAmount = Convert.ToDecimal(request.LimitAmount),
                    Position = request.Position,
                    RoleId = request.RoleId,
                    WorkflowLevelName = request.WorkflowLevelName,
                    UpdatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                };
                if (request.WorkflowLevelId > 0)
                {
                    item.CreatedOn = request.CreatedOn;
                    item.UpdatedOn = DateTime.Now;
                    item.WorkflowLevelId = request.WorkflowLevelId;
                }
                await _repo.AddUpdateWorkflowLevelAsync(item);

                var actionTaken = request.WorkflowGroupId > 0 ? "updated" : "added";
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = $"Successfuly {actionTaken}";
                return response;

            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new WorkflowLevelRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
