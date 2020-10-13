using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.Repository.Inplimentation.Cache;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{
	public class MakeAuthSettup : IRequestHandler<AuthSettupCreate, Responder>
	{
		private readonly DataContext _security;
		private readonly IResponseCacheService _cacheService;
		public MakeAuthSettup(DataContext security, IResponseCacheService cacheService)
		{
			_security = security;
			_cacheService = cacheService;
		}
		public async Task<Responder> Handle(AuthSettupCreate request, CancellationToken cancellationToken)
		{
			var response = new Responder { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
			try
			{ 


				var domain = _security.ScrewIdentifierGrid.FirstOrDefault(t => t.Media == request.Media && request.Module == t.Module);
				var other = _security.ScrewIdentifierGrid.FirstOrDefault(t => t.Media != request.Media && request.Module == t.Module);

				domain.ActiveOnMobileApp = request.ActiveOnMobileApp;
				domain.ActiveOnWebApp = request.ActiveOnWebApp;
				domain.ActiveDirectory = request.ActiveDirectory;
				domain.EnableLoginFailedLockout = request.EnableLoginFailedLockout;
				domain.InActiveSessionTimeout = TimeSpan.FromMinutes(request.InActiveSessionTimeout);
				domain.NumberOfFailedLoginBeforeLockout = request.NumberOfFailedLoginBeforeLockout;
				domain.PasswordUpdateCycle = request.PasswordUpdateCycle;
				domain.SecuritySettingActiveOnMobileApp = request.SecuritySettingActiveOnMobileApp;
				domain.SecuritySettingsActiveOnWebApp = request.SecuritySettingsActiveOnWebApp;
				domain.ShouldAthenticate = request.ShoulAthenticate;
				domain.UseActiveDirectory = request.UseActiveDirectory;
				domain.ShouldRetryAfterLockoutEnabled = request.ShouldRetryAfterLockoutEnabled;
				domain.RetryTimeInMinutes = TimeSpan.FromMinutes(request.RetryTimeInMinutes);
				domain.EnableRetryOnMobileApp = request.EnableRetryOnMobileApp;
				domain.EnableRetryOnWebApp = request.EnableRetryOnWebApp;
				domain.EnableLoadBalance = request.EnableLoadBalance;
				domain.LoadBalanceInHours = request.LoadBalanceInHours;

				_security.Entry(domain).CurrentValues.SetValues(domain);

				other.ActiveDirectory = request.ActiveDirectory;
				other.EnableLoginFailedLockout = request.EnableLoginFailedLockout;
				other.InActiveSessionTimeout = TimeSpan.FromMinutes(request.InActiveSessionTimeout);
				other.NumberOfFailedLoginBeforeLockout = request.NumberOfFailedLoginBeforeLockout;
				other.PasswordUpdateCycle = request.PasswordUpdateCycle;
				other.SecuritySettingActiveOnMobileApp = request.SecuritySettingActiveOnMobileApp;
				other.SecuritySettingsActiveOnWebApp = request.SecuritySettingsActiveOnWebApp; 
				other.UseActiveDirectory = request.UseActiveDirectory;
				other.ShouldRetryAfterLockoutEnabled = request.ShouldRetryAfterLockoutEnabled;
				other.RetryTimeInMinutes = TimeSpan.FromMinutes(request.RetryTimeInMinutes);
				other.EnableRetryOnMobileApp = request.EnableRetryOnMobileApp;
				other.EnableRetryOnWebApp = request.EnableRetryOnWebApp;
				other.EnableLoadBalance = request.EnableLoadBalance;
				other.LoadBalanceInHours = request.LoadBalanceInHours;

				_security.Entry(other).CurrentValues.SetValues(other);

				await _security.SaveChangesAsync();

				var authsetting = await _security.ScrewIdentifierGrid.ToListAsync();
				await _cacheService.ResetCacheAsync(CacheKeys.AuthSettings);
				await _cacheService.CatcheResponseAsync(CacheKeys.AuthSettings, authsetting, TimeSpan.FromSeconds(3600));

				response.ResponderId = domain.ScrewIdentifierGridId;
				response.Status.IsSuccessful = true;
				return response;
			}
			catch (Exception ex)
			{
				return response;
			}
		}
	}

}
