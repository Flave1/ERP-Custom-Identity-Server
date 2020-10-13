using APIGateway.Contracts.Commands.Common;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{
    public class DeleteEmailConfigCommand  : IRequest<DeleteRespObj>
    {
        public List<int> TargetIds { get; set; }
        public class DeleteEmailConfigCommandHandler : IRequestHandler<DeleteEmailConfigCommand, DeleteRespObj>
        {
            private readonly IAdminRepository _repo;
            public DeleteEmailConfigCommandHandler(IAdminRepository adminRepository)
            {
                _repo = adminRepository;
            }
            public async Task<DeleteRespObj> Handle(DeleteEmailConfigCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.TargetIds.Count() > 0)
                    {
                        foreach (var itemId in request.TargetIds)
                        {
                            await _repo.DeleteEmailConfigAsync(itemId);
                        }
                    }
                    return new DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Deleted succcessfully", } } };
                }
                catch (Exception ex)
                {
                    #region Error
                    var errorCode = ErrorID.Generate(4);
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
   
}
