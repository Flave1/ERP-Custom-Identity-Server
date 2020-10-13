using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Financial_Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid.Login
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, AuthResponse>
    {
		private readonly IHttpContextAccessor _accessor;
		private readonly IMeasureService _measure;
		public LogoutCommandHandler(IHttpContextAccessor  httpContextAccessor, IMeasureService measure)
		{
			_measure = measure;
			_accessor = httpContextAccessor;
		}
        public async Task<AuthResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
			var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
			try
			{
				var userid = _accessor.HttpContext.User?.FindFirst(a => a.Type == "userId")?.Value;
				if (string.IsNullOrEmpty(userid))
				{ 
					return response;
				}
				var tracked = await _measure.GetMeasuresByUserIdAsync(userid);
				if(tracked == null)
				{
					return response;
				}
				await _measure.RemoveTrackedAsync(tracked);
				return response;
			}
			catch (Exception)
			{ 
				throw;
			}
        }
    }
}
