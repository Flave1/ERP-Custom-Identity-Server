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

    public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetCurrencyQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetCurrencyQuery request, CancellationToken cancellationToken)
        {
            var lookUp = await _repo.GetCurrencyAsync(request.CurrencyId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.CurrencyId,
                    Code = lookUp.CurrencyCode,
                    LookupName = lookUp.CurrencyName,
                    BaseCurrency = (bool)lookUp.BaseCurrency,
                    Active = lookUp.INUSE,
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
