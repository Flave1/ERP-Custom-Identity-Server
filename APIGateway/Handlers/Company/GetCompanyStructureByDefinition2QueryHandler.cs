using APIGateway.Contracts.Queries.Company;
using APIGateway.Data;

using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Response.CompanySetup;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR; 
using System; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Ccompany
{ 
    public class GetCompanyStructureByDefinition2Query : IRequest<CompanyStructureResp2Obj>
    {
         public class GetCompanyStructureByDefinition2QueryHandler : IRequestHandler<GetCompanyStructureByDefinition2Query, CompanyStructureResp2Obj>
            {
                private readonly ICompanyRepository _repo;
                private readonly DataContext _dataContext;
                public GetCompanyStructureByDefinition2QueryHandler(ICompanyRepository companyRepository, DataContext dataContext)
                {
                    _dataContext = dataContext;
                    _repo = companyRepository;
                }
                public async Task<CompanyStructureResp2Obj> Handle(GetCompanyStructureByDefinition2Query request, CancellationToken cancellationToken)
                {
                    try
                    {
                    var listOfCompanyDef = await _repo.GetAllCompanyStructureDefinitionAsync();

                        return new CompanyStructureResp2Obj
                        {
                            IsMultiCompany = listOfCompanyDef.FirstOrDefault().IsMultiCompany,
                            OperatingLevel = listOfCompanyDef.FirstOrDefault()?.OperatingLevel??0,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = true,
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        #region Log error to file 
                        var errorCode = ErrorID.Generate(4);
                        return new CompanyStructureResp2Obj
                        {

                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Error occured!! Unable to process item",
                                    MessageId = errorCode,
                                    TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                                }
                            }
                        };
                        #endregion
                    }
                }
            }
    }
   
}
