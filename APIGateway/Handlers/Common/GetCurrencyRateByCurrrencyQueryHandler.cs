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

namespace APIGateway.Handlers.Common
{ 

    public class GetCurrencyRateByCurrrencyQueryHandler : IRequestHandler<GetCurrencyRateByCurrrencyQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetCurrencyRateByCurrrencyQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetCurrencyRateByCurrrencyQuery request, CancellationToken cancellationToken)
        {
            var lookUp = await _repo.GetCurrencyRatesByCurrencyAsync(request.CurrencyId);
            var currency = await _repo.GetAllCurrencyAsync();
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.CurrencyRateId,
                    Date = lookUp.Date,
                    ParentName = currency.FirstOrDefault(s => s.CurrencyId == lookUp.CurrencyId)?.CurrencyName,
                    ParentId = lookUp.CurrencyId,
                    Code = lookUp.CurrencyCode,
                    BuyingRate = (double)lookUp.BuyingRate,
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
                        FriendlyMessage = lookUp != null ? null : "Search Complete! No Record found"
                    }
                }
            };
        }
    }
}
