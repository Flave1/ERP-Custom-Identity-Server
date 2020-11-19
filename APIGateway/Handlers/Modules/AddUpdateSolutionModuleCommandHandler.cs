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
    public class AddUpdateSolutionModuleCommand : IRequest<SolutionModuleRegRespObj>
    {
        public int SolutionModuleId { get; set; }
        public string SolutionName { get; set; }
        public class AddUpdateSolutionModuleCommandHandler : IRequestHandler<AddUpdateSolutionModuleCommand, SolutionModuleRegRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            public AddUpdateSolutionModuleCommandHandler(
                ILoggerService loggerService, 
                DataContext data)
            {
                _logger = loggerService; 
                _data = data;
            }
            public async Task<SolutionModuleRegRespObj> Handle(AddUpdateSolutionModuleCommand request, CancellationToken cancellationToken)
            {
                var response = new SolutionModuleRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                {  
                    SolutionModule sol = new SolutionModule();
                    sol.SolutionModuleId = request.SolutionModuleId;
                    sol.SolutionName = request.SolutionName;
                    if (sol.SolutionModuleId > 0)
                    {
                        var selectedSol = await _data.SolutionModule.FindAsync(sol.SolutionModuleId);
                        if (selectedSol != null)
                        {
                            _data.Entry(selectedSol).CurrentValues.SetValues(sol);
                        }
                    }
                    else
                        await _data.SolutionModule.AddAsync(sol);
                    await _data.SaveChangesAsync();
                     
                    var actionTaken = request.SolutionModuleId < 1 ? "created" : "updated";
                    response.SolutionModuleId = sol.SolutionModuleId;
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
