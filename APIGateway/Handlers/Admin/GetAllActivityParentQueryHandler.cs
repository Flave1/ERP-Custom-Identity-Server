using APIGateway.Contracts.Queries.Admin;
using APIGateway.Contracts.Response.Admin;
using GODP.APIsContinuation.Repository.Interface.Admin; 
using GODPAPIs.Contracts.Response.Admin;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Admin
{
    public class GetAllActivityParentQueryHandler : IRequestHandler<GetAllActivityParentQuery, ActivityParentRespObj>
    {
        private readonly IAdminRepository _adminRepo;
        public GetAllActivityParentQueryHandler(IAdminRepository adminRepository)
        {
            _adminRepo = adminRepository;
        }
        public async Task<ActivityParentRespObj> Handle(GetAllActivityParentQuery request, CancellationToken cancellationToken)
        {
            var list = await _adminRepo.GetAllActivityParentsAsync();
            return new ActivityParentRespObj
            {
                ActivityParents = list.Select(x => new ActivityParentObj
                {
                    Active = x.Active,
                    ActivityParentId = x.ActivityParentId,
                    ActivityParentName = x.ActivityParentName,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Deleted = x.Deleted,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0? null: "Search Complete! No record found"
                    }
                }
            };
        }
    }
}
