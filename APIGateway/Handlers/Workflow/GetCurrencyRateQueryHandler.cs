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

    public class GetCurrencyRateQueryHandler : IRequestHandler<GetCurrencyRateQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetCurrencyRateQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetCurrencyRateQuery request, CancellationToken cancellationToken)
        {
            var lookUp = await _repo.GetCurrencyRateAsync(request.CurrencyRateId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.CurrencyRateId,  
                    BuyingRate = (double)lookUp.BuyingRate,
                    ParentId = lookUp.CurrencyId,
                    Code = lookUp.CurrencyCode,
                    Date = lookUp.Date,
                    SellingRate = (double)lookUp.SellingRate, 
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
