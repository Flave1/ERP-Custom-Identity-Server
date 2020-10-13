using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Company
{
    public class AddUpdateCompanyStructureCommand : IRequest<CompanyStructureRegRespObj>
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
        public IFormFile Logo { get; set; }
        public string LogoType { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }

        public int? CurrencyId { get; set; }

        public int? ReportCurrencyId { get; set; }
        public string ApplyRegistryTemplate { get; set; }
        public string PostalCode { get; set; }

        public bool IsMultiCompany { get; set; }
        public string Description { get; set; }

        public int? Subsidairy_Level { get; set; }
        public string RegistryTemplate { get; set; }

        public int? CompanyId { get; set; }
        public string FSTemplateName { get; set; }
        public IFormFile FSTemplate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
