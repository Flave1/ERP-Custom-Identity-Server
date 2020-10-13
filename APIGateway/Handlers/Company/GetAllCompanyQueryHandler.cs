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

namespace GODP.APIsContinuation.Handlers.Ccompany
{
    public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, CompanyRespObj>
    {
        private readonly ICompanyRepository _repo;
        private readonly IMapper _mapper;
        public GetAllCompanyQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repo = companyRepository;
        }
        public async Task<CompanyRespObj> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
        {
            var compList = await _repo.GetAllCompanyAsync();
            return new CompanyRespObj
            {
                Companies = _mapper.Map<List<CompanyObj>>(compList),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                   Message = new APIResponseMessage
                   {
                       FriendlyMessage = compList.Count() > 0 ? null : "Search Company! No record found",
                       
                   }
                }
            };
        }
    }
}
