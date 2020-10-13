using APIGateway.Contracts.Queries.Company;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure; 
using GODPAPIs.Contracts.Response.CompanySetup;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Company
{
    public class GetCompanyStructureDefinitionQuery : IRequest<CompanyStructureDefinitionRespObj>
    {
        public GetCompanyStructureDefinitionQuery() { }
        public int CompanyStructureDefinitionId { get; set; }
        public GetCompanyStructureDefinitionQuery(int companyStructureDefinition)
        {
            CompanyStructureDefinitionId = companyStructureDefinition;
        }
        public class GetCompanyStructureDefinitionQueryHandler : IRequestHandler<GetCompanyStructureDefinitionQuery, CompanyStructureDefinitionRespObj>
        {

            private readonly ICompanyRepository _compRepo;
            private readonly DataContext _dataContext;
            private readonly IAdminRepository _adminRepository; 
            public GetCompanyStructureDefinitionQueryHandler(
                ICompanyRepository companyRepository,
                DataContext dataContext,
                IAdminRepository adminRepository,
                ICommonRepository commonRepository)
            {
                _compRepo = companyRepository;
                _dataContext = dataContext; 
                _adminRepository = adminRepository;
            }
            public async Task<CompanyStructureDefinitionRespObj> Handle(GetCompanyStructureDefinitionQuery request, CancellationToken cancellationToken)
            { 
                var x = await _compRepo.GetCompanyStructureDefinitionAsync(request.CompanyStructureDefinitionId);
                var respItemList = new List<CompanyStructureDefinitionObj>(); 



                if (x != null)
                {
                    respItemList.Add(new CompanyStructureDefinitionObj
                    {
                        Definition = x.Definition,
                        Description = x.Description,
                        IsMultiCompany = x.IsMultiCompany,
                        OperatingLevel = x.OperatingLevel,
                        StructureDefinitionId = x.StructureDefinitionId,
                        StructureLevel = x.StructureLevel, 
                    });
                    return new CompanyStructureDefinitionRespObj
                    {
                        CompanyStructureDefinitions = respItemList,
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                        }
                    };
                }
                return new CompanyStructureDefinitionRespObj
                {
                    CompanyStructureDefinitions = respItemList,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = respItemList.Count() > 0 ? null : "Search Commplete! No record found",
                        }
                    }
                };
            }
        }
    }
   
}
