using APIGateway.Contracts.Response.Modules;
using APIGateway.Data;
using APIGateway.DomainObjects.Modules;
using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{ 
    public class GetSingleSolutionModuleQueryCommand : IRequest<SolutionModuleRespObj>
    { 
        public GetSingleSolutionModuleQueryCommand() { }
        public int SolutionModuleId { get; set; }
        public GetSingleSolutionModuleQueryCommand(int solutionModuleId)
        {
            SolutionModuleId = solutionModuleId;
        }

        public class GetSingleSolutionModuleQueryCommandHandler : IRequestHandler<GetSingleSolutionModuleQueryCommand, SolutionModuleRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            public GetSingleSolutionModuleQueryCommandHandler(
                ILoggerService loggerService, 
                DataContext data)
            {
                _logger = loggerService; 
                _data = data;
            }
            public async Task<SolutionModuleRespObj> Handle(GetSingleSolutionModuleQueryCommand request, CancellationToken cancellationToken)
            {
                var response = new SolutionModuleRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                try
                {
                    var sol = await _data.SolutionModule.FindAsync(request.SolutionModuleId);
                    var solList = new List<SolutionModuleObj>();
                    if (sol != null)
                    {
                        var SolutionModules = new SolutionModuleObj()
                        {
                            SolutionModuleId = sol.SolutionModuleId,
                            SolutionName = sol.SolutionName
                        };
                        solList.Add(SolutionModules);
                    } 
                    response.Status.Message.FriendlyMessage = solList.Count()>0 ? string.Empty : "Successfully";
                    return response; 
                }
                catch (Exception ex)
                {
                    #region Log error to file 
                    var errorCode = ErrorID.Generate(4);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "Error occured!! Unable to delete item";
                    response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                    return response;
                    #endregion
                }
            }
        }
    }
    
}
