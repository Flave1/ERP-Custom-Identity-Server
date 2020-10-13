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

namespace APIGateway.Handlers.Workflow
{ 
    public class GetJobTitleQueryHandler : IRequestHandler<GetJobTitleQuery, CommonLookupRespObj>
    {
        private readonly ICommonRepository _repo;
        public GetJobTitleQueryHandler(ICommonRepository commonRepository)
        {
            _repo = commonRepository;
        }
        public async Task<CommonLookupRespObj> Handle(GetJobTitleQuery request, CancellationToken cancellationToken)
        {
            var lookUp = await _repo.GetJobTitlAsync(request.JobTitleId);
            var respItemList = new List<CommonLookupsObj>();
            if (lookUp != null)
            {
                var item = new CommonLookupsObj
                {
                    LookupId = lookUp.JobTitleId,
                    LookupName = lookUp.Name,
                    Description = lookUp.JobDescription,
                    Skills = lookUp.Skills,
                    SkillDescription = lookUp.SkillDescription
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
