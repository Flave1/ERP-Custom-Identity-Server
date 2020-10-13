using APIGateway.Contracts.Queries.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using System.Collections.Generic; 
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{ 
    public class GetCityQueryHandler : IRequestHandler<GetCityQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetCityQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var lookUp = await _repo.GetCityAsync(request.CityId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.CityId,
                    Code = lookUp.CityCode,
                    LookupName = lookUp.CityName,
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
