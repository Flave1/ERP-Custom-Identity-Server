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

    public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetCountryQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        { 
            var lookUp = await _repo.GetCountryAsync(request.CountryId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.CountryId,
                    Code = lookUp.CountryCode,
                    LookupName = lookUp.CountryName,
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
