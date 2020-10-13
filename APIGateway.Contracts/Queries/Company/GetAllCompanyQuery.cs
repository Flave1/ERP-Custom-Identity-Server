using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Queries.Company
{
    public class GetAllCompanyQuery : IRequest<CompanyRespObj> { }
}
