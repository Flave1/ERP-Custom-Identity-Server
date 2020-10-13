using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GODP.APIsContinuation.Repository.Interface;
using GODP.APIsContinuation.DomainObjects.Account;  
using GODPAPIs.Contracts.RequestObjects.Account;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_API_Response;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.Commands.UserAccount;
using Microsoft.EntityFrameworkCore;
using GODPCloud.Helpers.Extensions;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Staff;
using APIGateway.Data;
using GOSLibraries.Options;
using APIGateway.DomainObjects.UserAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MimeKit;
using GOSLibraries;
using GOSLibraries.GOS_Financial_Identity;
using Support.SDK;
using APIGateway;
using APIGateway.Contracts.V1;

namespace GODP.APIsContinuation.Repository.Inplimentation
{
    public class IdentityRepoService : IIdentityRepoService
    {
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env; 
        public IdentityRepoService(UserManager<cor_useraccount> userManager, JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            DataContext dataContext, RoleManager<cor_userrole> roleManager, 
            ILoggerService loggerService, IHttpContextAccessor  httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
            _roleManager = roleManager;
            _logger = loggerService; 
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthenticationResult> LoginAsync(cor_useraccount user)
        { 
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken, string token)
        {
            var response = new AuthenticationResult { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            try
            {
                var validatedToken = GetClaimsPrincipalFromToken(token);
                if (validatedToken == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Token";
                    return response;
                }
                 

                var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                     .AddDays(expiryDateUnix);

                if (expiryDateTimeUtc > DateTime.UtcNow)
                {
                    response.Status.Message.FriendlyMessage = "This Token Hasn't Expired Yet";
                    return response;
                } 

                var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value; 
                var storedRefreshToken = _dataContext.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

                if (storedRefreshToken == null)
                {
                    response.Status.Message.FriendlyMessage = "This Token Hasn't Expired Yet";
                    return response;
                }
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    response.Status.Message.FriendlyMessage = "This Refresh Token has Expire";
                    return response;
                } 

                if (storedRefreshToken.Invalidated)
                {
                    response.Status.Message.FriendlyMessage = "This Refresh Token has been Invalidated";
                    return response;
                } 

                if (storedRefreshToken.Used)
                {
                    response.Status.Message.FriendlyMessage = "This Refresh Token has been Used";
                    return response;
                } 

                if (storedRefreshToken.JwtId != jti)
                {
                    response.Status.Message.FriendlyMessage = "This Refresh Token has been Used";
                    return response;
                }  
                storedRefreshToken.Used = true;
                _dataContext.RefreshTokens.Update(storedRefreshToken);
                await _dataContext.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(validatedToken.Claims.SingleOrDefault(x => x.Type == "userId").Value);

                return await GenerateAuthenticationResultForUserAsync(user);
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : RefreshTokenAsync{errorCode} Ex: {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : RefreshTokenAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            } 
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;
                else
                    return principal;
            }
            catch(Exception ex)
            {
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex: {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken &&
                            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                            StringComparison.InvariantCultureIgnoreCase);
        }

        //public async Task<AuthenticationResult> RegisterAsync(UserRegistrationReqObj userRegistration)
        //{
        //    try
        //    {
        //        var existingUser = await _userManager.FindByEmailAsync(userRegistration.Email);

        //        if (existingUser != null)
        //        {
        //            return new AuthenticationResult
        //            {
        //                //Errors = new[] { "" },
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "User with this email address already exist"
        //                    }
        //                }
        //            };
        //        }

        //        var user = new cor_useraccount
        //        {
        //            Email = userRegistration.Email,
        //            UserName = userRegistration.Email,
        //        };

        //        var createdUser = await _userManager.CreateAsync(user, "Password@1");


        //        if (!createdUser.Succeeded)
        //        {
        //            return new AuthenticationResult
        //            { 
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = createdUser.Errors.Select(x => x.Description).FirstOrDefault(),
        //                    }
        //                }
        //            };
        //        }

        //        return await GenerateAuthenticationResultForUserAsync(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Log error 
        //        var errorCode = ErrorID.Generate(4);
        //        _logger.Error($"ErrorID : RegisterAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return new AuthenticationResult
        //        {

        //            Status = new APIResponseStatus
        //            {
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Error occured!! Please tyr again later",
        //                    MessageId = errorCode,
        //                    TechnicalMessage = $"ErrorID : RegisterAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
        //                }
        //            }
        //        };
        //        #endregion
        //    }
        //}

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(cor_useraccount user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id),
                new Claim("staffId", user.StaffId.ToString()),
            };

                var userClaims = await _userManager.GetClaimsAsync(user);

                claims.AddRange(userClaims);

                var userRoles = await _userManager.GetRolesAsync(user);
                    
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole) ?? null);

                    var role = await _roleManager.FindByNameAsync(userRole);

