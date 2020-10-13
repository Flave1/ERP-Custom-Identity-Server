using APIGateway.Data;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{


    public class ExposeAnAuthGridHandler : IRequestHandler<ExposeAnAuthGrid, AuthSettupSingleGridResponder>
    {
        private readonly DataContext _context;
        public ExposeAnAuthGridHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<AuthSettupSingleGridResponder> Handle(ExposeAnAuthGrid request, CancellationToken cancellationToken)
        {
            var response = new AuthSettupSingleGridResponder { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            var res = await _context.ScrewIdentifierGrid.FindAsync(request.AuthSettupId);
            if (res == null)
            {
                response.Status.Message.FriendlyMessage = "Search Record Complete";
                return response;
            }
            response.AuthSettups = new AuthSettup
            {
                ActiveOnMobileApp = res.ActiveOnMobileApp,
                ActiveOnWebApp = res.ActiveOnWebApp,
                AuthSettupId = res.ScrewIdentifierGridId,
                Module = res.Module,
                ShouldAthenticate = res.ShouldAthenticate,
                Media = res.Media,
                ModuleName = Convert.ToString((Modules)res.Module),
                NotifierName = Convert.ToString((Media)res.Media),
                InActiveSessionTimeout = (res.InActiveSessionTimeout).Minutes,
                EnableLoginFailedLockout = res.EnableLoginFailedLockout,
                ActiveDirectory = res.ActiveDirectory,
                NumberOfFailedLoginBeforeLockout = res.NumberOfFailedLoginBeforeLockout,
                PasswordUpdateCycle = res.PasswordUpdateCycle,
                SecuritySettingActiveOnMobileApp = res.SecuritySettingActiveOnMobileApp,
                SecuritySettingsActiveOnWebApp = res.SecuritySettingsActiveOnWebApp,
                UseActiveDirectory = res.UseActiveDirectory, 
                RetryTimeInMinutes = (res.RetryTimeInMinutes).Minutes,
                ShouldRetryAfterLockoutEnabled = res.ShouldRetryAfterLockoutEnabled,
                EnableRetryOnMobileApp = res.EnableRetryOnMobileApp,
                EnableRetryOnWebApp = res.EnableRetryOnWebApp,
                LoadBalanceInHours = res.LoadBalanceInHours,
                EnableLoadBalance = res.EnableLoadBalance
            } ?? new AuthSettup();
            return response;
        }
    }
   
}
