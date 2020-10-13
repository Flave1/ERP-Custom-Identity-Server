using APIGateway.Data;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Commands.Company;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Handlers.Ccompany
{
    public class DeleteCompanyStructureDefinitionCommandHandler : IRequestHandler<DeleteCompanyStructureDefinitionCommand, DeleteRespObj>
    {

        private readonly ICompanyRepository _repo;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public DeleteCompanyStructureDefinitionCommandHandler(ICompanyRepository repository, DataContext dataContext, ILoggerService loggerService)
        {
            _repo = repository;
            _dataContext = dataContext;
            _logger = loggerService;
        }
        public async Task<DeleteRespObj> Handle(DeleteCompanyStructureDefinitionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {


                        if (request.StructureDefinitionIds.Count() > 0)
                        {
                            foreach (var itemId in request.StructureDefinitionIds)
                            {
                                await _repo.DeleteCompanyStructureDefinitionAsync(itemId);
                            }
                        }
                        await _transaction.CommitAsync();
                        return new DeleteRespObj
                        {

                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Item(s) deleted succcessfully",
                                }
                            }
                        };

                    }
                    catch (SqlException ex)
                    {
                        await _transaction.RollbackAsync();
                        #region Log error to file 

                        var errorCode = ErrorID.Generate(4);
                        _logger.Error($"ErrorID : DeleteCompanyStructureDefinitionCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                        return new DeleteRespObj
                        {

                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Error occured!! Unable to delete item",
                                    MessageId = errorCode,
                                    TechnicalMessage = $"ErrorID : DeleteCompanyStructureDefinitionCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                                }
                            }
                        };
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : DeleteCompanyStructureDefinitionCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DeleteRespObj
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : DeleteCompanyStructureDefinitionCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion

            }
        }

    }
}
