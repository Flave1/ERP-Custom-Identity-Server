using APIGateway.Contracts.Queries.Admin;
using APIGateway.Contracts.Response.Admin;
using APIGateway.Data;
using AutoMapper; 
using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks; 

namespace GODP.APIsContinuation.Handlers.Admin
{
    public class GetActivitiesByRoleIdQueryHandler : IRequestHandler<GetActivitiesByRoleIdQuery, ActivityRespObj>
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILoggerService _logger;
        private readonly DataContext _context;
        public GetActivitiesByRoleIdQueryHandler(IAdminRepository adminRepository, DataContext dataContext, 
            ILoggerService loggerService)
        {
            _adminRepository = adminRepository;
            _logger = loggerService; 
            _context = dataContext;
        }
        public async Task<ActivityRespObj> Handle(GetActivitiesByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var _ParentList = await _adminRepository.GetAllActivityParentsAsync();
            var data = (from p in _context.cor_activityparent
                        join q in _context.cor_activity on p.ActivityParentId equals q.ActivityParentId
                        orderby p.ActivityParentName
                        select new ActivityObj
                        {
                            UserId = "",
                            RoleName = "",
                            ActivityParentId = p.ActivityParentId,
                            ActivityParentName = p.ActivityParentName,
                            ActivityId = q.ActivityId,
                            ActivityName = q.ActivityName,
                            CanAdd = false,
                            CanApprove = false,
                            CanDelete = false,
                            CanEdit = false,
                            CanView = false,
                            RoleId = ""
                        }).ToList();

            if (!string.IsNullOrEmpty(request.RoleId))
            {
                var data2 = (from a in _context.cor_userrole
                             join b in _context.cor_userroleactivity on a.Id equals b.RoleId
                             join q in _context.cor_activity on b.ActivityId equals q.ActivityId
                             where b.RoleId == request.RoleId
                             orderby q.cor_activityparent.ActivityParentName
                             select new ActivityObj
                             {
                                 RoleId = a.Id,
                                 RoleName = a.Name,
                                 ActivityParentId = q.ActivityParentId, 
                                 ActivityId = q.ActivityId,
                                 ActivityName = q.ActivityName,
                                 CanAdd = (bool)b.CanAdd,
                                 CanApprove = (bool)b.CanApprove,
                                 CanDelete = (bool)b.CanDelete,
                                 CanEdit = (bool)b.CanEdit,
                                 CanView = (bool)b.CanView
                             }).ToList();
                if (data2.Any())
                {
                    foreach(var item in data2)
                    {
                        item.ActivityParentName = _ParentList.FirstOrDefault(s => s.ActivityParentId == item.ActivityParentId)?.ActivityParentName;
                    }
                    var data2Ids = data2.Select(a => a.ActivityId).ToList();

                    var result = data2.Concat(data.Where(x => !data2Ids.Contains(x.ActivityId)));

                    return new ActivityRespObj
                    {
                        Activities = result.OrderByDescending(x => x.RoleId).ToList(),
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = result.Count() > 0 ? true : false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = result.Count() > 0 ? null : "Search complete! No record found"
                            }
                        }
                    };
                     
                }
            }
            return new ActivityRespObj
            {
                Activities = data,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true
                }
                
            };
        }
    }
}
