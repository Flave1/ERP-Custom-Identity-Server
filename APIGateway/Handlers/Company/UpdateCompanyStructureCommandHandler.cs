using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure; 
using GODPAPIs.Contracts.Response.CompanySetup;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Company
{
    public class UpdateCompanyStructureCommand : IRequest<CompanyStructureRespObj>
    {
        public bool IsMultiCompany { get; set; }
        public int OperatingLevel { get; set; }
        public class UpdateCompanyStructureCommandHandler : IRequestHandler<UpdateCompanyStructureCommand, CompanyStructureRespObj>
        {
            private readonly ICompanyRepository _repo;
            public UpdateCompanyStructureCommandHandler(
                ICompanyRepository companyRepository )
            { 
                _repo = companyRepository;
            }
            public async Task<CompanyStructureRespObj> Handle(UpdateCompanyStructureCommand request, CancellationToken cancellationToken)
            { 
                var company = await _repo.GetAllCompanyStructureDefinitionAsync();
                try
                {
                    if(company.Count() > 0)
                    {
                        foreach (var item in company)
                        {
                            item.IsMultiCompany = (bool)request.IsMultiCompany;
                            item.OperatingLevel = request.OperatingLevel;
                            await _repo.AddUpdateCompanyStructureDefinitionAsync(item);
                        }
                        return new CompanyStructureRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = true,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Successful"
                                }
                            }
                        };
                    }
                    else
                    {
                        return new CompanyStructureRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "No Structure Definition found"
                                }
                            }
                        };
                    }
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                } 
            } 
        }
    } 
}
