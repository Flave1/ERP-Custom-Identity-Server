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
    public class GetAllCurrencyRateQueryHandler : IRequestHandler<GetAllCurrencyRateQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetAllCurrencyRateQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetAllCurrencyRateQuery request, CancellationToken cancellationToken)
        {
            var currencyList = await _repo.GetAllCurrencyAsync();
            var list = await _repo.GetAllCurrencyRateAsync();
            return new CommonLookupRespObj
            {
                CommonLookups = list.Select(x => new CommonLookupsObj()
                {
                    LookupId = x.CurrencyRateId,
                    Date = x.Date,
                    ParentId = x.CurrencyId,
                    Code = x.CurrencyCode,
                    BuyingRate = (double)x.BuyingRate,
                    SellingRate = (double)x.SellingRate,
                    ParentName = currencyList.FirstOrDefault(s => s.CurrencyId == x.CurrencyId)?.CurrencyName
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
