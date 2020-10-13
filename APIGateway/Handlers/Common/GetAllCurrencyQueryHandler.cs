using APIGateway.Contracts.Queries.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{
    public class GetAllCurrencyQueryHandlerHandler : IRequestHandler<GetAllCurrencyQuery , CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetAllCurrencyQueryHandlerHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetAllCurrencyQuery  request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllCurrencyAsync();
            return new CommonLookupRespObj
            {
                CommonLookups = list.Select(x => new CommonLookupsObj()
                {
                    LookupId = x.CurrencyId,
                    LookupName = x.CurrencyName,
                    Code = x.CurrencyCode,
                    Active = x.INUSE
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
