using APIGateway.Contracts.Queries.Admin; 

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.Admin;
using MediatR;
using System; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APIGateway.Data;   

namespace GODP.APIsContinuation.Handlers.Admin
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, RoleRespObj>
    {
		private readonly ILoggerService _logger;
		private readonly IAdminRepository _rep;
		private DataContext _dataContext;
		public GetAllRolesQueryHandler(DataContext dataContext, ILoggerService loggerService, IAdminRepository adminRepository)
		{
			_dataContext = dataContext;
			_logger = loggerService;
			_rep = adminRepository;
		}
        public async Task<RoleRespObj> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
			try
			{
				var userRoleList = _dataContext.UserRoles.ToList();
				var listOfRoles = await _rep.GetAllRolesAsync();
				return new RoleRespObj
				{
					Roles = listOfRoles.Select(x => new RoleObj()
					{
						Active = x.Active,
						RoleId = x.Id,
						RoleName = x.Name, 
						Deleted = x.Deleted,
						CreatedBy = x.CreatedBy,
						CreatedOn = x.CreatedOn,
						UpdatedBy = x.UpdatedBy,
						UpdatedOn = x.UpdatedOn,
						UserCount = userRoleList.Count(c =>c.RoleId == x.Id)
					}).ToList(),
					Status = new APIResponseStatus
					{
						IsSuccessful = true,
					}
				};
			}
			catch (Exception ex)
			{
				#region Log error to file 
				var errorCode = ErrorID.Generate(4);
				_logger.Error($"ErrorID : GetAllRolesQueryHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
				return new RoleRespObj
				{

					Status = new APIResponseStatus
					{
						IsSuccessful = false,
						Message = new APIResponseMessage
						{
							FriendlyMessage = "Error occured!! Please tyr again later",
							MessageId = errorCode,
							TechnicalMessage = $"ErrorID : GetAllRolesQueryHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
						}
					}
				};
				#endregion
			}
		}
    }
}
