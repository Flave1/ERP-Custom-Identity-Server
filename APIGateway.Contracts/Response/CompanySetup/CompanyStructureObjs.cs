using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Response.CompanySetup
{
    public class CompanyStructureObj
    {
        public int CompanyStructureId { get; set; } 
        public string Name { get; set; } 
        public string Code { get; set; } 
        public string CountryName { get; set; }
        public string HeadStaffName { get; set; }
        public string ParentCompanyName { get; set; }
        public int StructureLevel { get; set; }
        public int? StructureTypeId { get; set; } 
        public string StructureTypeName { get; set; }
        public int? CountryId { get; set; } 
        public string Address { get; set; } 
        public int? HeadStaffId { get; set; } 
        public int? ParentCompanyID { get; set; } 
        public string Parent { get; set; } 
        public string Address1 { get; set; } 
        public string Address2 { get; set; } 
        public string Telephone { get; set; } 
        public string Fax { get; set; } 
        public string Email { get; set; } 
        public string RegistrationNumber { get; set; } 
        public string TaxId { get; set; } 
        public int? NoOfEmployees { get; set; } 
        public string WebSite { get; set; } 
        public byte[] Logo { get; set; } 
        public string LogoType { get; set; } 
        public int? City { get; set; } 
        public int? State { get; set; } 
        public int? CurrencyId { get; set; } 
        public int? ReportCurrencyId { get; set; } 
        public string ApplyRegistryTemplate { get; set; } 
        public string PostalCode { get; set; } 
        public bool? IsMultiCompany { get; set; } 
        public string Description { get; set; } 
        public int? Subsidairy_Level { get; set; } 
        public string RegistryTemplate { get; set; }

        public int? CompanyId { get; set; } 
        public string FSTemplateName { get; set; } 
        public bool? Active { get; set; } 
        public bool? Deleted { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime? UpdatedOn { get; set; }
        public int ExcelLineNumber { get; set; }
        public string StaffCode { get; set; }
    }

    public class DeleteCompanyStructure
    {
        public List<int> CompanyStructureId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AddUpdateCompanyStructureObj
    {
        public int CompanyStructureId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? StructureTypeId { get; set; }
        public int? CountryId { get; set; }
        public string Address { get; set; }
        public int? HeadStaffId { get; set; }
        public int? ParentCompanyID { get; set; }
        public string Parent { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxId { get; set; }
        public int? NoOfEmployees { get; set; }
        public string WebSite { get; set; }
        public byte[] Logo { get; set; }
        public string LogoType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? CurrencyId { get; set; }
        public int? ReportCurrencyId { get; set; }
        public string ApplyRegistryTemplate { get; set; }
        public string PostalCode { get; set; }
        public bool? IsMultiCompany { get; set; }
        public string Description { get; set; }

        public int? Subsidairy_Level { get; set; }
        public string RegistryTemplate { get; set; }

        public int? CompanyId { get; set; }
        public string FSTemplateName { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class CompanyStructureRegRespObj
    {
        public int CompanyStructureId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CompanyStructureResp2Obj
    {
        public bool IsMultiCompany { get; set; }
        public int OperatingLevel { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class CompanyStructureRespObj
    {
        public IEnumerable<CompanyStructureObj> CompanyStructures { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class DataChildrenObj
    {
        public string label { get; set; }
        public string type { get; set; }
        public string styleClass { get; set; }
        public bool expanded { get; set; }
        public StaffDataObj data { get; set; }
        public List<DataChildrenObj> children { get; set; }
    }

    public class StaffDataObj
    {
        public string name { get; set; }
        public string avatar { get; set; }
    }

    public class CompanyStructureAddInfoRespObj
    {
        public CompanyAdditionalStructureInfoObj StructureInfo { get; set; }
        public APIResponseStatus Status { get; set; }
}
}
