using APIGateway.Contracts.Response.Email;
using APIGateway.Data;
using APIGateway.MailHandler.Service;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{ 
    public class GetCurrentUserMailsQuery: IRequest<EmailRespObj>
    {
        public int Module { get; set; }
        public class GetCurrentUserMailsQueryHandler : IRequestHandler<GetCurrentUserMailsQuery, EmailRespObj>
        {
            private readonly IEmailService _email;
            private readonly IHttpContextAccessor _accessor;
            private readonly UserManager<cor_useraccount> _userManager;
            private readonly DataContext _dataContext;
            public GetCurrentUserMailsQueryHandler(
                IEmailService emailService, 
                DataContext dataContext,
                IHttpContextAccessor httpContextAccessor,
                UserManager<cor_useraccount> userManager)
            {
                _email = emailService;
                _dataContext = dataContext;
                _userManager = userManager;
                _accessor = httpContextAccessor;
            }
            public async Task<EmailRespObj> Handle(GetCurrentUserMailsQuery request, CancellationToken cancellationToken)
            {
                var response = new EmailRespObj { Emails = new System.Collections.Generic.List<EmailMessageObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var userId = _accessor.HttpContext.User?.FindFirst(s => s.Type == "userId")?.Value;
                var byEmail = string.Empty;
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    byEmail = user.Email;
                }
                var res = await _dataContext.EmailMessage.Where(d => d.ReceivedBy.Trim().ToLower() == byEmail.Trim().ToLower() || d.ReceiverUserId == userId && d.Module == request.Module).ToListAsync();

                if (res.Count() > 0)
                {
                    response.Emails = res.OrderByDescending(x => x.EmailMessageId).Select(s => new EmailMessageObj()
                    {
                        Content = s.Content,
                        DateSent = s.DateSent,
                        EmailMessageId = s.EmailMessageId,
                        EmailStatus = s.EmailStatus,
                        StatusName = Convert.ToString((EmailStatus)s.EmailStatus),
                        SentBy = s.SentBy,
                        ReceivedBy = s.ReceivedBy,
                        Subject = s.Subject,
                    }).ToList();
                    response.EmailCount = res.Count(r => r.EmailStatus == (int)EmailStatus.Received);
                    return response;
                }
                return response;
            }
        }
    }
    
}
