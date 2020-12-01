using FluentValidation;
using GODPAPIs.Contracts.Commands.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Validations
{
    public class UpdateStaffCommandVal : AbstractValidator<UpdateStaffCommand>
    {
        public UpdateStaffCommandVal()
        {
            RuleFor(x => x.Address).NotEmpty().MinimumLength(4);
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.JobTitle).NotEmpty();
            RuleFor(x => x.MiddleName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.PhoneNumber).MinimumLength(11).NotEmpty();
            RuleFor(x => x.StateId).NotEmpty();
            RuleFor(x => x.UserRoleNames).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(2);
        }

    }
}
