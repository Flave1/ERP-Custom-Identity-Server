using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Company
{
    public class AddUpdateCompanyCommand : IRequest<CompanyRegRespObj>
    {
        public int CompanyId { get; set; } 
        public string Code { get; set; } 
        public string Name { get; set; } 
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
        public int CountryId { get; set; } 
        public int CurrencyId { get; set; } 
        public string ApplyRegistryTemplate { get; set; } 
        public string PostalCode { get; set; } 
        public bool? IsMultiCompany { get; set; } 
        public string Description { get; set; } 
        public int? Subsidairy_Level { get; set; } 
        public string RegistryTemplate { get; set; } 
        public bool? Active { get; set; } 
        public bool? Deleted { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime? UpdatedOn { get; set; }
    }
}
