using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.DomainObjects.Workflow;

using GOSLibraries.GOS_Error_logger.Service;
using GODPAPIs.Contracts.Commands.Workflow;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.RequestResponse.Workflow; 
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class AddUpdateWorkflowGroupCommandHandler : IRequestHandler<AddUpdateWorkflowGroupCommand, WorkflowGroupRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly IWorkflowRepository _repo;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddUpdateWorkflowGroupCommandHandler(ILoggerService loggerService, IWorkflowRepository repository,
            UserManager<cor_useraccount> userManger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerService;
            _repo = repository;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<WorkflowGroupRegRespObj> Handle(AddUpdateWorkflowGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.WorkflowGroupId < 1) 
                    if(await _repo.WorkflowGroupExistAsync(request.WorkflowGroupName))
                        return new WorkflowGroupRegRespObj
                        {

                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Workflow gorup already exist",
                                }
                            }
                        };

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);

                var approvalGroup = new cor_workflowgroup
                {
                    WorkflowGroupName = request.WorkflowGroupName,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = user.UserName,
                    UpdatedOn = DateTime.Now,
                };
                if (request.WorkflowGroupId > 0) approvalGroup.WorkflowGroupId = request.WorkflowGroupId;
                await _repo.AddUpdateWorkflowGroupAsync(approvalGroup);

                var actionTaken = request.WorkflowGroupId > 0 ? "updated" : "added";
                return new WorkflowGroupRegRespObj
                {
                    WorkflowGroupId = approvalGroup.WorkflowGroupId,
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
                _logger.Error($"ErrorID : AddUpdateWorkflowGroupCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new WorkflowGroupRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateWorkflowGroupCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
