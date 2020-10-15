using APIGateway.Contracts.Queries.Admin;
using APIGateway.Contracts.Response.Admin;
using APIGateway.Data;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.Response.Admin;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Admin
{ 
	public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, UserRoleRespObj>
	{
		private readonly ILoggerService _logger;
		private readonly IAdminRepository _rep;
		private DataContext _dataContext;
		private readonly IHttpContextAccessor _accessor;
		public GetUserRolesQueryHandler(
			DataContext dataContext, 
			ILoggerService loggerService, 
			IAdminRepository adminRepository,
			IHttpContextAccessor httpContextAccessor)
		{
			_dataContext = dataContext;
			_logger = loggerService;
			_rep = adminRepository;
			_accessor = httpContextAccessor;
		}
		public async Task<UserRoleRespObj> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
		{
			var response = new UserRoleRespObj {  UserRoles = new List<UserRoleObj>(), UserRoleActivities = new List<ActivityObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
			try
			{
				var userid = _accessor.HttpContext.User?.FindFirst(r => r.Type == "userId")?.Value ?? string.Empty;
				var userRoleList = await _dataContext.UserRoles.Where(r => r.UserId == userid).ToListAsync();
				response.UserRoles = userRoleList.Select(x => new UserRoleObj()
				{
					RoleName = _dataContext.Roles.FirstOrDefault(r => r.Id == x.RoleId)?.Name,
					RoleId = x.RoleId,
					UserId = x.UserId
				}).ToList() ?? new List<UserRoleObj>();

				if(response.UserRoles.Count() > 0)
				{
					response.UserRoleActivities = (from a in _dataContext.cor_userrole
												   join b in _dataContext.cor_userroleactivity on a.Id equals b.RoleId
												   join q in _dataContext.cor_activity on b.ActivityId equals q.ActivityId
												   where response.UserRoles.Select(r => r.RoleId).ToList().Contains(b.RoleId)
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
												   }).ToList() ?? new List<ActivityObj>();
					 
				}
				return response;
			}
			catch (Exception ex)
			{
				#region Log error to file 
				var errorCode = ErrorID.Generate(4);
				_logger.Error($"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
				response.Status.Message.FriendlyMessage = "Error occured!! Please try again later";
				response.Status.Message.MessageId = errorCode;
				response.Status.Message.TechnicalMessage = $"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
				return response;
				#endregion
			}
		}
	}
}
