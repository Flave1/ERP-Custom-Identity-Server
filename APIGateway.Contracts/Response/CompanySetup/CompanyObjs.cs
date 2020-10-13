using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Response.CompanySetup
{
    public class CompanyObj
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

    public class CompanyRespObj
    {
        public List<CompanyObj> Companies { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CompanyRegRespObj
    {
        public int CompanyId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CompanyExchangeRateObj
    {
        public int BaseCurrencyId { get; set; }
        public int CurrencyId { get; set; }
        public double BuyingRate { get; set; }
        public double SellingRate { get; set; } 
        public DateTime Date { get; set; } 
        public bool IsBaseCurrency { get; set; } 
    }

    public class CompanyExchangeRateRespObj : CompanyExchangeRateObj
    {
        public APIResponseStatus Status { get; set; }
    }

    public class Fstemplate
    {
        public string FsTempalteName { get; set; }
        public byte[] FsTemplate { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
