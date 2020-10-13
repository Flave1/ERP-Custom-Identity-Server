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
    public class GetAllCityQueryHandler : IRequestHandler<GetAllCityQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetAllCityQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
        {
            var stateList = await _repo.GetAllStateAsync();
            var list = await _repo.GetAllCityAsync();
            return new CommonLookupRespObj
            {
                CommonLookups = list.Select(x => new CommonLookupsObj()
                {
                    LookupId = x.CityId,
                    LookupName = x?.CityName,
                    Code = x?.CityCode,
                    ParentId = x.StateId,
                    ParentName = stateList.FirstOrDefault(c => c.StateId == x.StateId)?.StateName

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
