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

    public class GetDocumentTypeQuery : IRequest<CommonLookupRespObj>
    {
        public GetDocumentTypeQuery() { }
        public int DocumentTypeId { get; set; }
        public GetDocumentTypeQuery(int documentTypeId)
        {
            DocumentTypeId = documentTypeId;
        }
        public class GetDocumentTypeQueryHandler : IRequestHandler<GetDocumentTypeQuery, CommonLookupRespObj>
        {
            private readonly ICommonRepository _repo;
            public GetDocumentTypeQueryHandler(ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<CommonLookupRespObj> Handle(GetDocumentTypeQuery request, CancellationToken cancellationToken)
            {
                var lookUp = await _repo.GetDocumenttypeAsync(request.DocumentTypeId);
                var respItemList = new List<CommonLookupsObj>();
                if (lookUp != null)
                {
                    var item = new CommonLookupsObj
                    {
                        LookupId = lookUp.DocumentTypeId, 
                        LookupName = lookUp.Name,
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
