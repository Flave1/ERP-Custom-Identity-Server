using APIGateway.Contracts.Response.Email;
using APIGateway.MailHandler.Service;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.MailHandler.Send_Recieve_Handler
{

    public class GetSingleEmailDetailsComandQuery : IRequest<EmailRespObj>
    {
        public int EmailId { get; set; }
        public GetSingleEmailDetailsComandQuery() { }
        public GetSingleEmailDetailsComandQuery(int emailId)
        {
            EmailId = emailId;
        }
        public class GetSingleEmailDetailsComandQueryHandler : IRequestHandler<GetSingleEmailDetailsComandQuery, EmailRespObj>
        {
            private readonly IEmailService _email; 
            
            public GetSingleEmailDetailsComandQueryHandler(
                IEmailService emailService)
            {
                _email = emailService; 
            }
            public async Task<EmailRespObj> Handle(GetSingleEmailDetailsComandQuery request, CancellationToken cancellationToken)
            { 
                var response = new EmailRespObj { Emails = new List<EmailMessageObj>(), Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                { 
                    var res = await _email.GetSingleEmailAsync(request.EmailId);
                    
                    if (res != null)
                    {
                        if(res.EmailStatus == (int)EmailStatus.Received)
                        {
                            res.EmailStatus = (int)EmailStatus.Read;
                            await _email.SaveUpdateEmailAsync(res);
                        }
                        
                        var email = new EmailMessageObj
                        {
                            Content = res.Content,
                            DateSent = res.DateSent,
                            EmailMessageId = res.EmailMessageId,
                            EmailStatus = res.EmailStatus,
                            StatusName = Convert.ToString((EmailStatus)res.EmailStatus),
                            SentBy = res.SentBy,
                            ReceivedBy = res.ReceivedBy,
                            Subject = res.Subject, 
                        }??new EmailMessageObj(); 
                        response.Status.IsSuccessful = true;
                        response.Emails.Add(email);
                        return response;
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

}
