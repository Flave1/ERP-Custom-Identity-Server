using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.Operation; 

using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class UpdateWorkflowOperationCommandHandler : IRequestHandler<UpdateWorkflowOperationCommand, WorkflowOperationRegRespObj>
    {
        private readonly IWorkflowRepository _repo;
        public UpdateWorkflowOperationCommandHandler(IWorkflowRepository workflowRepository)
        {
            _repo = workflowRepository;
        }
        public async Task<WorkflowOperationRegRespObj> Handle(UpdateWorkflowOperationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<cor_operation> wkList = new List<cor_operation>();
                foreach(var wkfOp in request.WkflowOperations)
                {
                    if(wkfOp != null)
                    {
                        var item = new cor_operation
                        {
                            Active = true,
                            Deleted = true,
                            EnableWorkflow = (bool)wkfOp.EnableWorkflow,
                            OperationName = wkfOp.OperationName,
                            OperationTypeId = wkfOp.OperationTypeId,
                            UpdatedOn = DateTime.Today,
                            CompanyId = 1,
                            CreatedBy = "",
                            OperationId = wkfOp.OperationId,
                            CreatedOn = DateTime.Now, 
                            
                        };
                        wkList.Add(item);
                    }
                }
                if (!await _repo.UpdateWorkflowOperationAsync(wkList))
                {
                    return new WorkflowOperationRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Process Failed"
                            }
                        }
                    };
                }
                return new WorkflowOperationRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Successfull"
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                return new WorkflowOperationRegRespObj
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
