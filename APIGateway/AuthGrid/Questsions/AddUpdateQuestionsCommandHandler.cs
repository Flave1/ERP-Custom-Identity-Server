using APIGateway.Contracts.Response.Admin;
using APIGateway.Contracts.Response.Modules;
using APIGateway.Data;
using APIGateway.DomainObjects.Modules;
using APIGateway.DomainObjects.Questions;
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
    public class AddUpdateQuestionsCommand : IRequest<QuestionsRegRespObj>
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public class AddUpdateQuestionsCommandHandler : IRequestHandler<AddUpdateQuestionsCommand, QuestionsRegRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            public AddUpdateQuestionsCommandHandler(
                ILoggerService loggerService, 
                DataContext data)
            {
                _logger = loggerService; 
                _data = data;
            }
            public async Task<QuestionsRegRespObj> Handle(AddUpdateQuestionsCommand request, CancellationToken cancellationToken)
            {
                var response = new QuestionsRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                {  
                    Questions sol = new Questions();
                    sol.QuestionId = request.QuestionId;
                    sol.Question = request.Question;
                    if (sol.QuestionId > 0)
                    {
                        var selectedSol = await _data.Questions.FindAsync(sol.QuestionId);
                        if (selectedSol != null)
                        {
                            _data.Entry(selectedSol).CurrentValues.SetValues(sol);
                        }
                    }
                    else
                        await _data.Questions.AddAsync(sol);
                    await _data.SaveChangesAsync();


                    var actionTaken = request.QuestionId < 1 ? "created" : "updated";
                    response.QuestionId = sol.QuestionId;
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
