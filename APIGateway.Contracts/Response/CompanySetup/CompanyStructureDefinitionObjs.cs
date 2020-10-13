using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Response.CompanySetup
{
    public class CompanyStructureDefinitionObj : GeneralEntity
    {
        public int StructureDefinitionId { get; set; } 
        public string Definition { get; set; } 
        public string Description { get; set; }

        public int StructureLevel { get; set; } 
        public bool? IsMultiCompany { get; set; }

        public int? OperatingLevel { get; set; }
        public int ExcelLineNumber { get; set; }
    }

    public class CompanyStructureDefinitionRegRespObj
    {
        public int StructureDefinitionId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class CompanyStructureDefinitionRespObj
    {
        public List<CompanyStructureDefinitionObj> CompanyStructureDefinitions { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CompanyStructureDefinitionSearchObj
    {
        public int StructureDefinitionId { get; set; }
        public int AccessId { get; set; }
        public int CompanyId { get; set; }
    }

    public class CompanyAdditionalStructureInfoObj
    {
        public int companyStructureId { get; set; }
        public int companyId { get; set; }
        public string name { get; set; }
        public string companyCode { get; set; }
        public int? structureTypeId { get; set; }
        public string structureTypeName { get; set; }
        public int? countryId { get; set; }
        public string countryName { get; set; }
        public int? headStaffId { get; set; }
        public string headStaffName { get; set; }
        public int? parentCompanyID { get; set; }
        public string parentCompanyName { get; set; }
        public int structureLevel { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string registrationNumber { get; set; }
        public string taxId { get; set; }
        public int? noOfEmployees { get; set; }
        public string webSite { get; set; }
        public byte[] logo { get; set; }
        public string logoType { get; set; }
        public int? city { get; set; }
        public int? state { get; set; }
        public string currencyName { get; set; }
        public int? reportingCurrencyId { get; set; }
        public int? currencyId { get; set; }
        public string applyRegistryTemplate { get; set; }
        public string postalCode { get; set; }
        public bool? isMultiCompany { get; set; }
        public string description { get; set; }
        public string fSTemplateName { get; set; }
        public int? subsidairy_Level { get; set; }
        public string registryTemplate { get; set; }
        public byte[] FSTemplate { get; set; }

    }

}
