using APIGateway.Contracts.Queries.Company;
using APIGateway.Data;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.Repository.Interface.Admin;

namespace APIGateway.Handlers.Ccompany
{
    public class GetAllCompanyStructureQueryHandler : IRequestHandler<GetAllCompanyStructureQuery, CompanyStructureRespObj>
    {
        private readonly ICompanyRepository _compRepo;
        private readonly ICommonRepository _commonRepository;
        private readonly IAdminRepository _adminRepository;
        public GetAllCompanyStructureQueryHandler(ICompanyRepository companyRepository, ICommonRepository commonRepository, IAdminRepository adminRepository)
        {
            _compRepo = companyRepository;
            _commonRepository = commonRepository;
            _adminRepository = adminRepository;
        }
        public async Task<CompanyStructureRespObj> Handle(GetAllCompanyStructureQuery request, CancellationToken cancellationToken)
        {
            var countryList = await _commonRepository.GetAllCountryAsync();
            var stateList = await _commonRepository.GetAllStateAsync();
            var staffList = await _adminRepository.GetAllStaffAsync();
            var defList = await _compRepo.GetAllCompanyStructureDefinitionAsync();
            var list = await _compRepo.GetAllCompanyStructureAsync();
            return new CompanyStructureRespObj
            {
                CompanyStructures = list.Select(x => new CompanyStructureObj
                {
                    Active = x.Active,
                    Address = x?.Address,
                    Address1 = x?.Address1,
                    Address2 = x?.Address2,
                    ApplyRegistryTemplate = x?.ApplyRegistryTemplate,
                    City = x?.City,
                    Code = x?.Code,
                    CompanyId = x.CompanyId,
                    CompanyStructureId = x.CompanyStructureId,
                    CountryId = x.CountryId,
                    CountryName = countryList.FirstOrDefault(c => c.CountryId == x.CountryId)?.CountryName,
                    CreatedBy =x?.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    CurrencyId = x.CurrencyId,
                    Deleted = x.Deleted,
                    Description = x?.Description,
                    Email = x?.Email,
                    Fax = x?.Fax,
                    FSTemplateName  = x?.FSTemplateName,
                    HeadStaffId = x.HeadStaffId,
                    HeadStaffName = staffList.FirstOrDefault(d => d.StaffId == x.HeadStaffId)?.FirstName,
                    IsMultiCompany = x.IsMultiCompany,
                    Logo = x?.Logo,
                    LogoType = x?.LogoType,
                    Name = x?.Name,
                    NoOfEmployees = x.NoOfEmployees,
                    Parent = x?.Parent,
                    ParentCompanyID = x.ParentCompanyID,
                    ParentCompanyName =  x?.Parent,
                    PostalCode =x?.PostalCode,
                    RegistrationNumber = x?.RegistrationNumber,
                    RegistryTemplate = x?.RegistryTemplate,
                    ReportCurrencyId = x.ReportCurrencyId,
                    State = x?.State,
                    StructureLevel = defList?.FirstOrDefault(c => c.StructureDefinitionId == x.StructureTypeId)?.StructureLevel ?? 0,//.cor_companystructuredefinition.FirstOrDefaultAsync(c => c.StructureDefinitionId == x.StructureTypeId).Result?.StructureLevel??0,
                    StructureTypeId = x.StructureTypeId,
                    Subsidairy_Level = x.Subsidairy_Level,
                    TaxId = x.TaxId,
                    Telephone = x?.Telephone,
                    UpdatedBy = x?.UpdatedBy,
                    UpdatedOn = x.UpdatedOn,
                    WebSite = x?.WebSite,
                    StructureTypeName = defList.FirstOrDefault(c => c.StructureDefinitionId == x.StructureTypeId)?.Definition
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true, 
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete ! No Record found"
                    }
                }
            };
        }
    }
}
