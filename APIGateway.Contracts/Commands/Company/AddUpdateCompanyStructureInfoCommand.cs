using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Company
{
    public class AddUpdateCompanyStructureInfoCommand : IRequest<CompanyStructureRegRespObj>
    {
        public int CompanyStructureId { get; set; }
        public int CompanyId { get; set; }
        public string Came { get; set; }
        public string CompanyCode { get; set; }
        public int? StructureTypeId { get; set; }
        public string StructureTypeName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? HeadStaffId { get; set; }
        public string HeadStaffName { get; set; }
        public int? ParentCompanyID { get; set; }
        public string ParentCompanyName { get; set; }
        public int StructureLevel { get; set; }
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
        public string CurrencyName { get; set; }
        public int? ReportingCurrencyId { get; set; }
        public int? CurrencyId { get; set; }
        public string ApplyRegistryTemplate { get; set; }
        public string PostalCode { get; set; }
        public bool IsMultiCompany { get; set; }
        public string Description { get; set; }
        public string FSTemplateName { get; set; }
        public int? Subsidairy_Level { get; set; }
        public string RegistryTemplate { get; set; }
    }
}
