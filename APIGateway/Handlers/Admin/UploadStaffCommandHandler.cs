using APIGateway.Contracts.Commands.Admin;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using AutoMapper; 
using GODP.APIsContinuation.DomainObjects.Staff;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.Response.Admin;
using GODPCloud.Helpers.Extensions;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic; 
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Admin
{
    public class UploadStaffCommandHandler : IRequestHandler<UploadStaffCommand, FileUploadRespObj>
    {
        private readonly IAdminRepository _repo;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICommonRepository _commonRepo;
        private readonly ICompanyRepository _companyRepository;
        private readonly DataContext _dataContext;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly IAdminRepository _admin;
        

        private List<cor_useraccount> _UserObject = new List<cor_useraccount>();
        public UploadStaffCommandHandler(
            UserManager<cor_useraccount> userManager,
            IHttpContextAccessor httpContextAccessor,
            IAdminRepository adminRepository,
            ICommonRepository commonRepository,
            IMapper mapper,
            ICompanyRepository companyRepository,
            DataContext dataContext,
            IAdminRepository admin,
            RoleManager<cor_userrole> roleManager)
        {
            _mapper = mapper;
            _commonRepo = _commonRepo = commonRepository;
            _repo = adminRepository;
            _userManager = userManager;
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _roleManager = roleManager;
            _admin = admin;
            
        }
        public async Task<FileUploadRespObj> Handle(UploadStaffCommand request, CancellationToken cancellationToken)
        {
            var response = new FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            var _StaffFromRepo = await _repo.GetAllStaffAsync();

            var _Jobtitles = await _commonRepo.GetAllJobTitleAsync();
            var _Countries = await _commonRepo.GetAllCountry();
            var _States = await _commonRepo.GetAllStateAsync();
            var _CompanyStructures = await _companyRepository.GetAllCompanyStructureAsync();

            var loggedInUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
            var loggedInUser = await _userManager.FindByIdAsync(loggedInUserId);

            List<cor_staff> _StaffObject = new List<cor_staff>(); 
            List<StaffObj> StaffRecord = new List<StaffObj>();
          
            try
            {
                var files =_httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }
                }
                int ExcelLineNumber = 0;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                if(byteList.Count() > 0)
                {
                    foreach(var byterow in byteList)
                    {
                        MemoryStream stream = new MemoryStream(byterow);
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            try
                            {
                                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                                int totalRows = workSheet.Dimension.Rows;
                                int totalColumns = workSheet.Dimension.Columns;
                              
                                if(totalColumns != 18)
                                {
                                    response.Status.Message.FriendlyMessage = "27 Columns Expected";
                                    return response;
                                }

                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var item = new StaffObj();
                                    item.ExcelLineNumber = i;
                                    item.StaffCode = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty;
                                    item.FirstName = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty;
                                    item.LastName = workSheet.Cells[i, 3]?.Value != null ? workSheet.Cells[i, 3]?.Value.ToString() : string.Empty;
                                    item.MiddleName = workSheet.Cells[i, 4]?.Value != null ? workSheet.Cells[i, 4]?.Value.ToString() : string.Empty;
                                    item.Gender = workSheet.Cells[i, 5]?.Value != null ? workSheet.Cells[i, 5]?.Value.ToString() : string.Empty;
                                    item.JobTitleName = workSheet.Cells[i, 6]?.Value != null ? workSheet.Cells[i, 6]?.Value.ToString() : string.Empty;
                                    item.StaffOfficeName = workSheet.Cells[i, 7]?.Value != null ? workSheet.Cells[i, 7]?.Value.ToString() : string.Empty;
                                    item.PhoneNumber = workSheet.Cells[i, 8]?.Value != null ? workSheet.Cells[i, 8]?.Value.ToString() : string.Empty;
                                    item.Email = workSheet.Cells[i, 9]?.Value != null ? workSheet.Cells[i, 9]?.Value.ToString() : string.Empty;
                                    item.DateOfBirth = workSheet.Cells[i, 10]?.Value != null ? DateTime.Parse(workSheet.Cells[i, 10]?.Value.ToString()) : DateTime.Now;
                                    item.Address = workSheet.Cells[i, 11]?.Value != null ? workSheet.Cells[i, 11]?.Value.ToString() : string.Empty;
                                    item.CountryName = workSheet.Cells[i, 12]?.Value != null ? workSheet.Cells[i, 12]?.Value.ToString() : string.Empty;
                                    item.StateName = workSheet.Cells[i, 13]?.Value != null ? workSheet.Cells[i, 13]?.Value.ToString() : string.Empty;
                                    item.StaffLimit = workSheet.Cells[i, 14]?.Value != null ? Convert.ToDecimal(workSheet.Cells[i, 14]?.Value.ToString()) : 0;
                                    item.AccessNames = workSheet.Cells[i, 15]?.Value != null ? workSheet.Cells[i, 15]?.Value.ToString() : string.Empty;
                                    item.UserAccessLevelsNames = workSheet.Cells[i, 16]?.Value != null ? workSheet.Cells[i, 16]?.Value.ToString() : string.Empty;
                                    item.ExcelUserRoleNames = workSheet.Cells[i, 17]?.Value != null ? workSheet.Cells[i, 17]?.Value.ToString() : string.Empty;
                                    item.UserName = workSheet.Cells[i, 18]?.Value != null ? workSheet.Cells[i, 18]?.Value.ToString() : string.Empty;
                                    item.IsHRAdmin = workSheet.Cells[i, 19]?.Value != null ? bool.Parse(workSheet.Cells[i, 19]?.Value.ToString()) : false;
                                    item.PPEAdmin = workSheet.Cells[i, 20]?.Value != null ? bool.Parse(workSheet.Cells[i, 20]?.Value.ToString()) : false;
                                    item.IsPandPAdmin = workSheet.Cells[i, 21]?.Value != null ? bool.Parse(workSheet.Cells[i, 21]?.Value.ToString()) : false;
                                    item.IsCreditAdmin = workSheet.Cells[i, 22]?.Value != null ? bool.Parse(workSheet.Cells[i, 22]?.Value.ToString()) : false;
                                    item.IsInvestorFundAdmin = workSheet.Cells[i, 23]?.Value != null ? bool.Parse(workSheet.Cells[i, 23]?.Value.ToString()) : false;
                                    item.IsDepositAdmin = workSheet.Cells[i, 24]?.Value != null ? bool.Parse(workSheet.Cells[i, 24]?.Value.ToString()) : false;
                                    item.IsTreasuryAdmin = workSheet.Cells[i, 25]?.Value != null ? bool.Parse(workSheet.Cells[i, 25]?.Value.ToString()) : false;
                                    item.IsExpenseManagementAdmin = workSheet.Cells[i, 26]?.Value != null ? bool.Parse(workSheet.Cells[i, 26]?.Value.ToString()) : false;
                                    item.IsFinanceAdmin = workSheet.Cells[i, 27]?.Value != null ? bool.Parse(workSheet.Cells[i, 27]?.Value.ToString()) : false; 
                                    StaffRecord.Add(item);
                                }

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally { excelPackage.Dispose(); }

                        }
                    }
                }

                var staffCodeRetry = 1;
               
                if (StaffRecord.Count > 0)
                {
                    foreach (var item in StaffRecord)
                    {
                        staffCodeRetry  = staffCodeRetry + 1;
                        int jobTitleId = 0;
                        int stateId = 0;
                        int countryId = 0;
                        int staffOfficeId = 0;
                        var staffCode = string.Empty;
                        var accessLevelIds = new List<int>();

                        if (!string.IsNullOrEmpty(item.JobTitleName))
                        {
                            jobTitleId = _Jobtitles.FirstOrDefault(x => x.Name.ToLower().Contains(item.JobTitleName.ToLower())).JobTitleId;
                            if (jobTitleId < 1)
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid Jobtitle {item.JobTitleName}  on line {item.ExcelLineNumber}";
                                return response;
                            }
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"Jobtitle cannot be empty on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (!string.IsNullOrEmpty(item.CountryName))
                        {
                            countryId = _Countries.FirstOrDefault(x => x.CountryName.ToLower().Contains(item.CountryName.ToLower())).CountryId;
                            if (countryId < 0)
                            {
                                response.Status.Message.FriendlyMessage = $"Could not find {item.CountryName} on line {item.ExcelLineNumber}";
                                return response;
                            }
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"Country cannot be empty on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (!string.IsNullOrEmpty(item.StateName))
                        {
                            stateId = _States.FirstOrDefault(x => x.StateName.ToLower().Contains(item.StateName.ToLower())).StateId;
                            if (stateId < 0)
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid state name {item.StateName} on line {item.ExcelLineNumber}";
                                return response; 
                            }
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"State cannot be empty on line {item.ExcelLineNumber}";
                            return response;
                        } 

                        if (!string.IsNullOrEmpty(item.StaffOfficeName))
                        {
                            staffOfficeId = _CompanyStructures.FirstOrDefault(x => x.Name.ToLower().Trim() == item.StaffOfficeName.ToLower().Trim())?.CompanyStructureId??0;
                            if (staffOfficeId < 0)
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid office or company on line {item.ExcelLineNumber}";
                                return response;
                            }
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"Office cannot be empty on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.StaffCode))
                        {
                            staffCode = StaffCode.Generate(_dataContext.cor_staff.Where(s => s.Deleted == false).OrderBy(d => d.StaffId).LastOrDefault().StaffId + staffCodeRetry);
                        }
                        else
                        {
                            staffCode = item.StaffCode;
                        }

                        if (string.IsNullOrEmpty(item.FirstName))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on First Name Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.LastName))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on LastName Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.MiddleName))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Middle Name Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.PhoneNumber))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Phone Number Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.Gender))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Gender Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.Email))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Email Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            try
                            {
                                MailAddress m = new MailAddress(item.Email); 
                            }
                            catch (FormatException)
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid Email Detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                        }

                        if(item.DateOfBirth != null)
                        {
                            if(item.DateOfBirth.Value.Year >= DateTime.Today.Year)
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid Date of birth detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Date of birth detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (string.IsNullOrEmpty(item.Address))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Address Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }

                        if (item.StaffLimit < 1)
                        {
                            item.StaffLimit = 0;
                        }

                        if (string.IsNullOrEmpty(item.AccessNames)) 
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Access Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var acc = _dataContext.cor_companystructuredefinition.FirstOrDefault(a => a.Definition.Trim().ToLower() == item.AccessNames.Trim().ToLower());
                            if(acc == null)
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid access detected on column Access on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.AccessLevelId = acc.StructureDefinitionId;
                        }

                        if (string.IsNullOrEmpty(item.UserAccessLevelsNames))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Access Levels  Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var accessLevels = item.UserAccessLevelsNames.Split(',').ToList();
                            if (!_dataContext.cor_companystructure.Any(a => accessLevels.Contains(a.Name)))
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid access level detected on column Access Levels on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.UserAccessLevels = _dataContext.cor_companystructure.Where(s => accessLevels.Contains(s.Name)).Select(w => w.CompanyStructureId).ToList();
                        }

                        if (string.IsNullOrEmpty(item.ExcelUserRoleNames))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on Roles Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var roleNames = item.ExcelUserRoleNames.Split(',').ToList();
                            if (!_roleManager.Roles.Any(a => roleNames.Contains(a.Name)))
                            {
                                response.Status.Message.FriendlyMessage = $"Invalid Role name detected on column Roles on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.UserRoleNames = _roleManager.Roles.Where(s => roleNames.Contains(s.Name)).Select(w => w.Name).ToList();
                        }

                        if (string.IsNullOrEmpty(item.UserName))
                        {
                            response.Status.Message.FriendlyMessage = $"Empty cell on UserName Column Detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                         
                        cor_staff existingStaffDetail = new cor_staff();

                        existingStaffDetail = _dataContext.cor_staff?.FirstOrDefault(s => s.StaffCode.Trim().ToLower() == staffCode.Trim().ToLower()
                           && s.Email.Trim().ToLower() == item.Email.Trim().ToLower()) ?? new cor_staff();
                        Console.WriteLine(existingStaffDetail);

                        if(existingStaffDetail.StaffId < 1)
                        {
                            existingStaffDetail = _dataContext.cor_staff.FirstOrDefault(s => s.StaffCode.Trim().ToLower() == staffCode.Trim().ToLower());
                        }

                        if (existingStaffDetail != null)
                        { 
                            existingStaffDetail.StaffCode = staffCode;
                            existingStaffDetail.FirstName = item.FirstName;
                            existingStaffDetail.LastName = item.LastName;
                            existingStaffDetail.MiddleName = item.MiddleName;
                            existingStaffDetail.JobTitle = jobTitleId;
                            existingStaffDetail.PhoneNumber = item.PhoneNumber;
                            existingStaffDetail.Email = item.Email;
                            existingStaffDetail.Address = item.Address;
                            existingStaffDetail.Photo = null;
                            existingStaffDetail.DateOfBirth = item.DateOfBirth;
                            existingStaffDetail.Gender = item.Gender == "Male" ? "1" : "2";
                            existingStaffDetail.StateId = stateId;
                            existingStaffDetail.CountryId = countryId;
                            existingStaffDetail.StaffOfficeId = staffOfficeId;
                            existingStaffDetail.AccessLevel = item.AccessLevelId;
                            existingStaffDetail.StaffLimit = item.StaffLimit; 
                            existingStaffDetail.IsHRAdmin = item.IsHRAdmin;
                            existingStaffDetail.PPEAdmin = item.PPEAdmin;
                            existingStaffDetail.IsPandPAdmin = item.IsPandPAdmin;
                            existingStaffDetail.IsCreditAdmin = item.IsCreditAdmin;
                            existingStaffDetail.IsInvestorFundAdmin = item.IsInvestorFundAdmin;
                            existingStaffDetail.IsDepositAdmin = item.IsDepositAdmin;
                            existingStaffDetail.IsTreasuryAdmin = item.IsTreasuryAdmin;
                            existingStaffDetail.IsExpenseManagementAdmin = item.IsExpenseManagementAdmin;
                            existingStaffDetail.IsFinanceAdmin = item.IsFinanceAdmin; 

                            await _repo.AddUpdateStaffAsync(existingStaffDetail);
                            var user =  _userManager.Users.FirstOrDefault(s => s.StaffId == existingStaffDetail.StaffId);
                            var createdAsUser = await AddUpdateAsUserAsync(existingStaffDetail, user.Id, item.UserName);
                            if (!createdAsUser.Succeeded)
                            {
                                response.Status.Message.FriendlyMessage = $"{createdAsUser.Errors.FirstOrDefault().Description} on line {item.ExcelLineNumber}";
                                return response;
                            } 
                            var addUpdateUserRole = await AddUpdateUserRolesAsync(item.UserRoleNames, user);
                            if (!addUpdateUserRole.Succeeded)
                            {
                                response.Status.Message.FriendlyMessage = $"{addUpdateUserRole.Errors.FirstOrDefault().Description} on line {item.ExcelLineNumber}";
                                return response;
                            } 
                            await AddUpdateAccessLevelsAsync(item.UserAccessLevels, user.Id); 
                        }
                        else
                        { 
                            var newStaffDetail = new cor_staff();
                            newStaffDetail.StaffCode = staffCode;
                            newStaffDetail.FirstName = item.FirstName;
                            newStaffDetail.LastName = item.LastName;
                            newStaffDetail.MiddleName = item.MiddleName;
                            newStaffDetail.JobTitle = jobTitleId;
                            newStaffDetail.PhoneNumber = item.PhoneNumber;
                            newStaffDetail.Email = item.Email;
                            newStaffDetail.Address = item.Address;
                            newStaffDetail.Photo = null;
                            newStaffDetail.DateOfBirth = item.DateOfBirth;
                            newStaffDetail.Gender = item.Gender == "Male" ? "1" : "2";
                            newStaffDetail.StateId = stateId;
                            newStaffDetail.CountryId = countryId;
                            newStaffDetail.StaffOfficeId = staffOfficeId;
                            newStaffDetail.AccessLevel = item.AccessLevelId; 
                            newStaffDetail.StaffLimit = item.StaffLimit;

                            newStaffDetail.IsHRAdmin = item.IsHRAdmin;
                            newStaffDetail.PPEAdmin = item.PPEAdmin;
                            newStaffDetail.IsPandPAdmin = item.IsPandPAdmin;
                            newStaffDetail.IsCreditAdmin = item.IsCreditAdmin;
                            newStaffDetail.IsInvestorFundAdmin = item.IsInvestorFundAdmin;
                            newStaffDetail.IsDepositAdmin = item.IsDepositAdmin;
                            newStaffDetail.IsTreasuryAdmin = item.IsTreasuryAdmin;
                            newStaffDetail.IsExpenseManagementAdmin = item.IsExpenseManagementAdmin;
                            newStaffDetail.IsFinanceAdmin = item.IsFinanceAdmin;

                            using (var _trans = await _dataContext.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    await _repo.AddUpdateStaffAsync(newStaffDetail);
                                    var createdAsUser = await AddUpdateAsUserAsync(newStaffDetail, null, item.UserName);
                                    if (!createdAsUser.Succeeded)
                                    {
                                        await _trans.RollbackAsync();
                                        response.Status.Message.FriendlyMessage = createdAsUser.Errors.FirstOrDefault().Description;
                                        if (createdAsUser.Errors.Any(s => s.Code == "DuplicateUserName"))
                                        {
                                            response.Status.Message.FriendlyMessage = $"{createdAsUser.Errors.FirstOrDefault().Description} on line {item.ExcelLineNumber}";
                                        }
                                        return response;
                                    }
                                    var user =  _userManager.Users.Where(s => s.Email.Trim().ToLower() == item.Email.Trim().ToLower()).ToList();
                                    if(user.Count() > 1)
                                    {
                                        await _trans.RollbackAsync();
                                        response.Status.Message.FriendlyMessage = $"Duplicate Email  Detected on line {item.ExcelLineNumber}";
                                        return response;
                                    }

                                    var addUpdateUserRole = await AddUpdateUserRolesAsync(item.UserRoleNames, user.FirstOrDefault());
                                    if (!addUpdateUserRole.Succeeded)
                                    {
                                        await _trans.RollbackAsync();
                                        response.Status.Message.FriendlyMessage = $"{addUpdateUserRole.Errors.FirstOrDefault().Description} on line {item.ExcelLineNumber}";
                                        return response;
                                    }
                                    await AddUpdateAccessLevelsAsync(item.UserAccessLevels, user.FirstOrDefault().Id);

                                    await _trans.CommitAsync();
                                    
                                }
                                catch (Exception ex)
                                {
                                    await _trans.RollbackAsync();
                                    response.Status.Message.FriendlyMessage = $"{ex?.Message ?? ex.InnerException?.Message} on line {item.ExcelLineNumber}";
                                    response.Status.Message.TechnicalMessage = ex.ToString();
                                    return response;
                                }
                                finally { await _trans.DisposeAsync(); }
                            }

                        }
                    }
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = "Successful";
                    return response;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message?? ex.InnerException.Message);
                response.Status.Message.FriendlyMessage = ex.Message;
                return response;
            } 
 
            response.Status.IsSuccessful = true;
            response.Status.Message.FriendlyMessage = "Successful";
            return response;
        }

        //IDictionary<cor_useraccount, IdentityResult> result = new Dictionary<cor_useraccount, IdentityResult>
        //    {
        //        { user, await _userManager.CreateAsync(user, "Password@1") },
        //    };

        private async Task<IdentityResult> AddUpdateAsUserAsync(cor_staff item, string userId, string username)
        {
            IdentityResult result = new IdentityResult();
            var staff = _userManager.Users.FirstOrDefault(x => x.StaffId == item.StaffId);
            var user = new cor_useraccount();
            if (staff == null)
            { 
                user.PhoneNumber = item.PhoneNumber;
                user.Email = item.Email;
                user.IsFirstLoginAttempt = true;
                user.StaffId = item.StaffId;
                user.LastLoginDate = DateTime.Now;
                user.Deleted = false;
                user.CreatedOn = DateTime.Now;
                user.IsActive = true;
                user.UserName = username;
                user.Active = true;
                return result = await _userManager.CreateAsync(user, "Password@1");
            }
            var exUser = await _userManager.FindByIdAsync(staff.Id);
            if(exUser != null)
            {
                exUser.PhoneNumber = item.PhoneNumber;
                exUser.Email = item.Email; 
                exUser.UpdatedOn = DateTime.Now; 
                exUser.UserName = username; 
                result = await _userManager.UpdateAsync(exUser);
            }
            return result;
        }
        private async Task<IdentityResult> AddUpdateUserRolesAsync(IEnumerable<string> UserRoleNames, cor_useraccount user)
        {
            
            var result = new IdentityResult();
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count() > 0)
                {
                    result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                  
                }
                result = await _userManager.AddToRolesAsync(user, UserRoleNames); 
            }
            return result;
        }
        private async Task<bool> AddUpdateAccessLevelsAsync(IEnumerable<int> useraccesses, string userId)
        {
            List<cor_useraccess> useAccesses = new List<cor_useraccess>();
            if (useraccesses.Count() > 0)
            {
                
                foreach (var acessLevelId in useraccesses)
                {
                    var access = new cor_useraccess();
                    access.AccessLevelId = acessLevelId;
                    access.UserId = userId;
                    useAccesses.Add(access);
                }
                await _admin.AddUpdateUseraccessAsync(useAccesses);
            }
            return true;
        }
    }
}
