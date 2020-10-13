using GOSLibraries.GOS_API_Response;
using MediatR;
using System.Collections.Generic;

namespace APIGateway.AuthGrid
{
    public class AuthSettupCreate : IRequest<Responder>
    {
        public int AuthSettupId { get; set; }
        public bool ShoulAthenticate { get; set; }
        public int Media { get; set; }
        public int Module { get; set; }
        public bool ActiveOnMobileApp { get; set; }
        public bool ActiveOnWebApp { get; set; }
        public bool UseActiveDirectory { get; set; }
        public string ActiveDirectory { get; set; }
        public bool EnableLoginFailedLockout { get; set; }
        public int NumberOfFailedLoginBeforeLockout { get; set; }
        public int InActiveSessionTimeout { get; set; }
        public int PasswordUpdateCycle { get; set; }
        public bool ShouldRetryAfterLockoutEnabled { get; set; }
        public bool SecuritySettingActiveOnMobileApp { get; set; }
        public bool SecuritySettingsActiveOnWebApp { get; set; }
        public int RetryTimeInMinutes { get; set; }
        public bool EnableRetryOnMobileApp { get; set; }
        public bool EnableRetryOnWebApp { get; set; }
        public bool EnableLoadBalance { get; set; }
        public int LoadBalanceInHours { get; set; }
    }
    public class ExposeAnAuthGrid : IRequest<AuthSettupSingleGridResponder>
    {
        public ExposeAnAuthGrid() { }
        public int AuthSettupId { get; set; }
        public ExposeAnAuthGrid(int authSettupId)
        {
            AuthSettupId = authSettupId;
        }
    }
    public class ExposeAuthGrid : IRequest<AuthSettupGridResponder> { }
    public class AuthSettupGridResponder
    {
        public List<AuthSettup> AuthSettups { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class AuthSettupSingleGridResponder
    {
        public AuthSettup AuthSettups { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AuthSettup : IRequest<Responder>
    {
        public int AuthSettupId { get; set; }
        public bool ShouldAthenticate{ get; set; }
        public int Media { get; set; }
        public int Module { get; set; }
        public bool ActiveOnMobileApp { get; set; }
        public bool ActiveOnWebApp { get; set; }
        public string ModuleName { get; set; }
        public string NotifierName { get; set; }
        public bool UseActiveDirectory { get; set; }
        public string ActiveDirectory { get; set; }
        public bool EnableLoginFailedLockout { get; set; }
        public int NumberOfFailedLoginBeforeLockout { get; set; }
        public bool ShouldRetryAfterLockoutEnabled { get; set; }
        public bool EnableRetryOnMobileApp { get; set; }
        public bool EnableRetryOnWebApp { get; set; }
        public int RetryTimeInMinutes { get; set; }
        public int InActiveSessionTimeout { get; set; }
        public int PasswordUpdateCycle { get; set; }
        public bool SecuritySettingActiveOnMobileApp { get; set; }
        public bool SecuritySettingsActiveOnWebApp { get; set; }
        public bool EnableLoadBalance { get; set; }
        public int LoadBalanceInHours { get; set; }
    }

    public class SessionTrail : IRequest<SessionCheckerRespObj>
    {
        public string UserId { get; set; }
        public int Module { get; set; }
    }
    public class LoginFailed : IRequest<LogingFailedRespObj>
    {
        public string UserId { get; set; }
        public bool IsSuccessful { get; set; }
        public int Module { get; set; }
    }

}
