using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using APIGateway.Repository.Inplimentation.Cache;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.URI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using Support.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Wangkanai.Detection.Services;

namespace APIGateway.AuthGrid
{
    public interface IMeasureService
    {
        Task<bool> ReturnStatusAsync(string userid);
        Task<bool> GetMeasuresAsync(Tracker tracker);
        Task<Tracker> ReturnTrackAsync(int id);
        Task<IEnumerable<Tracker>> ReturnTracksAsync();
        Task<bool> SendOTPToEmailAsync(cor_useraccount user);
        Task<bool> RemoveOtpAsync(string otp);
        Task<bool> OTPDateExpiredAsync(string otp);
        Task<OTPTracker> GetSingleOtpTrackAsync(string otp);
        Task<bool> Is2FAForSMSEnabledOnMobileAsync(IDetectionService detector);
        Task<bool> Is2FAForEmailEnabledOnMobileAsync(IDetectionService detector);
        Task<bool> Is2FAForEmailEnabledOnDesktopAsync(IDetectionService detector);
        Task<bool> Is2FAForSMSEnabledOnDesktopAsync(IDetectionService detector);
        Task<Tracker> GetMeasuresByUserIdAsync(string userid);
        Task<bool> RemoveTrackedAsync(Tracker tracker);
        Task<LogingFailedRespObj> CheckForFailedTrailsAsync(bool isSuccessful, int module, string userid);
        Task<SessionCheckerRespObj> CheckForSessionTrailAsync(string userid, int module);
        Task<bool> PerformLockFunction(string userid, DateTime unlockat, bool isQuestionTime);
        Task<bool> UnlockUserAsync(string userid);
    }

    public class MeasureService : IMeasureService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _mailservice;
        private readonly DataContext _security;
        private readonly IBaseURIs _uRIs;
        private readonly IDetectionService _detectionService;
        private readonly IResponseCacheService _cacheService;
        private readonly ILoggerService _logger;
        private readonly UserManager<cor_useraccount> _userManager;
        public MeasureService(
            IHttpContextAccessor accessor,
            DataContext dataContext,
            IEmailService service,
            IWebHostEnvironment webHostEnvironment, 
            IBaseURIs uRIs,
            IDetectionService detectionService,
            IResponseCacheService cacheService,
            ILoggerService logger,
            UserManager<cor_useraccount> userManager)
        {
            _accessor = accessor;
            _mailservice = service; 
            _security = dataContext;
            _env = webHostEnvironment;
            _logger = logger;
            _uRIs = uRIs;
            _cacheService = cacheService;
            _userManager = userManager;
            _detectionService = detectionService;
        }
        public async Task<bool> GetMeasuresAsync(Tracker tracker)
        { 
            if(tracker.MeasureId > 0)
            {
                var tracked = await _security.Tracker.FindAsync(tracker.MeasureId);
                if (tracked != null)
                {
                    _security.Entry(tracker).CurrentValues.SetValues(tracker);
                } 
            }
            else
                await _security.Tracker.AddAsync(tracker);
            await SendLoginEmailAsync(tracker);
            return await _security.SaveChangesAsync() > 0;
        }

        public async Task<bool> PerformLockFunction(string user, DateTime unlockat, bool isQuestionTime)
        {
            var useraccount = await _userManager.FindByNameAsync(user) ?? null;
            if(useraccount == null)
            {
                return true;
            }
            useraccount.IsQuestionTime = isQuestionTime ? true : false;
            useraccount.EnableAt = unlockat;
            await _userManager.UpdateAsync(useraccount);
            return true; 
        }
        public async Task<bool> UnlockUserAsync(string userid)
        {
            var lockedAccount = await _userManager.FindByNameAsync(userid) ?? null;
            if (lockedAccount != null)
            {
                lockedAccount.IsQuestionTime = false;
                lockedAccount.EnableAt = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
                await _userManager.UpdateAsync(lockedAccount);
            }
            return true;
        }
        public async Task<IEnumerable<Tracker>> ReturnTracksAsync()
        {
            return await _security.Tracker.ToListAsync();
        }
        public async Task<Tracker> ReturnTrackAsync(int id)
        {
            return await _security.Tracker.FindAsync(id);
        }

