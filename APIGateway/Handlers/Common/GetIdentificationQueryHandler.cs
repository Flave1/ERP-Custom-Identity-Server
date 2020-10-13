using APIGateway.Contracts.Queries.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using System.Collections.Generic; 
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{ 

    public class GetIdentificationQueryHandler : IRequestHandler<GetIdentificationQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetIdentificationQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetIdentificationQuery request, CancellationToken cancellationToken)
        { 
            var lookUp = await _repo.GetIdentificationAsync(request.IdentificationId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.IdentificationId, 
                    LookupName = lookUp.IdentificationName,
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
