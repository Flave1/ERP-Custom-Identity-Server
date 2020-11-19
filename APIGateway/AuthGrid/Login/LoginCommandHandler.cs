using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.Repository.Inplimentation.Cache;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface; 
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity;
using MediatR; 
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace APIGateway.AuthGrid.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentityRepoService _service; 
        private readonly IMeasureService _measure; 
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IDetectionService _detectionService;
        private readonly DataContext _securityContext;
        private readonly ILoggerService _logger;
        private readonly IResponseCacheService _cacheService;
        public LoginCommandHandler(
            IIdentityRepoService  identityRepoService, 
            UserManager<cor_useraccount> userManager,
            IMeasureService measure,
            DataContext dataContext,
            IDetectionService detectionService,
            ILoggerService loggerService,
            IResponseCacheService responseCacheService)
        {
            _userManager = userManager;
            _cacheService = responseCacheService;
            _service = identityRepoService;
            _measure = measure;
            _securityContext = dataContext;
            _logger = loggerService;
            _detectionService = detectionService; 
        }

        public async Task<AuthResponse> OTPOptionsAsync(string userid)
        {
            try
            {
                var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var multiplefFA = await _securityContext.ScrewIdentifierGrid.Where(a => a.Module == (int)Modules.CENTRAL).ToListAsync();
                var user = await _userManager.FindByIdAsync(userid);
                if (multiplefFA.Count() > 0)
                {
                    if (_detectionService.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
                    {
                        if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL && a.Module == (int)Modules.CENTRAL).ActiveOnWebApp)
                        {
                            await _measure.SendOTPToEmailAsync(user);
                            response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your email";
                            return response;
                        }
                        if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS && a.Module == (int)Modules.CENTRAL).ActiveOnWebApp)
                        {
                            response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your number";
                            return response;
                        }
                    }
                    if (_detectionService.Device.Type.ToString().ToLower() == Device.Mobile.ToString().ToLower())
                    {
                        if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL && a.Module == (int)Modules.CENTRAL).ActiveOnMobileApp)
                        {
                            response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your email";
                            return response;
                        }
                        if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS && a.Module == (int)Modules.CENTRAL).ActiveOnMobileApp)
                        {
                            response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your number";
                            return response;
                        }
                    }
                }
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        private async Task<bool> ReplaceDeviceAsync(string userid, string token)
        {
            if(_detectionService.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
            {
                var currentDeviceDetail =await _securityContext.Tracker.Where(q => q.UserId == userid && q.Token != token).ToListAsync();
                if (currentDeviceDetail.Count() > 0)
                {
                    _securityContext.Tracker.RemoveRange(currentDeviceDetail);
                }
            }
            return await _securityContext.SaveChangesAsync() > 0;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {  
                if(!await _measure.ReturnStatusAsync(request.UserName))
                {
                    response.Status.IsSuccessful = false;
                    return response;
                }
                var lockoutSetting = new List<ScrewIdentifierGrid>();

                var cachedSetting = await _cacheService.GetCacheResponseAsync(CacheKeys.AuthSettings);

                if (!string.IsNullOrEmpty(cachedSetting))
                    lockoutSetting = JsonConvert.DeserializeObject<List<ScrewIdentifierGrid>>(cachedSetting);
                else
                    lockoutSetting = await _securityContext.ScrewIdentifierGrid.ToListAsync();

                await _cacheService.CatcheResponseAsync(CacheKeys.AuthSettings, lockoutSetting, TimeSpan.FromSeconds(86400));

                if (!await IsPasswordCharactersValid(request.Password))
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "Invalid Password";
                    return response;
                }
                if (!await UserExist(request))
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "User does not exist";
                    return response;
                }
                if (!await IsValidPassword(request))
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "User/Password Combination is wrong";
                    return response;
                }

                var user = await _userManager.FindByNameAsync(request.UserName);
                 
                var otp = await OTPOptionsAsync(user.Id);
                if (otp.Status.IsSuccessful)
                { 
                    otp.Status.Message.MessageId = user.Email;
                    return otp;
                }

                var result = await _service.LoginAsync(user);

                var measure = Measures.CollectAsMuchAsPossible(user, result, _detectionService); 
                var res = await _measure.GetMeasuresAsync(measure);

                await ReplaceDeviceAsync(user.Id, measure.Token);

                var session = _securityContext.SessionChecker.Find(user.Id);
                if (session != null)
                {
                    session.LastRefreshed = DateTime.UtcNow.Add(lockoutSetting.FirstOrDefault(v => v.Module == (int)Modules.CENTRAL).InActiveSessionTimeout);
                    session.Userid = user.Id;
                    session.Module = (int)Modules.CENTRAL;
                    _securityContext.Entry(session).CurrentValues.SetValues(session);
                    await _securityContext.SaveChangesAsync();
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

        private async Task<bool> IsValidPassword(LoginCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isValidPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPass)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> UserExist(LoginCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> IsPasswordCharactersValid(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }

    }
}
