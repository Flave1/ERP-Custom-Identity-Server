using APIGateway.Data;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.Commands.Admin;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Handlers.Admin
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRespObj>
    {
        private readonly IAdminRepository _adminRepo;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public DeleteRoleCommandHandler(IAdminRepository adminRepository, DataContext dataContext, ILoggerService loggerService)
        {
            _adminRepo = adminRepository;
            _dataContext = dataContext;
            _logger = loggerService;
        }
        public async Task<DeleteRespObj> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {

            var response = new DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Deleted succcessfully" } } };

            try
            {
                if (request.Req.Count() > 0)
                {
                    foreach (var itemId in request.Req)
                    {
                        await _adminRepo.DeleteUserRoleAsync(itemId);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                response.Deleted = false;
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = "Error occured!! Unable to delete item";
                response.Status.Message.MessageId = errorCode;
                response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                return response;
                #endregion
            }

        }
    } 
}
