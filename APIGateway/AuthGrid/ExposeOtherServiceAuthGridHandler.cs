using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.Repository.Inplimentation.Cache;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{
    public class ExposeOtherServiceAuthGrid : IRequest<AuthSettupGridResponder>
    {
        public class ExposeOtherServiceAuthGridHandler : IRequestHandler<ExposeOtherServiceAuthGrid, AuthSettupGridResponder>
        {
            private readonly DataContext _context;
            private readonly IResponseCacheService _cacheService;
            public ExposeOtherServiceAuthGridHandler(DataContext context, IResponseCacheService cacheService)
            {
                _context = context;
                _cacheService = cacheService;
            }
            public async Task<AuthSettupGridResponder> Handle(ExposeOtherServiceAuthGrid request, CancellationToken cancellationToken)
            {
                var response = new AuthSettupGridResponder { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var lockoutSetting = new List<ScrewIdentifierGrid>();
                var cachedSetting = await _cacheService.GetCacheResponseAsync(CacheKeys.AuthSettings);
                if (!string.IsNullOrEmpty(cachedSetting))
                {
                    lockoutSetting = JsonConvert.DeserializeObject<List<ScrewIdentifierGrid>>(cachedSetting);
                }
                else
                {
                    lockoutSetting = await _context.ScrewIdentifierGrid.ToListAsync();
                }
                response.AuthSettups = lockoutSetting.Select(q => new AuthSettup
                {
                    ActiveOnMobileApp = q.ActiveOnMobileApp,
                    ActiveOnWebApp = q.ActiveOnWebApp,
                    AuthSettupId = q.ScrewIdentifierGridId,
                    Module = q.Module,
                    Media = q.Media,
                    ShouldAthenticate = q.ShouldAthenticate,
                    ModuleName = Convert.ToString((Modules)q.Module),
                    NotifierName = Convert.ToString((Media)q.Media),
                    UseActiveDirectory = q.UseActiveDirectory,
                    SecuritySettingsActiveOnWebApp = q.SecuritySettingsActiveOnWebApp,
                    SecuritySettingActiveOnMobileApp = q.SecuritySettingActiveOnMobileApp,
                    PasswordUpdateCycle = q.PasswordUpdateCycle,
                    NumberOfFailedLoginBeforeLockout = q.NumberOfFailedLoginBeforeLockout,
                    ActiveDirectory = q.ActiveDirectory,
                    EnableLoginFailedLockout = q.EnableLoginFailedLockout,
                    InActiveSessionTimeout = (q.InActiveSessionTimeout).Minutes,
                    ShouldRetryAfterLockoutEnabled = q.ShouldRetryAfterLockoutEnabled,
                    RetryTimeInMinutes = (q.RetryTimeInMinutes).Minutes,
                    EnableRetryOnWebApp = q.EnableRetryOnWebApp,
                    EnableRetryOnMobileApp = q.EnableRetryOnMobileApp,
                    EnableLoadBalance = q.EnableLoadBalance,
                    LoadBalanceInHours = q.LoadBalanceInHours
                }).ToList() ?? new List<AuthSettup>();
                response.Status.Message.FriendlyMessage = lockoutSetting.Count() > 0 ? string.Empty : "Search Record Complete";
                return response;
            }
        }
    }
   
}
