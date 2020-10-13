using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Common
{
    public class CommonLookupRespObj
    {
        public List<CommonLookupsObj> CommonLookups { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class CommonLookupsObj : GeneralEntity
    {
        public int LookupId { get; set; }
        public int ParentId { get; set; }
        public string LookupName { get; set; }

        //Any Other
        public string  Code { get; set; } 
        public string ParentName { get; set; }
        public string Description { get; set; }
        public string SkillDescription { get; set; }
        public string Skills { get; set; }
        public double SellingRate { get; set; }
        public double BuyingRate { get; set; }
        public bool BaseCurrency { get; set; }
        public DateTime Date { get; set; }
        public decimal CorporateChargeAmount { get; set; }
        public decimal IndividualChargeAmount { get; set; }
        public int GLAccountId { get; set; }
        public bool IsMandatory { get; set; }
        public int ExcelLineNumber { get; set; }
    }
    public class LookUpRegRespObj
    {
        public int LookUpId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