                    if (role == null)
                    {
                        continue;
                    }
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach (var roleClaim in roleClaims)
                    {
                        if (claims.Contains(roleClaim)) continue;
                        claims.Add(roleClaim);
                    }
                }
                var tokenDecriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeSpan),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };


                var token = tokenHandler.CreateToken(tokenDecriptor);

                var refreshToken = new RefreshToken
                {
                    JwtId = token.Id,
                    UserId = user.Id,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddSeconds(6),
                };

                try
                {
                    await _dataContext.RefreshTokens.AddAsync(refreshToken);
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new AuthenticationResult
                    { 
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Something went wrong: {ex.InnerException.Message}",
                            }
                        }
                    };
                }

                return new AuthenticationResult
                { 
                    Token = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken.Token,
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }

        //public async Task<AuthenticationResult> ChangePasswsord(ChangePassword pass)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(pass.Email);

        //        var userPassword = await _userManager.CheckPasswordAsync(user, pass.OldPassword);

        //        if (!userPassword)
        //        {
        //            return new AuthenticationResult
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "This password is not associated to this account",
        //                    }
        //                }
        //            };
        //        }

        //        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //        var changepassword = await _userManager.ResetPasswordAsync(user, token, pass.NewPassword);

        //        if (!changepassword.Succeeded)
        //        {
        //            return new AuthenticationResult
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = changepassword.Errors.Select(x => x.Description).FirstOrDefault(),
        //                    }
        //                }
        //            };
        //        }

        //        return await GenerateAuthenticationResultForUserAsync(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Log error 
        //        var errorCode = ErrorID.Generate(4);
        //        _logger.Error($"ErrorID : ChangePasswsord{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return new AuthenticationResult
        //        {

        //            Status = new APIResponseStatus
        //            {
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Error occured!! Please tyr again later",
        //                    MessageId = errorCode,
        //                    TechnicalMessage = $"ErrorID : ChangePasswsord{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
        //                }
        //            }
        //        };
        //        #endregion
        //    }

        //}

        //public async Task<bool> CheckUserAsync(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null) return true;
        //    return false;
        //}
       

        //public async Task<ConfirnmationResponse> ConfirmEmailAsync(ConfirnmationRequest request)
        //{
        //    try
        //    {
        //        var confirmCode = ConfirmationCode.Generate();
        //        var sent = await SendAndStoreConfirmationCode(confirmCode, request.Email);
        //       if(!sent)
        //            return new ConfirnmationResponse
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Unable to send mail!! please contact systems administrator"
        //                    }
        //                }
        //            };

        //        return new ConfirnmationResponse
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = true,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Please Check your email for email for confirnmation"
        //                }
        //            }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Log error 
        //        var errorCode = ErrorID.Generate(4);
        //        _logger.Error($"ErrorID : ConfirmEmailAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return new ConfirnmationResponse
        //        {

        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Error occured!! Please tyr again later",
        //                    MessageId = errorCode,
        //                    TechnicalMessage = $"ErrorID : ConfirmEmailAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
        //                }
        //            }
        //        };
        //        #endregion
        //    }
         
        //}

        //public async Task<ConfirnmationResponse> VerifyAsync(string code)
        //{
        //    try
        //    {
        //        var verificationCodeFrmRepo = await _dataContext.Cor_Useremailconfirmations
        //       .FirstOrDefaultAsync(x => x.ConfirnamationTokenCode.Trim().ToLower() == code.Trim().ToLower());

        //        if (verificationCodeFrmRepo == null)
        //        {
        //            return new ConfirnmationResponse
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Verification Code Unidentified"
        //                    }
        //                }
        //            };
        //        }
        //        if (verificationCodeFrmRepo.ExpiryDate < DateTime.Now)
        //        {
        //            return new ConfirnmationResponse
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Verification Code has Expired"
        //                    }
        //                }
        //            };
        //        }
        //        return new ConfirnmationResponse
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = true,
        //            }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Log error 
        //        var errorCode = ErrorID.Generate(4);
        //        _logger.Error($"ErrorID : VerifyAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return new ConfirnmationResponse
        //        {

        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Error occured!! Please tyr again later",
        //                    MessageId = errorCode,
        //                    TechnicalMessage = $"ErrorID : VerifyAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
        //                }
        //            }
        //        };
        //        #endregion
        //    }

        //}

        public async Task<bool> SendAndStoreConfirmationCode(string code, string email)
        {
            try
            {
                var thisUser = await _userManager.FindByEmailAsync(email);
                //await _emailService.Send(new EmailMessage
                //{
                //    FromAddresses = new List<EmailAddress>
                //        {
                //            new EmailAddress{ Address = "Simisola.bello@godp.co.uk", Name = "Simisola Bello"}
                //        },
                //    ToAddresses = new List<EmailAddress>
                //        {
                //            new EmailAddress{ Address = "Simisola.bello@godp.co.uk", Name = thisUser.UserName}
                //        },
                //    Subject = "Account Confirmation",
                //    Content = $"Dear {thisUser.UserName}, <br> Copy and paste this code {code} on the confirmation field to change your password",
                //});

                var userConfirmationCode = new cor_useremailconfirmation
                {
                    ConfirnamationTokenCode = code,
                    ExpiryDate = DateTime.Now.AddHours(1),
                    IssuedDate = DateTime.Now,
                    UserId = thisUser.Id
                };
                 await _dataContext.Cor_Useremailconfirmations.AddAsync(userConfirmationCode);
                var saved = await _dataContext.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {
                var errorId = ErrorID.Generate(4);
                _logger.Error($"SendAndStoreConfirmationCode{errorId}   Error Message{ ex?.Message ?? ex?.InnerException?.Message}");
                return false;
            }


        }

        public async Task<UserDataResponseObj> FetchLoggedInUserDetailsAsync(string userId)
        {
            try
            {

                var currentUser = await _userManager?.FindByIdAsync(userId);
                if (currentUser == null) return new UserDataResponseObj { Status = new APIResponseStatus { IsSuccessful = true } };
                cor_staff currentUserStaffDetails = null;
                cor_companystructure currentUserCompanyStructDetails = null;
                UserDataResponseObj profile = null;
                List<string> activities = new List<string>();

                var additionalActivityIds = _dataContext.cor_userroleadditionalactivity.Where(x => x.UserId == currentUser.Id).Select(x => x.ActivityId);

                var userRoleActivityIds = (from a in _dataContext.UserRoles
                                           join b in _dataContext.cor_userrole on a.RoleId equals b.Id
                                           join c in _dataContext.cor_userroleactivity on b.Id equals c.RoleId
                                           where a.UserId == currentUser.Id
                                           select c.ActivityId).ToList();


                    

                if (userRoleActivityIds.Count() > 0)
                {
                    activities = _dataContext.cor_activity
                        .Where(x => additionalActivityIds
                        .Contains(x.ActivityId) || userRoleActivityIds
                        .Contains(x.ActivityId)).Select(x => x.ActivityName.ToLower()).ToList();
                }
                var userRoles = await _userManager.GetRolesAsync(currentUser);

                if (userRoles.Contains(StaticRoles.GODP))
                {
                    activities = await _dataContext.cor_activity.Where(x => x.Deleted == false).Select(s => s.ActivityName).ToListAsync();
                }

                if (currentUser.StaffId > 0)
                {
                    currentUserStaffDetails = await _dataContext.cor_staff.FirstOrDefaultAsync(z => z.StaffId == currentUser.StaffId);
                    currentUserCompanyStructDetails = await _dataContext.cor_companystructure.SingleOrDefaultAsync(m => m.CompanyStructureId == currentUserStaffDetails.StaffOfficeId);
                    if (currentUserCompanyStructDetails != null)
                    {
                        return profile = new UserDataResponseObj
                        {
                            BranchId = currentUserCompanyStructDetails.CompanyStructureId,
                            StaffId = currentUserStaffDetails.StaffId,
                            BranchName = currentUserCompanyStructDetails.Name,
                            CompanyId = currentUserCompanyStructDetails.CompanyStructureId,
                            CompanyName = currentUserCompanyStructDetails.Name,
                            CountryId = currentUserStaffDetails.CountryId,
                            CustomerName = currentUserStaffDetails.FirstName + " " + currentUserStaffDetails.LastName,
                            StaffName = currentUserStaffDetails.FirstName + " " + currentUserStaffDetails.LastName,
                            LastLoginDate = DateTime.Now,
                            UserId = currentUser.Id,
                            UserName = currentUser.UserName,
                            Roles = await _userManager.GetRolesAsync(currentUser),
                            Activities = activities.Count() < 1 ? null : activities,
                            Email = currentUser.Email,
                            DepartmentId = currentUserStaffDetails.StaffOfficeId ?? 0,
                            Status = new APIResponseStatus { IsSuccessful = true }
                        };
                    }
                }
                profile = new UserDataResponseObj
                {
                    LastLoginDate = DateTime.Now,
                    UserId = currentUser.Id,
                    UserName = currentUser.UserName,
                    StaffId = currentUser.StaffId,
                    Activities = activities.Count() < 1 ? null : activities,
                    Roles = await _userManager.GetRolesAsync(currentUser),
                    Email = currentUser.Email,
                    Status = new APIResponseStatus { IsSuccessful = true }
                };


                if (profile == null)
                {
                    return new UserDataResponseObj
                    {

                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Unable to fetch user details"
                            }
                        }
                    };
                }
                return profile;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : FetchLoggedInUserDetailsAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new UserDataResponseObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : FetchLoggedInUserDetailsAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
