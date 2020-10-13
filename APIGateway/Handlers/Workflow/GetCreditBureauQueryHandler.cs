using APIGateway.Contracts.Queries.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{ 
    public class GetCreditBureauQueryHandler : IRequestHandler<GetCreditBureauQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetCreditBureauQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetCreditBureauQuery request, CancellationToken cancellationToken)
        {
            var lookUp = await _repo.GetCreditBureauAsync(request.CreditBureauId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.CreditBureauId, 
                    LookupName = lookUp.CreditBureauName,
                    CorporateChargeAmount = lookUp.CorporateChargeAmount,
                    IndividualChargeAmount = lookUp.IndividualChargeAmount,
                    GLAccountId = lookUp.GLAccountId,
                    IsMandatory = lookUp.IsMandatory
                };
                respItemList.Add(item);
            }

            return new CommonLookupRespObj
            {
                CommonLookups = respItemList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = lookUp == null ? null : "Search Complete! No Record found"
                    }
                }
            };
        }
    }
}
