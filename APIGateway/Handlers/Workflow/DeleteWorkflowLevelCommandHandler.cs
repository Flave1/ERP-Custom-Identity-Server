using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Repository.Interface.Workflow;

using GOSLibraries.GOS_Error_logger.Service;
using GODPAPIs.Contracts.RequestResponse; 
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class DeleteWorkflowLevelCommandHandler : IRequestHandler<DeleteWorkflowLevelCommand, DeleteRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly IWorkflowRepository _repo;
        public DeleteWorkflowLevelCommandHandler(ILoggerService loggerService, IWorkflowRepository repository)
        {
            _repo = repository;
            _logger = loggerService;
        }
        public async Task<DeleteRespObj> Handle(DeleteWorkflowLevelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.WorkflowLevelIds.Count() < 1)
                    return new DeleteRespObj
                    {

                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Item Seleted",
                            }
                        }
                    };

                foreach (var itemId in request.WorkflowLevelIds)
                {
                    await _repo.DeleteWorkflowLevelAsync(itemId);
                }
                return new DeleteRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Deleted Successfully",
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : DeleteWorkflowLevelCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DeleteRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : DeleteWorkflowLevelCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
