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

    public class GetEmailConfigQuery : IRequest<EmailConfigRespObj>
    {
        public GetEmailConfigQuery() { }
        public int EmailConfigId { get; set; }
        public GetEmailConfigQuery(int emailConfigId)
        {
            EmailConfigId = emailConfigId;
        }
        public class GetEmailConfigQueryHandler : IRequestHandler<GetEmailConfigQuery, EmailConfigRespObj>
        {
            private readonly IAdminRepository _repo;
            public GetEmailConfigQueryHandler(IAdminRepository adminRepository)
            {
                _repo = adminRepository;
            }
            public async Task<EmailConfigRespObj> Handle(GetEmailConfigQuery request, CancellationToken cancellationToken)
            {
                var x = await _repo.GetEmailConfigAsync(request.EmailConfigId);
                var respItemList = new List<EmailConfigObj>();
                if (x != null)
                {
                    var item = new EmailConfigObj
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
                    };
                    respItemList.Add(item);
                }

                return new EmailConfigRespObj
                {
                    EmailConfigs = respItemList,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = x != null ? null : "Search Complete! No Record found"
                        }
                    }
                };
            }
        }
    }
   
} 
