using AutoMapper;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Queries.Company;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APIGateway.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace GODP.APIsContinuation.Handlers.Ccompany
{
   
    public class GetFsTemplateQuery : IRequest<Fstemplate>
    {
        public GetFsTemplateQuery() { }
        public int CompanyStructureId { get; set; }
        public GetFsTemplateQuery(int compId)
        {
            CompanyStructureId = compId;
        }
        public class GetFsTemplateQueryHandler : IRequestHandler<GetFsTemplateQuery, Fstemplate>
        {
            private readonly DataContext _dataContext;
            private readonly IWebHostEnvironment _webHostEnvironment;
            public GetFsTemplateQueryHandler(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
            {
                _dataContext = dataContext;
                _webHostEnvironment = webHostEnvironment;
            }
            public async Task<Fstemplate> Handle(GetFsTemplateQuery request, CancellationToken cancellationToken)
            {
                return null;
            }
        }
    }
   
}
