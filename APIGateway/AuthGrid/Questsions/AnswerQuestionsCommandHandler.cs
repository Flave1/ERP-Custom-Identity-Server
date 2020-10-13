using APIGateway.Contracts.Response.Admin;
using APIGateway.Contracts.Response.Modules;
using APIGateway.Data;
using APIGateway.DomainObjects.Modules;
using APIGateway.DomainObjects.Questions;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{ 
    public class AnswerQuestionsCommand : IRequest<QuestionsRegRespObj>
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public string UserName { get; set; }
        public class AnswerQuestionsCommandHandler : IRequestHandler<AnswerQuestionsCommand, QuestionsRegRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            private readonly UserManager<cor_useraccount> _userManager;
            public AnswerQuestionsCommandHandler(
                ILoggerService loggerService, 
                DataContext data,
                UserManager<cor_useraccount> userManager)
            {
                _logger = loggerService; 
                _data = data;
                _userManager = userManager;
            }
            public async Task<QuestionsRegRespObj> Handle(AnswerQuestionsCommand request, CancellationToken cancellationToken)
            {
                var response = new QuestionsRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                {
                    var user = await _userManager.FindByNameAsync(request.UserName);
                    if(user != null)
                    {
                        if (!string.IsNullOrEmpty(user.SecurityAnswer))
                        {
                            if (user.SecurityAnswer.Trim().ToLower() == request.Answer.Trim().ToLower())
                            {
                                user.IsQuestionTime = false;
                                user.EnableAt = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
                                await _userManager.UpdateAsync(user);
                            }
                        }
                        else
                        {
                            response.Status.IsSuccessful = false;
                            response.Status.Message.FriendlyMessage = $"No Security questions found";
                            return response;
                        }
                    }
                    else
                    {
                        response.Status.IsSuccessful = false;
                        response.Status.Message.FriendlyMessage = $"Invalid user Name";
                        return response;
                    }
                     
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = $"Successfully";
                    return response; 
                }
                catch (Exception ex)
                {
                    #region Log error to file 
                    var errorCode = ErrorID.Generate(4);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                      
                    response.Status.Message.FriendlyMessage = "Error occured!! Unable to process request";
                    response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                    return response;
                    #endregion
                }
            }
        }
    }
    
}
