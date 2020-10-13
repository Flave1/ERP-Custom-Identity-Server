using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response; 
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Company
{
    public class DeleteCompanyStructureDefinitionCommand : IRequest<DeleteRespObj>
    {
        public List<int> StructureDefinitionIds { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class DeleteCompanyStructureCommand : IRequest<DeleteRespObj>
    {
        public List<int> CompanyStructureIds { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
