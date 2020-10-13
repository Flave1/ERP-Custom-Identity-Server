using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Company
{
    public class GetAllCompanyStructureQuery : IRequest<CompanyStructureRespObj> { }
    public class GetCompanyStructureByStaffIdQuery : IRequest<CompanyStructureRespObj>
    {
        public GetCompanyStructureByStaffIdQuery() { }
        public int StaffId { get; set; }
        public GetCompanyStructureByStaffIdQuery(int staffId)
        {
            StaffId = staffId;
        }
    }
}
