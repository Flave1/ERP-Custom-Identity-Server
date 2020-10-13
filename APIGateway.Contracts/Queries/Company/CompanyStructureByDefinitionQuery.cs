using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Company
{
    public class GetCompanyStructureByDefinitionQuery : IRequest<CompanyStructureRespObj> {
        public GetCompanyStructureByDefinitionQuery() { }
        public int StructureDefinitionId { get; set; }
        public GetCompanyStructureByDefinitionQuery(int structureDefinitionId)
        {
            StructureDefinitionId = structureDefinitionId;
        }
    }
}
