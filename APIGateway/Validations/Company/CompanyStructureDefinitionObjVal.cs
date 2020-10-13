using FluentValidation;
using GODPAPIs.Contracts.Response.CompanySetup; 

namespace APIGateway.Validations.Company
{
    public class CompanyStructureDefinitionObjVal : AbstractValidator<CompanyStructureDefinitionObj>
    {
        public CompanyStructureDefinitionObjVal()
        {
            RuleFor(x => x.StructureLevel).NotEmpty();  
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Definition).NotEmpty();
        }
    }
}