        private async Task<bool> SendLoginEmailAsync(Tracker person)
        {
            try
            {
                var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
                          + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
                          + Path.DirectorySeparatorChar.ToString() + "login.html";

                var builder = new BodyBuilder();
                var uri = $"{_uRIs.MainClient}#/auth/login";
                using (StreamReader SourceReader = File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }
                var time = DateTime.Now;
                string messageBody = string.Format(builder.HtmlBody, time, uri);
                var emailMessage = new EmailMessage();
                emailMessage.Subject = "Account Login";
                emailMessage.Content = messageBody; 
                emailMessage.SendIt = true;
                emailMessage.FromAddresses = new List<EmailAddress>();
                emailMessage.ToAddresses.Add(new EmailAddress { Address = person.Email});
                await _mailservice.Send(emailMessage);
                return await Task.Run(() => true);
            }
            catch (Exception)
            {

                return true;
            }
            
        }

        private async Task<OTPTracker> ProduceOtpAsync(cor_useraccount user)
        {
            Random rnd = new Random();
            var otp = (rnd.Next(100000, 999999)).ToString();

            var newOtp = new OTPTracker
            {
                DateIssued = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMinutes(2),
                OTP = otp.Insert(2, "-").Insert(5, "-"),
                UserId = user.Id,
                Email = user.Email,
            };
            _logger.Information("I am in ProduceOtpAsync ");
            await _security.OTPTracker.AddAsync(newOtp);
            return newOtp;
        }
        public async Task<bool> SendOTPToEmailAsync(cor_useraccount user)
        {
            _logger.Information("I am in SendOTPToEmailAsync ");
            var newOtp = await ProduceOtpAsync(user);
            await SendOTPEmailAsync(newOtp);
           return await _security.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveOtpAsync(string otp)
        {
            var otpitem = await _security.OTPTracker.FirstOrDefaultAsync(q => q.OTP.Trim().ToLower() == otp.Trim().ToLower());
            if(otpitem != null)
            {
                _security.OTPTracker.Remove(otpitem);
                return await _security.SaveChangesAsync() > 0;
            }
            return await Task.Run(() => false);
        }

        private async Task<bool> SendOTPEmailAsync(OTPTracker person)
        {
            try
            {
                _logger.Information("I am in SendOTPEmailAsync ");
                var pathToFile = _env.WebRootPath + Path.DirectorySeparatorChar.ToString()
                          + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate"
                          + Path.DirectorySeparatorChar.ToString() + "otp.html";

                var builder = new BodyBuilder();
                var callbackuri = $"{_uRIs.MainClient}identity/otp/login?OTP={person.OTP}&Email={person.Email}";
                 
                var encodedUri = HttpUtility.UrlPathEncode(callbackuri.ToString());
                using (StreamReader SourceReader = File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }
                string messageBody = string.Format(builder.HtmlBody, person.OTP, encodedUri);
                var emailMessage = new EmailMessage();
                emailMessage.Subject = "OTP Verification";
                emailMessage.Content = messageBody.Replace('{', ' ').Replace('}', ' ');
                emailMessage.SendIt = true;
                emailMessage.FromAddresses = new List<EmailAddress>();
                emailMessage.ToAddresses.Add(new EmailAddress { Address = person.Email });
                _logger.Information("About to enter mail sender method");
                await _mailservice.Send(emailMessage);
                return await Task.Run(() => true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return true;
            }

        }

        public async Task<bool> OTPDateExpiredAsync(string otp)
        {
            var otpitem = await _security.OTPTracker.FirstOrDefaultAsync(q => q.OTP.Trim().ToLower() == otp.Trim().ToLower());
            if (otpitem != null)
            {
                if(DateTime.Now > otpitem.ExpiryDate)
                {
                    await this.RemoveOtpAsync(otpitem.OTP);
                    return await Task.Run(() => true);
                }
            }
            return await Task.Run(() => false);
        }

        public async Task<OTPTracker> GetSingleOtpTrackAsync(string otp)
        {
            return await _security.OTPTracker.FirstOrDefaultAsync(q => q.OTP.Trim().ToLower() == otp.Trim().ToLower())??null;
        }
         
        public async Task<bool> Is2FAForSMSEnabledOnDesktopAsync(IDetectionService detector)
        {
            var multiplefFA = await _security.ScrewIdentifierGrid.Where(a => a.Module == (int)Modules.CENTRAL && a.Media == (int)Media.SMS).ToListAsync();

            if (multiplefFA.Count() > 0)
            { 
                if (detector.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
                {
                    return multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS && a.Module == (int)Modules.CENTRAL).ActiveOnWebApp;
                } 
            } 
            return await Task.Run(() => false);
        }

        public async Task<bool> Is2FAForEmailEnabledOnDesktopAsync(IDetectionService detector)
        {
            var multiplefFA = await _security.ScrewIdentifierGrid.Where(a => a.Module == (int)Modules.CENTRAL && a.Media == (int)Media.EMAIL).ToListAsync();
            if (multiplefFA.Count() > 0)
            {
                if (detector.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
                {
                    return multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL && a.Module == (int)Modules.CENTRAL).ActiveOnWebApp;
                }
            }
            return await Task.Run(() => false);
        }

        public async Task<bool> Is2FAForEmailEnabledOnMobileAsync(IDetectionService detector)
        {
            var multiplefFA = await _security.ScrewIdentifierGrid.Where(a => a.Module == (int)Modules.CENTRAL && a.Media == (int)Media.EMAIL).ToListAsync();
            if (multiplefFA.Count() > 0)
            {
                if (detector.Device.Type.ToString().ToLower() == Device.Mobile.ToString().ToLower())
                {
                    return multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL && a.Module == (int)Modules.CENTRAL).ActiveOnMobileApp;
                }
            }
            return await Task.Run(() => false);
        }
        public async Task<bool> Is2FAForSMSEnabledOnMobileAsync(IDetectionService detector)
        {
            var multiplefFA = await _security.ScrewIdentifierGrid.Where(a => a.Module == (int)Modules.CENTRAL && a.Media == (int)Media.SMS).ToListAsync();
            if (multiplefFA.Count() > 0)
            {
                if (detector.Device.Type.ToString().ToLower() == Device.Mobile.ToString().ToLower())
                {
                    return multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS && a.Module == (int)Modules.CENTRAL).ActiveOnMobileApp;
                }
            }
            return await Task.Run(() => false);
        }

