using APIGateway.Contracts.Commands.Admin;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Admin;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GenerateExportStaffCommadHandler : IRequestHandler<GenerateExportStaffCommad, byte[]>
    {
        private readonly IAdminRepository _repo;
        private readonly DataContext _dataContext;
        private readonly ICommonRepository _commonRepo;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly UserManager<cor_useraccount> _userManager;
        public GenerateExportStaffCommadHandler(
            DataContext dataContext, 
            IAdminRepository adminRepository,
            ICommonRepository commonRepository,
            RoleManager<cor_userrole> roleManager,
            UserManager<cor_useraccount> userManager)
        {
            _repo = adminRepository;
            _dataContext = dataContext;
            _roleManager = roleManager;
            _commonRepo = commonRepository;
            _userManager = userManager;
        }
        public async Task<byte[]> Handle(GenerateExportStaffCommad request, CancellationToken cancellationToken)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("StaffCode");
                dt.Columns.Add("FirstName");
                dt.Columns.Add("LastName");
                dt.Columns.Add("MiddleName");
                dt.Columns.Add("Gender");
                dt.Columns.Add("JobTitle");
                dt.Columns.Add("Office/Department");
                dt.Columns.Add("PhoneNumber");
                dt.Columns.Add("Email");
                dt.Columns.Add("DateOfBirth");
                dt.Columns.Add("Address");
                dt.Columns.Add("Country");
                dt.Columns.Add("State");
                dt.Columns.Add("StaffLimit");
                dt.Columns.Add("Access");
                dt.Columns.Add("Access Levels");
                dt.Columns.Add("Roles");
                dt.Columns.Add("User Name");


                var structures = await (from a in _dataContext.cor_staff
                                        join b in _dataContext.Users on a.StaffId equals b.StaffId 
                                        where a.Deleted == false
                                        orderby a.FirstName
                                        select new GODPAPIs.Contracts.Response.Admin.StaffObj
                                        {
                                            StaffId = a.StaffId,
                                            StaffCode = a.StaffCode,
                                            FirstName = a.FirstName,
                                            LastName = a.LastName,
                                            MiddleName = a.MiddleName,
                                            JobTitle = a.JobTitle, 
                                            PhoneNumber = a.PhoneNumber,
                                            Email = a.Email,
                                            Address = a.Address,
                                            DateOfBirth = a.DateOfBirth,
                                            Gender = a.Gender,
                                            StateId = a.StateId,
                                            StateName = a.cor_state.StateName,
                                            CountryId = a.CountryId,
                                            Active = a.Active,
                                            UserName = b.UserName,
                                            UserId = b.Id,
                                            UserStatus = b.Active,
                                            AccessLevel = a.AccessLevel,
                                            StaffLimit = a.StaffLimit,
                                            AccessLevelId = a.AccessLevel ??0,
                                            StaffOfficeId = a.StaffOfficeId, 

                                        }).ToListAsync();
                
                foreach(var item  in structures)
                {
                    item.CountryName = _dataContext.cor_country.FirstOrDefault(s => s.CountryId == item.CountryId)?.CountryName ?? string.Empty;
                    item.StateName = _dataContext.cor_state.FirstOrDefault(s => s.StateId == item.StateId)?.StateName ?? string.Empty;
                    item.JobTitleName = _dataContext.cor_jobtitles.FirstOrDefault(s => s.JobTitleId == item.JobTitle)?.Name;
                    item.StaffOfficeName = _dataContext.cor_companystructure.FirstOrDefault(a => a.CompanyStructureId == item.StaffOfficeId)?.Name;
                    var user = await _userManager.FindByEmailAsync(item.Email);
                    if(user != null)
                    {
                        var userlevels = _dataContext.cor_useraccess.Where(s => s.UserId == user.Id).Select(w => w.AccessLevelId).ToList();
                        item.UserAccessLevelsNames = string.Join(',', _dataContext.cor_companystructure.Where(w => userlevels.Contains(w.CompanyStructureId)).Select(w => w.Name).ToList());
                        item.ExcelUserRoleNames = string.Join(",", await _userManager.GetRolesAsync(user));
                        item.AccessNames = _dataContext.cor_companystructuredefinition.FirstOrDefault(s => s.StructureDefinitionId == item.AccessLevel)?.Definition;
                    } 
                }

                foreach (var kk in structures)
                {
                    var row = dt.NewRow();
                    row["StaffCode"] = kk.StaffCode;
                    row["FirstName"] = kk.FirstName;
                    row["LastName"] = kk.LastName;
                    row["MiddleName"] = kk.MiddleName;
                    row["Gender"] = kk.Gender == "1" ? "Male" : "Female";
                    row["JobTitle"] = kk.JobTitleName;
                    row["Office/Department"] = kk.StaffOfficeName;
                    row["PhoneNumber"] = kk.PhoneNumber;
                    row["Email"] = kk.Email;
                    row["DateOfBirth"] = kk.DateOfBirth;
                    row["Address"] = kk.Address;
                    row["Country"] = kk.CountryName;
                    row["State"] = kk.StateName;
                    row["StaffLimit"] = kk.StaffLimit;
                    row["Access"] = kk.AccessNames;
                    row["Access Levels"] = kk.UserAccessLevelsNames;
                    row["Roles"] = kk.ExcelUserRoleNames;
                    row["User Name"] = kk.UserName;
                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (structures != null)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (OfficeOpenXml.ExcelPackage pck = new ExcelPackage())
                    {
                        OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.Add("StaffList");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }
                return fileBytes;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
    }
}
