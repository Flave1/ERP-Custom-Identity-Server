using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.Repository.Inplimentation.Cache;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Financial_Identity;
using MediatR; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid.OTPLogin
{
    public class OTPLoginCommandHandler : IRequestHandler<OTPLoginCommand, AuthResponse>
    {
        private readonly IIdentityRepoService _service;
        private readonly IMeasureService _measure; 
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly DataContext _dataContext;
        private readonly IResponseCacheService _cacheService;
        public OTPLoginCommandHandler(
            IIdentityRepoService identityRepoService,
            UserManager<cor_useraccount> userManager,
            IMeasureService measure,
            IResponseCacheService cacheService,
            DataContext dataContext)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _service = identityRepoService;
            _measure = measure;
            _cacheService = cacheService;
        }
        public async Task<AuthResponse> Handle(OTPLoginCommand request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
        
            try
            {
                var lockoutSetting = new List<ScrewIdentifierGrid>();

                var cachedSetting = await _cacheService.GetCacheResponseAsync(CacheKeys.AuthSettings);

                if (!string.IsNullOrEmpty(cachedSetting))
                    lockoutSetting = JsonConvert.DeserializeObject<List<ScrewIdentifierGrid>>(cachedSetting);
                else
                    lockoutSetting = await _dataContext.ScrewIdentifierGrid.ToListAsync();

                await _cacheService.CatcheResponseAsync(CacheKeys.AuthSettings, lockoutSetting, TimeSpan.FromSeconds(3600));

                var user = await _userManager.FindByEmailAsync(request.Email); 
                await _measure.RemoveOtpAsync(request.OTP);
                var result = await _service.LoginAsync(user);   

                var measure = Measures.CollectAsMuchAsPossible(user, result, request);
                await _measure.GetMeasuresAsync(measure);
               
                 
                var session = _dataContext.SessionChecker.Find(user.Id);
                if (session != null)
                {
                    session.LastRefreshed = DateTime.UtcNow.Add(lockoutSetting.FirstOrDefault(v => v.Module == (int)Modules.CENTRAL).InActiveSessionTimeout);
                    session.Userid = user.Id;
                    session.Module = (int)Modules.CENTRAL;
                    _dataContext.Entry(session).CurrentValues.SetValues(session);
                    await _dataContext.SaveChangesAsync();
                }

                response.Token = result.Token;
                response.RefreshToken = result.RefreshToken;
                return response;
            }
            catch (Exception ex)
            {
                response.Status.Message.FriendlyMessage = ex?.Message ?? ex?.InnerException?.Message;
                response.Status.Message.TechnicalMessage = ex.ToString();
                return response;
            }
        }
    }
}
