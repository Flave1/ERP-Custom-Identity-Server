using APIGateway.Contracts.Commands.Common;
using APIGateway.Repository.Interface.Common;

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

    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, DeleteRespObj>
    {
        private readonly ICommonRepository _repo;
        public DeleteCityCommandHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<DeleteRespObj> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ItemsId.Count() > 0)
                {
                    foreach (var itemId in request.ItemsId)
                    {
                        await _repo.DeleteCityAsync(itemId);
                    }
                }
                return new DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Item(s) deleted succcessfully", } } };
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
