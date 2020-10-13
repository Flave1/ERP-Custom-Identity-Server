using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Company
{

    public class GetCompanyStructureQuery : IRequest<CompanyStructureRespObj>
    {
        public GetCompanyStructureQuery()
        {

        }
        public  int CompanyStructureId { get; set; }
        public GetCompanyStructureQuery(int companyStructureId)
        {
            CompanyStructureId = companyStructureId;
        }
    }
}
