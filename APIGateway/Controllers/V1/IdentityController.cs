using System.Threading.Tasks;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Commands.UserAccount;
using GODPAPIs.Contracts.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GOSLibraries.GOS_Financial_Identity;
using APIGateway.ActivityRequirement;
using APIGateway.AuthGrid;
using GOSLibraries.Enums;
using Wangkanai.Detection.Services;
using Puchase_and_payables.Handlers.Supplier.Settup;
using APIGateway.AuthGrid.Recovery;

namespace Libraryhub.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityRepoService _identityService; 
        private readonly IMediator _mediator;
        private readonly IMeasureService _measure;
        private readonly IDetectionService _detection;

        public IdentityController(
            IIdentityRepoService identityService,  
            IMediator  mediator,
            IMeasureService measure,
            IDetectionService detection)
        {
            _identityService = identityService;  
            _mediator = mediator;
            _measure = measure;
            _detection = detection;
        }
   
        [HttpPost(ApiRoutes.Identity.LOGIN)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        { 
            var response = await _mediator.Send(command);
            var securityResp = await _measure.CheckForFailedTrailsAsync(response.Status.IsSuccessful, (int)Modules.CENTRAL, command.UserName);
            if (securityResp.Status.IsSuccessful)
            {
                if (response.Status.IsSuccessful)
                {
                    return Ok(response);
                }
                await _measure.UnlockUserAsync(command.UserName);
                return BadRequest(response); 
            }
            await _measure.PerformLockFunction(command.UserName, securityResp.UnLockAt, securityResp.IsSecurityQuestion);
            return BadRequest(securityResp);
        }

        [HttpPost(ApiRoutes.Identity.OTP_LOGIN)]
        public async Task<IActionResult> OTP_LOGIN([FromQuery] OTPLoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Identity.LOGIN_OUT)]
        public async Task<IActionResult> LOGIN_OUT()
        {
            var command = new LogoutCommand();
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Identity.ANSWAER)]
        public async Task<IActionResult> ANSWAER([FromBody] AnswerQuestionsCommand command)
        { 
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Identity.RECOVER_PASSWORD_BY_EMAIL)]
        public async Task<IActionResult> RECOVER_PASSWORD_BY_EMAIL([FromBody] RecoverAccountByEmailCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.Identity.NEW_PASS)]
        public async Task<IActionResult> NEW_PASS([FromBody] ChangePasswordCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Identity.UPDATE_PASS)]
        public async Task<IActionResult> UPDATE_PASS([FromBody] ChangeOldPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        

        #region Lookat
        [HttpPost(ApiRoutes.Identity.REFRESHTOKEN)]
        public async Task<IActionResult> Refresh([FromBody] UserRefreshTokenReqObj request)
        {

            var authResponse = await _identityService.RefreshTokenAsync(request.RefreshToken, request.Token);
            //if (!authResponse.Success)
            //{
            //    return BadRequest(new AuthFailedResponse
            //    {
            //        Status = authResponse.Status,
            //    });
            //}

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        //[HttpPost(ApiRoutes.Identity.CHANGE_PASSWORD)]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request)
        //{

        //    if (request.Email.Length < 1)
        //    {
        //        return BadRequest(new AuthFailedResponse
        //        {
        //            //Errors = new[] { "Email Required" }
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Email Required"
        //                }
        //            }
        //        });
        //    }

        //    if (request.OldPassword.Length < 1)
        //    {
        //        return BadRequest(new AuthFailedResponse
        //        {
        //            //Errors = new[] { "Old Password Required" }
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Old Password Required"
        //                }
        //            }
        //        });
        //    }

        //    if (request.NewPassword.Length < 1)
        //    {
        //        return BadRequest(new AuthFailedResponse
        //        {
        //            //Errors = new[] { "New Password Required" },
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "New Password Required"
        //                }
        //            }

        //        });
        //    }


        //    var authResponse = await _identityService.ChangePasswsord(request);
        //    if (!authResponse.Success)
        //    {
        //        return BadRequest(new AuthFailedResponse
        //        {
        //            //Errors = authResponse.Errors
        //            Status = authResponse.Status
        //        });
        //    }
        //    return Ok(new AuthSuccessResponse
        //    {
        //        Token = authResponse.Token,
        //        RefreshToken = authResponse.RefreshToken
        //    });
        //}

        //[HttpPost(ApiRoutes.Identity.CONFIRM_EMAIL)]
        //public async Task<IActionResult> ConfirmEmail([FromBody] ConfirnmationRequest request)
        //{

        //    try
        //    {
        //        if (request.Email.Length < 1)
        //        {
        //            return BadRequest(new ConfirnmationResponse
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Email Required change password"
        //                    }

        //                }
        //            });
        //        }


        //        var userExist = await _identityService.CheckUserAsync(request.Email);
        //        if (!userExist)
        //        {

        //            return BadRequest(new ConfirnmationResponse
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Email not found"
        //                    }

        //                }
        //            });
        //        }
        //        var response = await _identityService.ConfirmEmailAsync(request);
        //        if (!response.Status.IsSuccessful)
        //        {
        //            return BadRequest(new ConfirnmationResponse
        //            {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = false,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = response.Status.Message.FriendlyMessage
        //                    }

        //                }
        //            });
        //        }
        //        return Ok(new ConfirnmationResponse
        //        {

        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = true
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _loggerService.Error(ex.Message ?? ex.InnerException.Message);
        //        return BadRequest(ex.Message ?? ex.InnerException.Message);
        //    }

        //}

        //[HttpPost(ApiRoutes.Identity.CONFIRM_CODE)]
        //public async Task<IActionResult> ConfirmationCode([FromBody] ConfirnmationRequest request)
        //{

        //    if (request.Code.Length < 4)
        //    {
        //        return BadRequest(new ConfirnmationResponse
        //        {
        //            Status = new APIResponseStatus{ IsSuccessful = false,  Message = new APIResponseMessage { FriendlyMessage = "Invalid Verification Code" }}
        //        });
        //    }

        //    var userExist = await _identityService.VerifyAsync(request.Email);
        //    if (!userExist.Status.IsSuccessful)
        //    {

        //        return BadRequest(
        //            new ConfirnmationResponse
        //            { 
        //                Status = new APIResponseStatus{ IsSuccessful = false, Message = new APIResponseMessage{ FriendlyMessage = userExist.Status.Message.FriendlyMessage
        //             }
        //            }
        //        });
        //    }
        //    return Ok(new ConfirnmationResponse{ Status = new APIResponseStatus
        //        {
        //            IsSuccessful = true
        //        }
        //    });
        //}

        #endregion

        public string token { get; set; }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Identity.FETCH_USERDETAILS)]
        public async Task<ActionResult<UserDataResponseObj>> GetUserProfile()
        {
            string userId = HttpContext.User?.FindFirst(c => c.Type == "userId").Value;

            var profile = await _identityService.FetchLoggedInUserDetailsAsync(userId);
            
            if (!profile.Status.IsSuccessful)
            {
                return BadRequest(profile.Status);
            }
            return Ok(profile);
        } 
    } 
}