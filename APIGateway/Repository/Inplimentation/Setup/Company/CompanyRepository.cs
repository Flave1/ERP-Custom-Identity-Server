using APIGateway.Data;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Ifrs;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Repository.Inplimentation.Setup.Company
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<cor_useraccount> _userManager;
        public CompanyRepository(DataContext dataContext, UserManager<cor_useraccount> userManager)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        #region company
        public async Task<bool> UpdateCompanyAsync(cor_company model)
        {
            if (model.CompanyId > 0)
            {
                var item = await _dataContext.cor_company.FindAsync(model.CompanyId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_company.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;

        }
        public async Task<cor_company> GetCompanyByIdAsync(int companyId)
        {
            return await _dataContext.cor_company.SingleOrDefaultAsync(x => x.CompanyId == companyId);
        }
        public async Task<IList<cor_company>> GetAllCompanyAsync()
        {
            return await _dataContext.cor_company.Where(x => x.Deleted == false).ToListAsync();
        }
        #endregion

        #region Company Structure Definition
        public async Task<bool> AddUpdateCompanyStructureDefinitionAsync(cor_companystructuredefinition model)
        {
            if (model.StructureDefinitionId > 0)
            {
                var item = await _dataContext.cor_companystructuredefinition.FindAsync(model.StructureDefinitionId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_companystructuredefinition.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCompanyStructureDefinitionAsync(int Id)
        {
            if (Id > 0)
            {
                var item = await _dataContext.cor_companystructuredefinition.FindAsync(Id);
                item.Deleted = true;
                _dataContext.Entry(item).CurrentValues.SetValues(item);
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<byte[]> GenerateExportCompanyStructureDefinitionAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("Level");

            var structures = await (from a in _dataContext.cor_companystructuredefinition
                                    where a.Deleted == false
                                    select new CompanyStructureDefinitionObj
                                    {
                                        StructureDefinitionId = a.StructureDefinitionId,
                                        StructureLevel = a.StructureLevel,
                                        Definition = a.Definition,
                                        Description = a.Description,
                                        IsMultiCompany = (bool)a.IsMultiCompany,
                                        OperatingLevel = a.OperatingLevel,

                                    }).ToListAsync();


            foreach (var kk in structures)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Definition;
                row["Description"] = kk.Description;
                row["Level"] = kk.StructureLevel;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CompanyStructureDifinition");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public async Task<IEnumerable<cor_companystructuredefinition>> GetAllCompanyStructureDefinitionAsync()
        {
            return await _dataContext.cor_companystructuredefinition.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_companystructuredefinition> GetCompanyStructureDefinitionAsync(int Id)
        {
            return await _dataContext.cor_companystructuredefinition.SingleOrDefaultAsync(x => x.StructureDefinitionId == Id);
        }

        public async Task<bool> UploadCompanyStructureDefinitionAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return await Task.Run(() => false);

                List<CompanyStructureDefinitionObj> uploadedRecord = new List<CompanyStructureDefinitionObj>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new CompanyStructureDefinitionObj
                        {
                            Definition = workSheet.Cells[i, 1].Value.ToString(),
                            Description = workSheet.Cells[i, 2].Value.ToString(),
                            StructureLevel = int.Parse(workSheet.Cells[i, 3].Value.ToString()),
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {
                        var accountTypeExist = await _dataContext.cor_companystructuredefinition.FirstOrDefaultAsync(x => x.Definition.ToLower() == entity.Definition.ToLower());
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.Definition = entity.Definition;
                            accountTypeExist.Description = entity.Description;
                            accountTypeExist.StructureLevel = entity.StructureLevel;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = entity.CreatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new cor_companystructuredefinition
                            {
                                Definition = entity.Definition,
                                Description = entity.Description,
                                StructureLevel = entity.StructureLevel,
                                Active = true,
                                Deleted = false,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.cor_companystructuredefinition.AddAsync(accountType);
                        }
                    }
                }
                var response = await _dataContext.SaveChangesAsync() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Company Structure
        public async Task<CompanyStructureRespObj> GetCompanyStructureByDefinitionAsync(int structureDefinitionId)
        {
            var definition = await _dataContext.cor_companystructuredefinition.FindAsync(structureDefinitionId);

            var company = (from a in _dataContext.cor_companystructure
                           where a.Deleted == false 
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
                           }).ToList();
            return new CompanyStructureRespObj
            {
                CompanyStructures = company.Where(x => x.StructureLevel == (definition.StructureLevel - 1)),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true
                }
            };
        }

        public async Task<CompanyStructureRespObj> GetCompanyStructureByAccessIdAsync(int accessId)
        {
            try
            {
                var company = await (from a in _dataContext.cor_companystructure
                                     where a.Deleted == false && a.StructureTypeId == accessId
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
                                     }).ToListAsync();

                return new CompanyStructureRespObj
                {
                    CompanyStructures = company,
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = company.Count() > 0 ? null : "Search Complete! No Record Found"
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddUpdateCompanyStructureAsync(cor_companystructure model)
        {
            if (model.CompanyStructureId > 0)
            {
                var item = await _dataContext.cor_companystructure.FindAsync(model.CompanyStructureId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_companystructure.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_companystructure>> GetAllCompanyStructureAsync()
        {
            return await _dataContext.cor_companystructure.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_companystructure> GetCompanyStructureAsync(int companyStructureId)
        {
            try
            {
                return await _dataContext.cor_companystructure.FirstOrDefaultAsync(a => a.CompanyStructureId == companyStructureId && a.Deleted == false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<CompanyStructureRespObj> GetCompanyStructureByStaffIdAsync(int staffId)
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
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.StaffId == staffId);
                var useraccess = _dataContext.cor_useraccess.Where(x => x.UserId == user.Id).ToList();
                if (useraccess != null)
                {
                    foreach (var item in useraccess)
                    {
                        if (item.AccessLevelId == groupComp.CompanyStructureId)
                        {
                            var structType = _dataContext.cor_companystructuredefinition.Where(x => x.Deleted == false && x.StructureLevel <= operatingLevel).ToList();
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



        public async Task<bool> DeleteCompanyStructureAsync(int companyStructureId)
        {
            var item = await _dataContext.cor_companystructure.FindAsync(companyStructureId);
            if (item != null)
            {
                item.Deleted = true;
                _dataContext.Entry(item).CurrentValues.SetValues(item);
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UploadCompanyStructureAsync(byte[] record, string createdBy)
        {
            try
            {

                if (record == null) return false;

                List<CompanyStructureObj> uploadedRecord = new List<CompanyStructureObj>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new CompanyStructureObj
                        {
                            Name = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            StructureTypeName = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                            HeadStaffName = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                            CountryName = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,
                            ParentCompanyName = workSheet.Cells[i, 5].Value != null ? workSheet.Cells[i, 5].Value.ToString() : null,
                            Address = workSheet.Cells[i, 6].Value != null ? workSheet.Cells[i, 6].Value.ToString() : null,
                        });
                    }
                }
                List<cor_companystructure> structures = new List<cor_companystructure>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        int structureTypeId = 0;
                        int headStaffId = 0;
                        int countryId = 0;
                        int parentCompanyID = 0;
                        if (item.StructureTypeName != null)
                        {
                            var data = await _dataContext.cor_companystructuredefinition.FirstOrDefaultAsync(x => x.Definition.ToLower().Contains(item.StructureTypeName.ToLower()));
                            if (data != null) structureTypeId = data.StructureDefinitionId;
                        }
                        if (item.CountryName != null)
                        {
                            var data = await _dataContext.cor_country.FirstOrDefaultAsync(x => x.CountryName.ToLower().Contains(item.CountryName.ToLower()));
                            if (data != null) countryId = data.CountryId;
                        }
                        if (item.HeadStaffName != null)
                        {
                            var data = await _dataContext.cor_staff.FirstOrDefaultAsync(x => x.FirstName.ToLower().Contains(item.HeadStaffName.ToLower()));
                            if (data != null) headStaffId = data.StaffId;
                        }
                        if (item.ParentCompanyName != null)
                        {
                            var title = await _dataContext.cor_companystructure.FirstOrDefaultAsync(x => x.Name.ToLower().Contains(item.ParentCompanyName.ToLower()));
                            if (title != null) parentCompanyID = title.CompanyStructureId;
                        }

                        var accountTypeExist = await _dataContext.cor_companystructure.FirstOrDefaultAsync(x => x.Name.ToLower() == item.Name.ToLower());
                        if (accountTypeExist != null)
                        //if (structureTypeId != 0 && headStaffId != 0 && countryId != 0 && parentCompanyID != 0)
                        {
                            accountTypeExist.Name = item.Name;
                            accountTypeExist.StructureTypeId = structureTypeId;
                            accountTypeExist.CountryId = countryId;
                            accountTypeExist.Address = item.Address;
                            accountTypeExist.HeadStaffId = headStaffId;
                            accountTypeExist.ParentCompanyID = parentCompanyID;
                            accountTypeExist.Parent = item.ParentCompanyName;
                            accountTypeExist.CompanyId = item.CompanyId??0;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = item.UpdatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new cor_companystructure
                            {
                                Name = item.Name,
                                StructureTypeId = structureTypeId,
                                CountryId = countryId,
                                Address = item.Address,
                                HeadStaffId = headStaffId,
                                ParentCompanyID = parentCompanyID,
                                Parent = item.ParentCompanyName,
                                CompanyId = item.CompanyId??0,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,

                            };
                            _dataContext.cor_companystructure.Add(accountType);
                        }
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UploadCompanyFSTemplateAsync(string filename, int companyId, string createdby)
        {
            var company = _dataContext.cor_companystructure.FirstOrDefault(x => x.CompanyStructureId == companyId);
            if (company == null)
                return false;
            company.FSTemplateName = filename;
            company.UpdatedBy = createdby;
            company.UpdatedOn = DateTime.Now;
            var response = await _dataContext.SaveChangesAsync() > 0;
            return response;
        }

        public async Task<byte[]> GenerateExportCompanyStructureAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("Level");

            var structures = await (from a in _dataContext.cor_companystructuredefinition
                                    where a.Deleted == false
                                    select new CompanyStructureDefinitionObj
                                    {
                                        StructureDefinitionId = a.StructureDefinitionId,
                                        StructureLevel = a.StructureLevel,
                                        Definition = a.Definition,
                                        Description = a.Description,
                                        IsMultiCompany = a.IsMultiCompany,
                                        OperatingLevel = a.OperatingLevel,
                                    }).ToListAsync();


            foreach (var kk in structures)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Definition;
                row["Description"] = kk.Description;
                row["Level"] = kk.StructureLevel;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CompanyStructureDifinition");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public Task<IEnumerable<DataChildrenObj>> GetCompanyStructureChartAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUpdateCompanyStructureInfoAsync(cor_companystructure model)
        {
            if (model.CompanyId > 0)
            {
                var item = await _dataContext.cor_companystructure.FindAsync(model.CompanyId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_companystructure.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<CompanyAdditionalStructureInfoObj> GetCompanyStructureByDefinitionInfoAsync(int structureDefinitionId)
        {
            try
            {
                var company = await (from a in _dataContext.cor_companystructure
                                     where a.Deleted == false && a.CompanyStructureId == structureDefinitionId
                                     select new CompanyAdditionalStructureInfoObj
                                     {
                                         companyStructureId = a.CompanyStructureId,
                                         name = a.Name,
                                         structureTypeId = a.StructureTypeId,
                                         structureTypeName = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).Definition,
                                         countryId = a.CountryId,
                                         countryName = _dataContext.cor_country.FirstOrDefault(x => x.CountryId == a.CountryId).CountryName,
                                         address1 = a.Address1,
                                         headStaffId = a.HeadStaffId,
                                         headStaffName = _dataContext.cor_staff.FirstOrDefault(x => x.StaffId == a.HeadStaffId).FirstName,
                                         parentCompanyID = a.ParentCompanyID,
                                         parentCompanyName = a.Parent,
                                         structureLevel = _dataContext.cor_companystructuredefinition.FirstOrDefault(x => x.StructureDefinitionId == a.StructureTypeId).StructureLevel,
                                         address2 = a.Address2,
                                         telephone = a.Telephone,
                                         fax = a.Fax,
                                         email = a.Email,
                                         registrationNumber = a.RegistrationNumber,
                                         taxId = a.TaxId,
                                         noOfEmployees = a.NoOfEmployees,
                                         webSite = a.WebSite,
                                         logo = a.Logo,
                                         logoType = a.LogoType,
                                         state = a.State,
                                         city = a.City,
                                         companyCode = a.Code,
                                         currencyId = a.CurrencyId,
                                         reportingCurrencyId = a.ReportCurrencyId,
                                         applyRegistryTemplate = a.ApplyRegistryTemplate,
                                         registryTemplate = a.RegistryTemplate,
                                         postalCode = a.PostalCode,
                                         isMultiCompany = a.IsMultiCompany,
                                         subsidairy_Level = a.Subsidairy_Level,
                                         description = a.Description,
                                         fSTemplateName = a.FSTemplateName,
                                         //FSTemplate = a.FSTemplate
                                     }).FirstOrDefaultAsync();

                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CompanyExistAsync(string email, string name)
        {
            return await _dataContext.cor_company.AnyAsync(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Name.Trim().ToLower() == name.Trim().ToLower() && x.Deleted == false);
        }

        public async Task<bool> CompanyStructureExistAsync(string email, string name)
        {
            return await _dataContext.cor_companystructure.AnyAsync(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public async Task<bool> UpdateCompanystructuredefinitionAsync(cor_companystructuredefinition model)
        {
            var item = await _dataContext.cor_companystructuredefinition.FindAsync(model.StructureDefinitionId);
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync()>0;
        }


        #endregion

    }
}
