using APIGateway.AuthGrid.Recovery;
using APIGateway.Data;
using FluentValidation;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Validations.Recovery
{
    public class ChangeOldPasswordCommandVal : AbstractValidator<ChangeOldPasswordCommand>
    {
        private readonly UserManager<cor_useraccount> _userManager;
        public ChangeOldPasswordCommandVal(UserManager<cor_useraccount> userManager)
        {
            _userManager = userManager;

            RuleFor(e => e.NewPassword).MustAsync(GenericValidators.IsPasswordCharactersValid).WithMessage("New password must contain atleast 8 characters, uppercase, lowercase, symbols and alphanumeric eg : 'Password@1'");
            RuleFor(e => e.OldPassword).MustAsync(GenericValidators.IsPasswordCharactersValid).WithMessage("Old password must contain atleast 8 characters, uppercase, lowercase, symbols and alphanumeric eg : 'Password@1'");
            RuleFor(e => e).MustAsync(IsUserPassword).WithMessage("Old Password does not match this user account");
        } 
        public async Task<bool> IsUserPassword(ChangeOldPasswordCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isValidPass = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!isValidPass)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        } 
    }
}
