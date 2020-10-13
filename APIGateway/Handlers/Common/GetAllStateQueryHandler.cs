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

    public class GetAllStateQueryHandler : IRequestHandler<GetAllStateQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetAllStateQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetAllStateQuery request, CancellationToken cancellationToken)
        {
            var countryList = await _repo.GetAllCountryAsync();
            var list = await _repo.GetAllStateAsync();
            return new CommonLookupRespObj
            {
                CommonLookups = list.Select(x => new CommonLookupsObj()
                {
                    LookupId = x.StateId,
                    LookupName = x?.StateName,
                    ParentId = x?.CountryId??0,
                    Code = x?.StateCode,
                    ParentName = countryList.FirstOrDefault(s => s.CountryId == x.CountryId)?.CountryName
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