        public async Task<Tracker> GetMeasuresByUserIdAsync(string userid)
        {
            return await _security.Tracker.FirstOrDefaultAsync(q => q.UserId == userid)?? null;
        }

        public async Task<bool> RemoveTrackedAsync(Tracker tracker)
        {
            _security.Tracker.Remove(tracker);
            return await _security.SaveChangesAsync() > 0;
        }

        public Task<bool> SendOTPEmailAsync(cor_useraccount user)
        {
            throw new NotImplementedException();
        }

        public async Task<LogingFailedRespObj> CheckForFailedTrailsAsync(bool isSuccessful, int module, string userid)
        {
            var usergent = string.Empty;

            if (string.IsNullOrEmpty(userid)) 
                usergent = _detectionService.UserAgent.ToLower().Trim(); 
            else
                usergent = userid;

            var response = new LogingFailedRespObj { IsSecurityQuestion = false, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            var lockoutSetting = new List<ScrewIdentifierGrid>();
             
            var cachedSetting = await _cacheService.GetCacheResponseAsync(CacheKeys.AuthSettings);

            if (!string.IsNullOrEmpty(cachedSetting))
                lockoutSetting = JsonConvert.DeserializeObject<List<ScrewIdentifierGrid>>(cachedSetting);
            else
                lockoutSetting = await _security.ScrewIdentifierGrid.ToListAsync();

            if(lockoutSetting.Count() > 0 )await _cacheService.CatcheResponseAsync(CacheKeys.AuthSettings, lockoutSetting, TimeSpan.FromSeconds(3600)); 

            if (_detectionService.Device.Type.ToString() == Device.Desktop.ToString() && lockoutSetting.Any(e => e.EnableLoginFailedLockout && e.EnableRetryOnWebApp && e.Module == module) )
            {
                var setup = lockoutSetting.FirstOrDefault(e => e.EnableLoginFailedLockout && e.EnableRetryOnWebApp && e.Module == module);

                var loginFailed = _security.LogingFailedChecker.FirstOrDefault(r => r.Userid == usergent);

                if (isSuccessful)
                { 
                    if (loginFailed != null)
                    {
                        _security.LogingFailedChecker.Remove(loginFailed);
                        await _security.SaveChangesAsync();
                        return response;
                    }
                }
                if (!isSuccessful)
                {
                    if (loginFailed == null)
                    {
                        loginFailed = new LogingFailedChecker();
                        loginFailed.Counter = 1;
                        loginFailed.RetryTime = DateTime.UtcNow.Add(setup.RetryTimeInMinutes);
                        loginFailed.Userid = usergent;
                        _security.LogingFailedChecker.Add(loginFailed);
                        await _security.SaveChangesAsync();
                        return response;
                    }
                    else
                    {
                        if (loginFailed.Counter >= setup.NumberOfFailedAttemptsBeforeSecurityQuestions && !setup.ShouldRetryAfterLockoutEnabled)
                        {
                            response.Status.Message.FriendlyMessage = "Please proceed to answer security question";
                            response.IsSecurityQuestion = true;
                            response.Status.IsSuccessful = false;
                            return response;
                        }
                        loginFailed.Counter = loginFailed.Counter + 1;
                        _security.Entry(loginFailed).CurrentValues.SetValues(loginFailed);
                        await _security.SaveChangesAsync();
                        if (loginFailed.Counter >= setup.NumberOfFailedLoginBeforeLockout)
                        {
                            var timeRemain = (loginFailed.RetryTime - DateTime.UtcNow).Minutes + 1;
                            if (DateTime.UtcNow < loginFailed.RetryTime)
                            {
                                string TimeVariable = string.Empty;
                                TimeVariable = $"{timeRemain} minutes";
                                if (timeRemain == 1)
                                {
                                    TimeVariable = $"{(loginFailed.RetryTime - DateTime.UtcNow).Seconds} seconds";
                                }
                                response.Status.IsSuccessful = false;
                                response.UnLockAt = loginFailed.RetryTime;
                                response.Status.Message.FriendlyMessage = $"Please retry after  {TimeVariable}";
                                return response;
                            }
                            else
                            {
                                _security.LogingFailedChecker.Remove(loginFailed);
                                await _security.SaveChangesAsync();
                                return response;
                            }

                        }
                        loginFailed.RetryTime = DateTime.UtcNow.Add(setup.RetryTimeInMinutes);
                        loginFailed.Userid = usergent;
                        _security.Entry(loginFailed).CurrentValues.SetValues(loginFailed);
                        await _security.SaveChangesAsync();
                        return response;
                    }
                }
            }
            
            
            
            
            if (_detectionService.Device.Type.ToString() == Device.Mobile.ToString()
                    && lockoutSetting.Any(e => e.EnableLoginFailedLockout && e.EnableRetryOnWebApp && e.Module == module))
            {
                var setup = lockoutSetting.FirstOrDefault(e => e.EnableLoginFailedLockout && e.ActiveOnMobileApp && e.Module == module);
                var failedCached = _security.LogingFailedChecker.FirstOrDefault(r => r.Userid == usergent);
                if (isSuccessful)
                { 
                    if (failedCached != null)
                    {
                        _security.LogingFailedChecker.Remove(failedCached);
                        await _security.SaveChangesAsync();
                        return response;
                    }
                }
                if (!isSuccessful)
                {
                    var loginFailed = _security.LogingFailedChecker.Find(usergent);
                    if (loginFailed == null)
                    {
                        loginFailed = new LogingFailedChecker();
                        loginFailed.Counter = 1;
                        loginFailed.RetryTime = DateTime.UtcNow.Add(setup.RetryTimeInMinutes);
                        loginFailed.Userid = usergent;
                        _security.LogingFailedChecker.Add(loginFailed);
                        await _security.SaveChangesAsync();
                        return response;
                    }
                    else
                    {
                        if (loginFailed.Counter >= setup.NumberOfFailedAttemptsBeforeSecurityQuestions && !setup.ShouldRetryAfterLockoutEnabled)
                        {
                            response.Status.Message.FriendlyMessage = "Please proceed to answer security question";
                            response.IsSecurityQuestion = true;
                            response.Status.IsSuccessful = false;
                            return response;
                        }
                        loginFailed.Counter = loginFailed.Counter + 1;
                        _security.LogingFailedChecker.Remove(loginFailed);
                        await _security.SaveChangesAsync();
                        if (loginFailed.Counter >= setup.NumberOfFailedLoginBeforeLockout)
                        {
                            var timeRemain = (loginFailed.RetryTime - DateTime.UtcNow).Minutes + 1;
                            if (DateTime.UtcNow < loginFailed.RetryTime)
                            {
                                string TimeVariable = string.Empty;
                                TimeVariable = $"{timeRemain} minutes";
                                if (timeRemain == 1)
                                {
                                    TimeVariable = $"{(loginFailed.RetryTime - DateTime.UtcNow).Seconds} seconds";
                                }
                                response.Status.IsSuccessful = false;
                                response.Status.Message.FriendlyMessage = $"Please retry after  {TimeVariable}";
                                return response;
                            }
                            else
                            {
                                _security.LogingFailedChecker.Remove(loginFailed);
                                await _security.SaveChangesAsync();
                                return response;
                            }
                        }
                        loginFailed.RetryTime = DateTime.UtcNow.Add(setup.RetryTimeInMinutes);
                        loginFailed.Userid = usergent;
                        _security.Entry(loginFailed).CurrentValues.SetValues(loginFailed);
                        await _security.SaveChangesAsync();
                        return response;
                    }   
                }
            }
            return response;
        }

        public async Task<SessionCheckerRespObj> CheckForSessionTrailAsync(string userid, int module)
        { 
            var response = new SessionCheckerRespObj { StatusCode = 200, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            var lockoutSetting = new List<ScrewIdentifierGrid>();

            var cachedSetting = await _cacheService.GetCacheResponseAsync(CacheKeys.AuthSettings);

            if (!string.IsNullOrEmpty(cachedSetting)) 
                lockoutSetting = JsonConvert.DeserializeObject<List<ScrewIdentifierGrid>>(cachedSetting);
            else
                lockoutSetting = await _security.ScrewIdentifierGrid.ToListAsync();

            await _cacheService.CatcheResponseAsync(CacheKeys.AuthSettings, lockoutSetting, TimeSpan.FromSeconds(3600));

            if (lockoutSetting.Any(e => e.InActiveSessionTimeout != TimeSpan.Parse("00:00:00.0000000") && e.Module == module))
            {
                var setup = lockoutSetting.FirstOrDefault(e => e.Module == module);
                var cached = _security.SessionChecker.Where(r => r.Userid == userid).ToList();

                var loginSession = _security.SessionChecker.FirstOrDefault(r => r.Userid == userid);
                if (loginSession == null)
                {
                    loginSession = new SessionChecker();
                    loginSession.LastRefreshed = DateTime.UtcNow.Add(setup.InActiveSessionTimeout);
                    loginSession.Userid = userid;
                    loginSession.Module = module;
                    _security.SessionChecker.Add(loginSession);
                    await _security.SaveChangesAsync();
                    response.StatusCode = 200;
                    return response;
                }
                else
                { 
                    if (DateTime.UtcNow > loginSession.LastRefreshed)
                    {
                        response.StatusCode = 401;
                        _security.SessionChecker.Remove(loginSession);
                        await _security.SaveChangesAsync();
                        return response;
                    }
                    else
                    {
                        loginSession.LastRefreshed = DateTime.UtcNow.Add(setup.InActiveSessionTimeout); 
                        loginSession.Module = module;
                        _security.Entry(loginSession).CurrentValues.SetValues(loginSession);
                        await _security.SaveChangesAsync(); 
                        return response;
                    } 
                }
            } 
            return response;
        }

        public async Task<bool> ReturnStatusAsync(string userid)
        {
             
            var lockedAccount = await  _userManager.FindByNameAsync(userid) ?? null;
            if (lockedAccount != null)
            {
                if (lockedAccount.IsQuestionTime)
                    return false;
                if (lockedAccount.EnableAt > DateTime.UtcNow) 
                    return false; 
            }
            return await Task.Run(() => true);
        }
 
    }
}
