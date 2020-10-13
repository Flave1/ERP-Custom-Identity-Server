using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Data;
using APIGateway.Repository.Interface.Workflow;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Commands.Supplier;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class DeleteWorkflowCommandHandler : IRequestHandler<DeleteWorkflowCommand, DeleteRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public DeleteWorkflowCommandHandler(IWorkflowRepository workflowRepository, DataContext dataContext, ILoggerService loggerService)
        {
            _repo = workflowRepository;
            _dataContext = dataContext;
            _logger = loggerService;
        }
        public async Task<DeleteRespObj> Handle(DeleteWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.WorkflowIds.Count() > 0)
                {
                    foreach (var itemId in request.WorkflowIds)
                    {
                         _repo.DeleteWorkflowAsync(itemId);
                    } 
                }
                else
                {
                    return new DeleteRespObj
                    {

                        Status = new APIResponseStatus
                        {
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Id(s) Required",
                            }
                        }
                    };
                }
                return new DeleteRespObj
                {
                    Deleted = true,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Item(s) deleted succcessfully",
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DeleteRespObj
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
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
