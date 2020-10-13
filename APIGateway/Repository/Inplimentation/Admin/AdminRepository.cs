using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Staff;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.Response.Admin;
using GOSLibraries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Repository.Inplimentation.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly UserManager<cor_useraccount> _userManager;
        public AdminRepository(DataContext dataContext, RoleManager<cor_userrole> roleManager, UserManager<cor_useraccount> userManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<bool> AddUpdateStaffAsync(cor_staff model)
        {
            if(model.StaffId > 0)
            {
                var item = await _dataContext.cor_staff.FindAsync(model.StaffId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else 
                await _dataContext.cor_staff.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }
 
        public async Task<bool> DeleteStaffAsync(int staffId)
        {
            var staffDetails = await _dataContext.cor_staff.FindAsync(staffId);
            if(staffDetails != null)
            {
                var userDetails = await _userManager.Users.FirstOrDefaultAsync(s => s.StaffId == staffId);
                _dataContext.cor_staff.Remove(staffDetails);
                if (userDetails != null)
                {
                    await _userManager.DeleteAsync(userDetails); 
                }
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role != null)
            {
                var roleActivities =  _dataContext.cor_userroleactivity.Where(s => s.RoleId == role.Id).ToList();
                await _roleManager.DeleteAsync(role);
                if(roleActivities.Count() > 0)
                {
                    _dataContext.cor_userroleactivity.RemoveRange(roleActivities);
                }
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }
         
        public async Task<byte[]> GenerateExportStaffAsync()
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
                                  JobTitleName = _dataContext.cor_jobtitles.FirstOrDefault(x => x.JobTitleId == a.JobTitle).Name,
                                  StaffOfficeName = _dataContext.cor_companystructure.FirstOrDefault(x => x.CompanyStructureId == a.StaffOfficeId).Name,
                                  PhoneNumber = a.PhoneNumber,
                                  Email = a.Email,
                                  Address = a.Address,
                                  DateOfBirth = a.DateOfBirth,
                                  Gender = a.Gender,
                                  StateId = a.StateId,
                                  StateName = a.cor_state.StateName,
                                  CountryId = a.CountryId,
                                  CountryName = a.cor_country.CountryName,
                                  Active = a.Active,
                                  UserName = b.UserName,
                                  UserId = b.Id,
                                  UserStatus = b.Active

                              }).ToListAsync();


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
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (OfficeOpenXml.ExcelPackage pck = new OfficeOpenXml.ExcelPackage())
                {
                    OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.Add("StaffList");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public async Task<IEnumerable<cor_activityparent>> GetAllActivityParentsAsync()
        {
            return await _dataContext.cor_activityparent.Where(x => x.Deleted == false).ToListAsync();
        }

     

        public Task<IEnumerable<cor_activityparent>> GetActivitiesByRoleId(string roleId)
        {
            throw new NotFiniteNumberException();
        }

        public async Task<IEnumerable<cor_staff>> GetAllStaffAsync()
        {
            return await _dataContext.cor_staff.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_staff> GetStaffAsync(int staffId)
        {
            return await _dataContext.cor_staff.FirstOrDefaultAsync(x => x.Deleted == false && x.StaffId == staffId);
        }


        public async Task<IEnumerable<cor_userrole>> GetAllRolesAsync()
        {
            var er =  await _roleManager.Roles.Where(s => s.Name != StaticRoles.GODP).ToListAsync();
            return er;
        }

        public Task<IEnumerable<cor_staff>> GetStaffLiteDetailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<cor_activityparent>> GetUserRoleAndActivities(int userRoleId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StaffCodeExistAsync(string staffCode)
        {
           var data = await  _dataContext.cor_staff.AnyAsync(x => x.StaffCode.ToLower().Trim() == staffCode.ToLower().Trim());
            return data;
        }

        public async Task<bool> UploadStaffAsync(List<StaffObj> staffs,  string createdBy)
        {
            try
            {
                List<cor_staff> structures = new List<cor_staff>();
                if (staffs.Count > 0)
                { 
                    foreach (var item in staffs)
                    {
                        int jobTitleId = 0;
                        int stateId = 0;
                        int countryId = 0;
                        int staffOfficeId = 0;
                        if (item.JobTitleName != null)
                        {
                             jobTitleId = _dataContext.cor_jobtitles.FirstOrDefault(x => x.Name.ToLower().Contains(item.JobTitleName.ToLower())).JobTitleId;
                            if (jobTitleId < 1)
                            {
                                throw new Exception("Invalid Selection", new Exception("Please select a valid Jobtitle"));
                            } 
                        }
                        if (item.CountryName != null)
                        {
                            countryId = _dataContext.cor_country.FirstOrDefault(x => x.CountryName.ToLower().Contains(item.CountryName.ToLower())).CountryId;
                            if (countryId < 0)
                            {
                                throw new Exception("Invalid Selection", new Exception("Please select a valid country name"));
                            } 
                        }

                        if (item.StateName != null)
                        {
                             stateId = _dataContext.cor_state.FirstOrDefault(x => x.StateName.ToLower().Contains(item.StateName.ToLower())).StateId;
                            if (stateId < 0)
                            {
                                throw new Exception("Invalid Selection", new Exception("Please select a valid state name"));
                            } 
                        }

                        if (item.StaffOfficeName != null)
                        {
                            staffOfficeId = _dataContext.cor_companystructure.FirstOrDefault(x => x.Name.ToLower().Contains(item.StaffOfficeName.ToLower())).CompanyStructureId;
                            if (staffOfficeId < 0)
                            {
                                throw new Exception("Invalid Selection", new Exception("Please select a valid office or company"));
                            } 
                        } 
                        if (!await StaffCodeExistAsync(item.StaffCode))
                        {
                            var structure = new cor_staff
                            {
                                StaffCode = item.StaffCode,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                MiddleName = item.MiddleName,
                                JobTitle = jobTitleId,
                                PhoneNumber = item.PhoneNumber,
                                Email = item.Email,
                                Address = item.Address,
                                DateOfBirth = item.DateOfBirth,
                                Gender = item.Gender == "Male" ? "1" : "2",
                                StateId = stateId,
                                CountryId = countryId,
                                StaffOfficeId = staffOfficeId,
                                AccessLevel = null,
                                StaffLimit = item.StaffLimit,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                                Active = true,
                                Deleted = false,
                            };
                            structures.Add(structure);
                        }
                    }
                    _dataContext.cor_staff.AddRange(structures);
                }

                bool response = false;
                try
                {
                    response = _dataContext.SaveChanges() > 0;
                    if (response)
                    {
                        try
                        { 
                            foreach (var item in structures)
                            {
                                var staff = _dataContext.cor_staff.Where(x => x.StaffCode == item.StaffCode).FirstOrDefault();
                                if (staff != null)
                                {
                                    var user = new cor_useraccount
                                    {
                                        UserName = item.FirstName.ToLower(),
                                        StaffId = staff.StaffId,
                                        Email = item.Email,
                                        EmailConfirmed = true, 
                                        PhoneNumber = item.PhoneNumber,
                                        PhoneNumberConfirmed = true,
                                        TwoFactorEnabled = false,
                                        LockoutEnabled = true,
                                        AccessFailedCount = 0,
                                        IsFirstLoginAttempt = true, 
                                        SecurityAnswer = "",
                                        NextPasswordChangeDate = DateTime.Now.AddDays(30),
                                        IsActive = true,
                                        CreatedBy = staff.CreatedBy,
                                        CreatedOn = DateTime.Now,
                                        Active = true,
                                        Deleted = false,
                                    };
                                    var created = await _userManager.CreateAsync(user, "Password@1"); 
                                } 
                            } 
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        } 
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<bool> UsernameExistAsync(string username)
        {
            throw new NotImplementedException();
        }

        public bool UserRoleExistAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<cor_userroleadditionalactivity>> GetAllUserroleAdditionalActivities()
        {
            return await _dataContext.cor_userroleadditionalactivity.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetStaffRolesAsync(int staffId)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(s => s.StaffId == staffId);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddUpdateUseraccessAsync(List<cor_useraccess> model)
        {

            if (model.Count() != 0 && !string.IsNullOrEmpty(model.FirstOrDefault().UserId))
            {
                var currentUserLevels = _dataContext.cor_useraccess.Where(s => s.UserId == model.FirstOrDefault().UserId).ToList();
                if (currentUserLevels.Count() > 0)
                {
                    _dataContext.cor_useraccess.RemoveRange(currentUserLevels);
                    await _dataContext.SaveChangesAsync();
                }
                if (model.Count() > 0)
                {
                    foreach (var userAces in model)
                    {
                        await _dataContext.cor_useraccess.AddAsync(userAces);
                        await _dataContext.SaveChangesAsync();
                    }
                }
                else
                    return true;
            }
            return true;
        }

        public async Task<bool> AddUpdateEmailConfigAsync(cor_emailconfig model)
        {
            if (model.EmailConfigId > 0)
            {
                var item = await _dataContext.cor_emailconfig.FindAsync(model.EmailConfigId);
                if(item != null)
                {
                    _dataContext.Entry(item).CurrentValues.SetValues(model);
                }
            }
            else
                await _dataContext.cor_emailconfig.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEmailConfigAsync(int emailConfigId)
        {
            var item = await _dataContext.cor_emailconfig.FindAsync(emailConfigId);  
            if(item != null)
            {
                _dataContext.cor_emailconfig.Remove(item);
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<cor_emailconfig> GetEmailConfigAsync(int emailConfigId)
        {
            return await _dataContext.cor_emailconfig.FirstOrDefaultAsync(x => x.Deleted == false && x.EmailConfigId == emailConfigId);
        }

        public async Task<List<cor_emailconfig>> GetAllEmailConfigAsync()
        {
            return await _dataContext.cor_emailconfig.Where(x => x.Deleted == false).OrderByDescending(q => q.EmailConfigId).ToListAsync();
        }
    }
}
