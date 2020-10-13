using APIGateway.Data;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Commands.Company;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.Data.SqlClient;
using System; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Ccompany
{
    public class DeleteCompanyStructureCommandHandler : IRequestHandler<DeleteCompanyStructureCommand, DeleteRespObj>
    {

        private readonly ICompanyRepository _repo;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public DeleteCompanyStructureCommandHandler(ICompanyRepository repository, DataContext dataContext, ILoggerService loggerService)
        {
            _repo = repository;
            _dataContext = dataContext;
            _logger = loggerService;
        }
        public async Task<DeleteRespObj> Handle(DeleteCompanyStructureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (request.CompanyStructureIds.Count() > 0)
                        {
                            foreach (var itemId in request.CompanyStructureIds)
                            {
                                await _repo.DeleteCompanyStructureAsync(itemId);
                            }
                        }
                        await _transaction.CommitAsync();
                        return new DeleteRespObj
                        {
                            Deleted = true,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = true,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Deleted succcessfully",
                                }
                            }
                        }; 
                    }
                    catch (SqlException ex)
                    {
                        await _transaction.RollbackAsync();
                        #region Log error to file 

                        var errorCode = ErrorID.Generate(4);
                        _logger.Error($"ErrorID : DeleteCompanyStructureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                        return new DeleteRespObj
                        {

                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Error occured!! Unable to delete item",
                                    MessageId = errorCode,
                                    TechnicalMessage = $"ErrorID : DeleteCompanyStructureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
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
                _logger.Error($"ErrorID : DeleteCompanyStructureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DeleteRespObj
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : DeleteCompanyStructureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion

            }
        }

    }
}
