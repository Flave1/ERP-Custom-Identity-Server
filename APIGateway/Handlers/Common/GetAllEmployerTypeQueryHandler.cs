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

    public class GetAllEmployerTypeQueryHandler : IRequestHandler<GetAllEmployerTypeQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetAllEmployerTypeQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetAllEmployerTypeQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllEmployerTypeAsync();
            return new CommonLookupRespObj
            {
                CommonLookups = list.Select(x => new CommonLookupsObj()
                {
                    LookupId = x.EmployerTypeId,
                    LookupName = x.Type,
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
