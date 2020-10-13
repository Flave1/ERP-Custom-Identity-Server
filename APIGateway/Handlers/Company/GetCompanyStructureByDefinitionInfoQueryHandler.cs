using APIGateway.Contracts.Queries.Company;
using APIGateway.Data;

using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GOSLibraries.GOS_Error_logger.Service;

namespace APIGateway.Handlers.Ccompany
{
    public class GetCompanyStructureByDefinitionInfoQueryHandler : IRequestHandler<GetCompanyStructureByDefinitionInfoQuery, CompanyStructureAddInfoRespObj>
    {
        private readonly ICompanyRepository _repo;
        private readonly DataContext _dataContext;
        public GetCompanyStructureByDefinitionInfoQueryHandler(ICompanyRepository companyRepository, DataContext dataContext)
        {
            _dataContext = dataContext;
            _repo = companyRepository;
        }
        public async Task<CompanyStructureAddInfoRespObj> Handle(GetCompanyStructureByDefinitionInfoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await(from a in _dataContext.cor_companystructure
                                    where a.Deleted == false && a.CompanyStructureId == request.StructureDefinitionId
                                    select new CompanyAdditionalStructureInfoObj
                                    {
                                        companyStructureId = a.CompanyStructureId,
                                        name = a.Name,
                                        structureTypeId = a.StructureTypeId,
                                        structureTypeName = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).Definition,
                                        countryId = a.CountryId,
                                        countryName = _dataContext.cor_country.FirstOrDefault(x => x.CountryId == a.CountryId).CountryName,
                                        address1 = a.Address1,
                                        headStaffId = a.HeadStaffId,
                                        headStaffName = _dataContext.cor_staff.FirstOrDefault(x => x.StaffId == a.HeadStaffId).FirstName,
                                        parentCompanyID = a.ParentCompanyID,
                                        parentCompanyName = a.Parent,
                                        structureLevel = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).StructureLevel,
                                        address2 = a.Address2,
                                        telephone = a.Telephone,
                                        fax = a.Fax,
                                        email = a.Email,
                                        registrationNumber = a.RegistrationNumber,
                                        taxId = a.TaxId,
                                        noOfEmployees = a.NoOfEmployees,
                                        webSite = a.WebSite,
                                        logo = a.Logo,
                                        logoType = a.LogoType,
                                        state = a.State,
                                        city = a.City,
                                        companyCode = a.Code,
                                        currencyId = a.CurrencyId,
                                        reportingCurrencyId = a.ReportCurrencyId,
                                        applyRegistryTemplate = a.ApplyRegistryTemplate,
                                        registryTemplate = a.RegistryTemplate,
                                        postalCode = a.PostalCode,
                                        isMultiCompany = a.IsMultiCompany,
                                        subsidairy_Level = a.Subsidairy_Level,
                                        description = a.Description,
                                        fSTemplateName = a.FSTemplateName,
                                    }).FirstOrDefaultAsync();

                return new CompanyStructureAddInfoRespObj
                { 
                    StructureInfo = item,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = item == null ? "Search Complete! No record found" : null
                        } 
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                return new CompanyStructureAddInfoRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
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
