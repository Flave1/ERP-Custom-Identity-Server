using APIGateway.Contracts.Commands.Company;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Validations.Company
{
    public class AddUpdateCompanyStructureInfoCommandVal : AbstractValidator<AddUpdateCompanyStructureInfoCommand>
    {
        public AddUpdateCompanyStructureInfoCommandVal()
        {
            RuleFor(x => x.Address1).NotEmpty();
            RuleFor(x => x.Address2).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.CompanyCode).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty(); 
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.HeadStaffId).NotEmpty();
            RuleFor(x => x.IsMultiCompany).NotEmpty();
            RuleFor(x => x.ParentCompanyID).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.ReportingCurrencyId).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.StructureLevel).NotEmpty();
            RuleFor(x => x.StructureTypeId).NotEmpty();
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.Telephone).NotEmpty();
        }
    }
}
