using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Company
{
    public class GetCompanyStructureByDefinitionInfoQuery : IRequest<CompanyStructureAddInfoRespObj>
    {
        public GetCompanyStructureByDefinitionInfoQuery()
        {

        }
        public int StructureDefinitionId { get; set; }
        public GetCompanyStructureByDefinitionInfoQuery(int structureDefinitionId)
        {
            StructureDefinitionId = StructureDefinitionId;
        }
    }
}
