using FluentValidation;
using GODPAPIs.Contracts.Commands.Company; 

namespace GODP.APIsContinuation.Validations.Company
{
    public class AddUpdateCompanyCommandVal : AbstractValidator<AddUpdateCompanyCommand>
    {
        public AddUpdateCompanyCommandVal()
        {
            RuleFor(x => x.Address1).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Telephone).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.Address1).NotEmpty();
        }
    }
}
