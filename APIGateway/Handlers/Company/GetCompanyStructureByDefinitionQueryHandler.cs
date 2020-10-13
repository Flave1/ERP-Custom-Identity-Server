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

    public class GetCompanyStructureByDefinitionQueryHandler : IRequestHandler<GetCompanyStructureByDefinitionQuery, CompanyStructureRespObj>
    {
        private readonly ICompanyRepository _repo;
        private readonly DataContext _dataContext;
        public GetCompanyStructureByDefinitionQueryHandler(ICompanyRepository companyRepository, DataContext dataContext)
        {
            _dataContext = dataContext;
            _repo = companyRepository;
        }
        public async Task<CompanyStructureRespObj> Handle(GetCompanyStructureByDefinitionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var definition = _dataContext.cor_companystructuredefinition.Find(request.StructureDefinitionId);

                var company = (from a in _dataContext.cor_companystructure
                               where a.Deleted == false
                               select new CompanyStructureObj
                               {
                                   CompanyStructureId = a.CompanyStructureId,
                                   Name = a.Name,
                                   StructureTypeId = a.StructureTypeId,
                                   CountryId = a.CountryId,
                                   Address = a.Address,
                                   HeadStaffId = a.HeadStaffId,
                                   ParentCompanyID = a.ParentCompanyID,
                                   ParentCompanyName = a.Parent,
                                   CompanyId = a.CompanyId, 
                               }).ToList();
                foreach(var comp in company)
                {
                    comp.CountryName = _dataContext.cor_country.FirstOrDefault(x => x.CountryId == comp.CountryId)?.CountryName;
                    comp.HeadStaffName = _dataContext.cor_staff.FirstOrDefault(x => x.StaffId == comp.HeadStaffId)?.FirstName;
                    comp.StructureLevel = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == comp.StructureTypeId)?.StructureLevel??0;
                    comp.StructureTypeName = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == comp.StructureTypeId)?.Definition?? string.Empty;
                }

                var currentParent = company.Where(x => x.StructureLevel == (definition.StructureLevel - 1));

                return new CompanyStructureRespObj
                {
                    CompanyStructures = currentParent,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = currentParent.Count() > 0 ? null : "Search Complete! No matching record"
                        }

                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                return new CompanyStructureRespObj
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
