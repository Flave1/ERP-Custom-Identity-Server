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
    public class GetStateQuery : IRequest<CommonLookupRespObj>
    {
        public GetStateQuery() { }
        public int StateId { get; set; }
        public GetStateQuery(int stateId) { StateId = stateId; }
        public class GetStateQueryHandler : IRequestHandler<GetStateQuery, CommonLookupRespObj>
        {
            private readonly ICommonRepository _repo;
            public GetStateQueryHandler(ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<CommonLookupRespObj> Handle(GetStateQuery request, CancellationToken cancellationToken)
            {
                var lookUp = await _repo.GetStateAsync(request.StateId);
                var respItemList = new List<CommonLookupsObj>();
                if (lookUp != null)
                {
                    var item = new CommonLookupsObj
                    {
                        LookupId = lookUp.StateId,
                        Code = lookUp.StateCode,
                        LookupName = lookUp.StateName,
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
   
} 
