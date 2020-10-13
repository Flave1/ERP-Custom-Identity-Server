using GODP.APIsContinuation.DomainObjects.Account;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.Commands.UserAccount;
using GODPAPIs.Contracts.RequestObjects.Account;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Repository.Interface
{
    public interface IIdentityRepoService
    {
        //Task<AuthenticationResult> RegisterAsync(UserRegistrationReqObj userRegistration);
        //Task<AuthenticationResult> ChangePasswsord(ChangePassword pass);
        Task<AuthenticationResult> LoginAsync(cor_useraccount user);
        Task<AuthenticationResult> RefreshTokenAsync(string userId, string token);
        //Task<bool> CheckUserAsync(string email);
        Task<UserDataResponseObj> FetchLoggedInUserDetailsAsync(string userId);
        //Task<ConfirnmationResponse> ConfirmEmailAsync(ConfirnmationRequest request);
        //Task<ConfirnmationResponse> VerifyAsync(string code); 
    }
}
