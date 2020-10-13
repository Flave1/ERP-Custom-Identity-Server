using APIGateway.ActivityRequirement;
using APIGateway.AuthGrid;
using APIGateway.Data;
using FluentValidation;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace APIGateway.Validations.Admin
{
    public class AuthSettupCreateValidator : AbstractValidator<AuthSettupCreate>
    {
        private readonly DataContext _dataContext; 
        public AuthSettupCreateValidator(DataContext data)
        {
            _dataContext = data;

            RuleFor(c => c.Module).NotEmpty();
            RuleFor(c => c.Media).NotEmpty();
            RuleFor(c => c.InActiveSessionTimeout).NotEmpty().WithMessage("InActive Session Timeout Cannot be empty");
            RuleFor(c => c.PasswordUpdateCycle).NotEmpty().WithMessage("Password Update Cycle Cannot be empty"); 
            //RuleFor(c => c).MustAsync(NoDuplicate).WithMessage("Duplicate Settup Detected");
            RuleFor(c => c).MustAsync(CheckForActiveDirectory).WithMessage("Active Directory Expcted");
            RuleFor(c => c).MustAsync(CheckForFailedLoginLockout).WithMessage("Number of Login Expected");
            RuleFor(c => c).MustAsync(CheckForFailedLoginMinutes).WithMessage("Minutes for Login Retry Expected");
            RuleFor(c => c).MustAsync(CheckForLoadBalance).WithMessage("Load Balancer is expecting time function");

            RuleFor(c => c).MustAsync(RetryTimeNotMoreThanSeven).WithMessage("Number of retries cannot be more than six (6)");
        }

        private async Task<bool> RetryTimeNotMoreThanSeven(AuthSettupCreate request, CancellationToken cancellationToken)
        {
            if (request.NumberOfFailedLoginBeforeLockout > 6)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> CheckForLoadBalance(AuthSettupCreate request, CancellationToken cancellationToken)
        {
            if (request.EnableLoadBalance && request.LoadBalanceInHours < 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> CheckForFailedLoginLockout(AuthSettupCreate request, CancellationToken cancellationToken)
        {
            if(request.EnableLoginFailedLockout && request.NumberOfFailedLoginBeforeLockout < 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> CheckForFailedLoginMinutes(AuthSettupCreate request, CancellationToken cancellationToken)
        {
            if (request.EnableLoginFailedLockout && request.RetryTimeInMinutes <= 0)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> CheckForActiveDirectory(AuthSettupCreate request, CancellationToken cancellationToken)
        {
            if(request.UseActiveDirectory && string.IsNullOrEmpty(request.ActiveDirectory))
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        //private async Task<bool> NoDuplicate(AuthSettupCreate request, CancellationToken cancellationToken)
        //{
        //    var auth = await _dataContext.ScrewIdentifierGrid.Where(a => a.Media == request.Media && a.Module == request.Module && request.AuthSettupId != a.ScrewIdentifierGridId).ToListAsync();
        //    if (auth.Count() > 0)
        //    {
        //        return await Task.Run(() => false);
        //    }
        //    return await Task.Run(() => true);
        //} 
    }
    //[ERPLogin]
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly UserManager<cor_useraccount> _userManager; 
        public LoginCommandValidator(UserManager<cor_useraccount> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Password)
                .NotEmpty();
                //.MinimumLength(8)
                //.MaximumLength(16)
            //    .MustAsync(IsPasswordCharactersValid).WithMessage("Invalid Password");
            RuleFor(a => a.UserName).NotEmpty();
            //RuleFor(c => c).MustAsync(UserExist).WithMessage("User does not exist")
            //    .MustAsync(IsValidPassword).WithMessage("User/Password Combination is wrong");
        }
        
        private async Task<bool> IsValidPassword(LoginCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isValidPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPass)
            {
                return await Task.Run(() => false); 
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> UserExist(LoginCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return await Task.Run(() => false); 
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> IsPasswordCharactersValid(string password, CancellationToken cancellationToken)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password)); 
        }
    }

    public class OTPLoginCommandValidator : AbstractValidator<OTPLoginCommand>
    {
        private readonly IMeasureService _measure;
        private readonly UserManager<cor_useraccount> _userManager;
        public OTPLoginCommandValidator(IMeasureService measure, UserManager<cor_useraccount>  userManager)
        {
            _measure = measure;
            _userManager = userManager;

            RuleFor(a => a.OTP).NotEmpty();
            RuleFor(a => a.Email).NotEmpty();
            RuleFor(a => a).MustAsync(OTPExistAsync).WithMessage("OTP Error Occurred!! <br/> UnIdentified OTP");
            RuleFor(a => a).MustAsync(IsValidOtpAsync).WithMessage("OTP Error Occurred!! <br/> OTP Has Expired");
            RuleFor(a => a).MustAsync(UserOwnCurrentOTPAsyync).WithMessage("OTP Error Occurred!! <br/> OTP not associated to this user account");
        }

        private async Task<bool> OTPExistAsync(OTPLoginCommand request, CancellationToken cancellation)
        {
            var otp = await _measure.GetSingleOtpTrackAsync(request.OTP);
            if (otp == null)
            {
                return await Task.Run(() => false); 
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> IsValidOtpAsync(OTPLoginCommand request, CancellationToken cancellation)
        {
            var isNotValidOTP = await _measure.OTPDateExpiredAsync(request.OTP);
            if (isNotValidOTP)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
 
        private async Task<bool> UserOwnCurrentOTPAsyync(OTPLoginCommand request, CancellationToken cancellation)
        { 
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            { 
                return await Task.Run(() => false);
            }

            if (user.Email.Trim().ToLower() != request.Email.Trim().ToLower())
            {
                return await Task.Run(() => false); 
            }
             return await Task.Run(() => true);
        }
    }


}
