using APIGateway.Contracts.Queries.Company;
using APIGateway.Data;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Ccompany
{
    public class GetCompanyStructureByStaffIdQueryHandler : IRequestHandler<GetCompanyStructureByStaffIdQuery, CompanyStructureRespObj>
    {
        private DataContext _dataContext;
        private readonly UserManager<cor_useraccount> _userManager;
        public GetCompanyStructureByStaffIdQueryHandler(UserManager<cor_useraccount>  userManager, DataContext dataContext)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }
        public async Task<CompanyStructureRespObj> Handle(GetCompanyStructureByStaffIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<CompanyStructureObj> compList = new List<CompanyStructureObj>();
                var compDef = await _dataContext.cor_companystructuredefinition.FirstOrDefaultAsync(x => x.Deleted == false);
                var groupComp = _dataContext.cor_companystructure.FirstOrDefault(x => x.Deleted == false && x.ParentCompanyID == 0);
                if (groupComp == null)
                    return new CompanyStructureRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "You don't have a group company"
                            }
                        } 
                    };
               
                
                int operatingLevel = 0;
                if (compDef != null)
                {
                    if (compDef.IsMultiCompany)
                    {
                        operatingLevel = compDef.OperatingLevel ?? 0;
                    }
                }
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.StaffId == request.StaffId);
                var useraccess = _dataContext.cor_useraccess.Where(x => x.UserId == user.Id).ToList();
                if (useraccess != null)
                {
                    foreach (var item in useraccess)
                    {
                        if (item.AccessLevelId == groupComp.CompanyStructureId)
                        {
                            var structType = _dataContext?.cor_companystructuredefinition?.Where(x => x.Deleted == false && x.StructureLevel <= operatingLevel)?.ToList();
                            if (structType != null)
                            {
                                foreach (var s in structType)
                                {
                                    var cc = _dataContext.cor_companystructure.Where(x => x.Deleted == false && x.StructureTypeId == s.StructureDefinitionId).ToList();
                                    if (cc != null)
                                    {
                                        foreach (var k in cc)
                                        {
                                            var jj = (from a in _dataContext.cor_companystructure
                                                      where a.Deleted == false && a.CompanyStructureId == k.CompanyStructureId
                                                      select new CompanyStructureObj
                                                      {
                                                          CompanyStructureId = a.CompanyStructureId,
                                                          Name = a.Name,
                                                          StructureTypeId = a.StructureTypeId,
                                                          StructureTypeName = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).Definition,
                                                          CountryId = a.CountryId,
                                                          CountryName = _dataContext.cor_country.FirstOrDefault(x => x.CountryId == a.CountryId).CountryName,
                                                          Address = a.Address,
                                                          HeadStaffId = a.HeadStaffId,
                                                          HeadStaffName = _dataContext.cor_staff.FirstOrDefault(x => x.StaffId == a.HeadStaffId).FirstName,
                                                          ParentCompanyID = a.ParentCompanyID,
                                                          ParentCompanyName = a.Parent,
                                                          CompanyId = a.CompanyId,
                                                          StructureLevel = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).StructureLevel,
                                                      }).FirstOrDefault();
                                            if (jj != null)
                                            {
                                                compList.Add(jj);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var company = (from a in _dataContext.cor_companystructure
                                           where a.Deleted == false && a.CompanyStructureId == item.AccessLevelId
                                           select new CompanyStructureObj
                                           {
                                               CompanyStructureId = a.CompanyStructureId,
                                               Name = a.Name,
                                               StructureTypeId = a.StructureTypeId,
                                               StructureTypeName = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).Definition,
                                               CountryId = a.CountryId,
                                               CountryName = _dataContext.cor_country.FirstOrDefault(x => x.CountryId == a.CountryId).CountryName,
                                               Address = a.Address,
                                               HeadStaffId = a.HeadStaffId,
                                               HeadStaffName = _dataContext.cor_staff.FirstOrDefault(x => x.StaffId == a.HeadStaffId).FirstName,
                                               ParentCompanyID = a.ParentCompanyID,
                                               ParentCompanyName = a.Parent,
                                               CompanyId = a.CompanyId,
                                               StructureLevel = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).StructureLevel,
                                           }).FirstOrDefault();
                            if (company != null)
                            {
                                compList.Add(company);
                            }
                        }
                    }
                }

                return new CompanyStructureRespObj
                {
                    CompanyStructures = compList,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
