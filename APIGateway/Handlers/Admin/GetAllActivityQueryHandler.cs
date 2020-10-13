using APIGateway.Contracts.Queries.Admin;
using APIGateway.Contracts.Response.Admin;
using APIGateway.Data; 
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Admin
{  
    public class GetAllActivityQueryHandler : IRequestHandler<GetAllActivityQuery, ActivityRespObj>
    {
        private readonly DataContext _dataContext;
        public GetAllActivityQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ActivityRespObj> Handle(GetAllActivityQuery request, CancellationToken cancellationToken)
        {
            var list = await _dataContext.cor_activity.ToListAsync();

            return new ActivityRespObj
            {
                Activities = list.Select(x => new ActivityObj
                {
                    Active = x.Active,
                    ActivityId = x.ActivityId,
                    ActivityName = x.ActivityName,
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
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No record found"
                    }
                }
            };
        }
    }
}
