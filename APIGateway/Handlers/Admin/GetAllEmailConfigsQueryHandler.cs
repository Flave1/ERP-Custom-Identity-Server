using APIGateway.Contracts.Queries.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Contracts.Response.Email;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{
    public class GetAllEmailConfigQuery : IRequest<EmailConfigRespObj>
    {
        public class GetAllEmailConfigQueryHandler : IRequestHandler<GetAllEmailConfigQuery, EmailConfigRespObj>
        {
            private readonly IAdminRepository _repo;
            public GetAllEmailConfigQueryHandler(IAdminRepository adminRepository)
            {
                _repo = adminRepository;
            }
            public async Task<EmailConfigRespObj> Handle(GetAllEmailConfigQuery request, CancellationToken cancellationToken)
            {
                var list = await _repo.GetAllEmailConfigAsync();
                return new EmailConfigRespObj
                {
                    EmailConfigs = list.Select(x => new EmailConfigObj()
                    {
                        SMTPPort = x.SMTPPort,
                        BaseFrontend = x.BaseFrontend,
                        EmailConfigId = x.EmailConfigId,
                        EnableSSL = x.EnableSSL,
                        MailCaption = x.MailCaption,
                        SenderEmail = x.SenderEmail,
                        SenderPassword = x.SenderPassword,
                        SenderUserName = x.SenderUserName,
                        SendNotification = x.SendNotification,
                        SmtpClient = x.SmtpClient
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No Record Found"
                        }
                    }
                };
            }
        }
    }
   
}
