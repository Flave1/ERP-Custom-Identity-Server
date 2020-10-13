using APIGateway.Contracts.Response.Admin; 
using APIGateway.Data;  
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
    public class GetQuestionQueryQuery : IRequest<QuestionsRespObj>
    { 
        public GetQuestionQueryQuery() { }
        public int QuestionsId { get; set; }
        public GetQuestionQueryQuery(int questionsId)
        {
            QuestionsId = questionsId;
        }

        public class GetQuestionQueryQueryHandler : IRequestHandler<GetQuestionQueryQuery, QuestionsRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            public GetQuestionQueryQueryHandler(
                ILoggerService loggerService, 
                DataContext data)
            {
                _logger = loggerService; 
                _data = data;
            }
            public async Task<QuestionsRespObj> Handle(GetQuestionQueryQuery request, CancellationToken cancellationToken)
            {
                var response = new QuestionsRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                try
                {
                    var sol = await _data.Questions.FindAsync(request.QuestionsId);
                    var solList = new List<QuestionsObj>();
                    if (sol != null)
                    {
                        var Questionss = new QuestionsObj()
                        {
                            QuestionId = sol.QuestionId,
                            Qiestion = sol.Question
                        };
                        solList.Add(Questionss);
                    } 
                    response.Status.Message.FriendlyMessage = solList.Count()>0 ? string.Empty : "Successfully";
                    response.Questions = solList;
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
