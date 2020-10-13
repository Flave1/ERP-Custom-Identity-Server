using APIGateway.Contracts.Response.Modules;
using APIGateway.Data;
using APIGateway.DomainObjects.Modules;
using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{ 
    public class DeleteSolutionModuleCommand : IRequest<SolutionModuleRegRespObj>
    {
        public int SolutionModuleId { get; set; } 
        public class DeleteSolutionModuleCommandHandler : IRequestHandler<DeleteSolutionModuleCommand, SolutionModuleRegRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            public DeleteSolutionModuleCommandHandler(
                ILoggerService loggerService, 
                DataContext data)
            {
                _logger = loggerService; 
                _data = data;
            }
            public async Task<SolutionModuleRegRespObj> Handle(DeleteSolutionModuleCommand request, CancellationToken cancellationToken)
            {
                var response = new SolutionModuleRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                {   
                    if (request.SolutionModuleId > 0)
                    {
                        var selectedSol = await _data.SolutionModule.FindAsync(request.SolutionModuleId);
                        if (selectedSol != null)
                        {
                            _data.SolutionModule.Remove(selectedSol);
                        }
                        await _data.SaveChangesAsync();
                    }   
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = $"Successfully ";
                    return response; 
                }
                catch (Exception ex)
                {
                    #region Log error to file 
                    var errorCode = ErrorID.Generate(4);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                      
                    response.Status.Message.FriendlyMessage = "Error occured!! Unable to delete item";
                    response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                    return response;
                    #endregion
                }
            }
        }
    }
    
}
