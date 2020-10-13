using APIGateway.Data;
using APIGateway.DomainObjects.Credit;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Account;
using GODP.APIsContinuation.DomainObjects.Company; 
using GODP.APIsContinuation.DomainObjects.Currency;
using GODP.APIsContinuation.DomainObjects.Operation;
using GODP.APIsContinuation.DomainObjects.Others;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Repository.Inplimentation.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DataContext _dataContext;
        public CommonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<IEnumerable<cor_branch>> GetAllBranchesAsync()
        {
            return await _dataContext.cor_branch.Where(x => x.Deleted == false).ToListAsync();
        }

        //public  async Task<IEnumerable<credit_callmemotype>> GetAllCallMemoTypeAsync()
        //{
        //    return await _dataContext.credit_callmemotype.Where(x => x.Deleted == false).ToListAsync();
        //}

        public async Task<IEnumerable<cor_city>> GetAllCityAsync()
        {
            return await _dataContext.cor_city.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_city>> GetAllCityByStateAsync(int stateId)
        {
            return await _dataContext.cor_city.Where(x => x.Deleted == false && x.StateId == stateId).ToListAsync();
        }

        public async Task<IEnumerable<cor_country>> GetAllCountryAsync()
        {
            return await _dataContext.cor_country.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_department>> GetAllDepartmentsAsync()
        {
            return await _dataContext.cor_department.Where(x => x.Deleted == false).ToListAsync();
        }

        //public async Task<IEnumerable<credit_directortype>> GetAllDirectorTypeAsync()
        //{
        //    return await _dataContext.credit_directortype.Where(x => x.Deleted == false).ToListAsync();
        //}

        public async Task<IEnumerable<credit_documenttype>> GetAllDocumentTypeAsync()
        {
            return await _dataContext.credit_documenttype.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_employertype>> GetAllEmployerTypeAsync()
        {
            return await _dataContext.cor_employertype.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_gender>> GetAllGenderAsync()
        {
            return await _dataContext.cor_gender.Where(x => x.Deleted == false).ToListAsync();
        }

        //public async Task<IEnumerable<cor_chartofaccount>> GetAllGLAccountAsync()
        //{
        //    return await _dataContext.cor_chartofaccount.Where(x => x.Deleted == false).ToListAsync();
        //}

        public async Task<IEnumerable<cor_operation>> GetAllLoanManagementOperationTypeAsync()
        {
            return await _dataContext.cor_operation.Where(x => x.Deleted == false && x.OperationTypeId == 5).ToListAsync();
        }

        public async Task<IEnumerable<cor_maritalstatus>> GetAllMaritalStatusAsync()
        {
            return await _dataContext.cor_maritalstatus.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_activityparent>> GetAllModulesAsync()
        {
            return await _dataContext.cor_activityparent.Where(x => x.Deleted == false).OrderBy(x => x.ActivityParentName).ToListAsync();
        }

        //public async Task<IEnumerable<credit_producttype>> GetAllProductTypeAsync()
        //{
        //    return await _dataContext.credit_producttype.Where(x => x.Deleted == false).ToListAsync();
        //}

        public async Task<IEnumerable<cor_state>> GetAllStateInCountryAsync(int countryId)
        {
            return await _dataContext.cor_state.Where(x => x.Deleted == false && countryId == x.CountryId).ToListAsync();
        }

        public async Task<IEnumerable<cor_state>> GetAllStateAsync()
        {
            return await _dataContext.cor_state.Where(x => x.Deleted == false).ToListAsync();
        }
        public async Task<IEnumerable<cor_title>> GetAllTitleAsync()
        {
            return await _dataContext.cor_title.Where(x => x.Deleted == false).ToListAsync();
        }
        public async Task<IEnumerable<cor_jobtitles>> GetAllJobTitleAsync()
        {
            return await _dataContext.cor_jobtitles.Where(x => x.Deleted == false).ToListAsync();
        }

        //.........
        public async Task<bool> AddUpdateCountryAsync(cor_country model)
        {
            if (model.CountryId > 0)
            {
                var item = await _dataContext.cor_country.FindAsync(model.CountryId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_country.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteCountryAsync(int countryId)
        {
            var item = await _dataContext.cor_country.FindAsync(countryId);
            _dataContext.cor_country.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<byte[]> GenerateExportCountry()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Country Name");
            dt.Columns.Add("Country Code");

            var countries = await (from a in _dataContext.cor_country
                             where a.Deleted == false && a.Active == true
                             select
                         new cor_country
                         {
                             CountryName = a.CountryName,
                             CountryCode = a.CountryCode
                         }).ToListAsync();

            foreach (var kk in countries)
            {
                var row = dt.NewRow();
                row["Country Name"] = kk.CountryName;
                row["Country Code"] = kk.CountryCode;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (countries != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("countries");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        public async Task<cor_country> GetCountryAsync(int countryId)
        {
            return await _dataContext.cor_country.FirstOrDefaultAsync(x => x.CountryId == countryId);  
        }

        public async Task<cor_country> GetCountryByCountryCodeAsync(string code)
        {
            return await _dataContext.cor_country.FirstOrDefaultAsync(x => x.CountryCode.Trim().ToLower() == code.Trim().ToLower() && x.Deleted == false);
        }

        public async Task UploadCountry(byte[] record, string createdBy)
        {
            try
            {
                var uploadedRecord = new List<cor_country>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new cor_country
                        {
                            CountryName = workSheet.Cells[i, 1].Value.ToString(),
                            CountryCode = workSheet.Cells[i, 2].Value.ToString(),
                            Active = true,
                            Deleted = false,
                            CreatedBy = createdBy,
                            CreatedOn = DateTime.Now,
                            UpdatedBy = createdBy,
                            UpdatedOn = DateTime.Now,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var countryDb = await _dataContext.cor_country.FirstOrDefaultAsync(x => x.CountryCode == item.CountryCode
                            && x.Deleted == false);

                        if (countryDb != null)
                        {
                            countryDb.CountryName = item.CountryName;
                            countryDb.CountryCode = item.CountryCode;
                            countryDb.Active = true;
                            countryDb.Deleted = false;
                            countryDb.UpdatedBy = createdBy;
                            countryDb.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var country = new cor_country
                            {
                                CountryName = item.CountryName,
                                CountryCode = item.CountryCode,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            _dataContext.cor_country.Add(country);
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddUpdateJobTitleAsync(cor_jobtitles model)
        {
            if (model.JobTitleId > 0)
            {
                var item = await _dataContext.cor_jobtitles.FindAsync(model.JobTitleId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_jobtitles.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        } 
        public async Task<cor_jobtitles> GetJobTitlAsync(int jobTitleId)
        {
            return await _dataContext.cor_jobtitles.FirstOrDefaultAsync(x => x.Deleted == false && x.JobTitleId == jobTitleId);
        }

        public async Task<bool> DeleteJobTitleAsync(int jobTitleId)
        {

            var item = await _dataContext.cor_jobtitles.FindAsync(jobTitleId);
            _dataContext.cor_jobtitles.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<byte[]> GenerateExportJobTitle()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("JobDescription");
            dt.Columns.Add("Skills");
            dt.Columns.Add("SkillDescription");

            var structures = await (from a in _dataContext.cor_jobtitles
                              where a.Deleted == false
                              select new cor_jobtitles
                              {
                                  JobTitleId = a.JobTitleId,
                                  Name = a.Name,
                                  JobDescription = a.JobDescription,
                                  Skills = a.Skills,
                                  SkillDescription = a.SkillDescription,
                              }).ToListAsync();


            foreach (var kk in structures)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["JobDescription"] = kk.JobDescription;
                row["Skills"] = kk.Skills;
                row["SkillDescription"] = kk.SkillDescription;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("JobTilte");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        public async Task<IEnumerable<cor_country>> GetAllCountry()
        {
            return await _dataContext.cor_country.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadJobTitle(byte[] record, string createdBy)
        {
            try
            {

                if (record == null) return false;

                List<cor_jobtitles> uploadedRecord = new List<cor_jobtitles>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new cor_jobtitles
                        {
                            Name = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            JobDescription = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                            Skills = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                            SkillDescription = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,

                        });
                    }
                }
                List<cor_jobtitles> jobTitles = new List<cor_jobtitles>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var accountTypeExist = _dataContext.cor_jobtitles.Where(x => x.Name.ToLower() == item.Name.ToLower()).FirstOrDefault();
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.Name = item.Name;
                            accountTypeExist.JobDescription = item.JobDescription;
                            accountTypeExist.Skills = item.Skills;
                            accountTypeExist.SkillDescription = item.SkillDescription;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = item.UpdatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {

                            var accountType = new cor_jobtitles
                            {
                                Name = item.Name,
                                JobDescription = item.JobDescription,
                                Skills = item.Skills,
                                SkillDescription = item.SkillDescription,
                                Active = true,
                                Deleted = false,
                                CreatedBy = item.CreatedBy,
                                CreatedOn = DateTime.Now

                            };
                            await _dataContext.cor_jobtitles.AddAsync(accountType);
                        }
                    }
                }
                var response = await _dataContext.SaveChangesAsync() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddUpdateCityAsync(cor_city model)
        {
            if (model.CityId > 0)
            {
                var item = await _dataContext.cor_city.FindAsync(model.CityId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_city.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<cor_city> GetCityAsync(int cityId)
        {
            return await _dataContext.cor_city.FirstOrDefaultAsync(x => x.CityId == cityId && x.Deleted == false);
        }

        public async Task<bool> DeleteCityAsync(int cityId)
        {
            var item = await _dataContext.cor_city.FindAsync(cityId);
            _dataContext.cor_city.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_city>> GetCitiesByStateAsync(int stateId)
        {
            return await _dataContext.cor_city.Where(x => x.StateId == stateId && x.Deleted == false).ToListAsync();
        }

        public  Task UploadCityAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GenerateExportCityAsync()
        {
            throw new NotImplementedException();
        }
         

        public async Task<bool> AddUpdateBranchAsync(cor_branch model)
        {
            if (model.BranchId > 0)
            {
                var item = await _dataContext.cor_branch.FindAsync(model.BranchId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_branch.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_branch>> GetAllBranchAsync()
        {
            return await _dataContext.cor_branch.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_branch> GetBranchAsync(int branchId)
        {
            return await _dataContext.cor_branch.FirstOrDefaultAsync(x => x.Deleted == false && x.BranchId == branchId);
        }

        public async Task<bool> DeleteBranchAsync(int branchId)
        {
            var item = await _dataContext.cor_branch.FindAsync(branchId); 
            _dataContext.cor_branch.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<cor_branch> GetBranchsByCompanyAsync(int companyId)
        {
            return await _dataContext.cor_branch.FirstOrDefaultAsync(x => x.CompanyId == companyId && x.Deleted == false);
        }

        public async Task<bool> AddUpdateDocumentTypeAsync(credit_documenttype entity)
        {
            if (entity.DocumentTypeId > 0)
            {
                var item = await _dataContext.credit_documenttype.FindAsync(entity.DocumentTypeId);
                _dataContext.Entry(item).CurrentValues.SetValues(entity);
            }
            else
                await _dataContext.credit_documenttype.AddAsync(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public Task<bool> UploadDocumentTypeAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GenerateExportDocumentTypeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteDocumentTypeAsync(int documentTypeId)
        {
            var item = await _dataContext.credit_documenttype.FindAsync(documentTypeId);
            _dataContext.credit_documenttype.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<credit_documenttype> GetDocumenttypeAsync(int documentTypeId)
        {
            return await _dataContext.credit_documenttype.FirstOrDefaultAsync(x => x.Deleted == false && x.DocumentTypeId == documentTypeId);
        }

        public async Task<bool> AddUpdateCurrencyRateAsync(cor_currencyrate model)
        {
            if (model.CurrencyRateId > 0)
            {
                var item = await _dataContext.cor_currencyrate.FindAsync(model.CurrencyRateId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_currencyrate.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_currencyrate>> GetAllCurrencyRateAsync()
        {
            return await _dataContext.cor_currencyrate.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_currencyrate> GetCurrencyRateAsync(int currencyRateId)
        {
            return await _dataContext.cor_currencyrate.FirstOrDefaultAsync(x => x.CurrencyRateId == currencyRateId && x.Deleted == false);
        }

        public async Task<bool> DeleteCurrencyRateAsync(int currencyRateId)
        {
            var item = await _dataContext.cor_currencyrate.FindAsync(currencyRateId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<cor_currencyrate> GetCurrencyRatesByCurrencyAsync(int currencyId)
        {
            return await _dataContext.cor_currencyrate.FirstOrDefaultAsync(x => x.CurrencyId == currencyId && x.Deleted == false);
        }

        public Task UploadCurrencyRateAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateExportCurrencyRateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUpdateIdentificationAsync(cor_identification model)
        {
            if (model.IdentificationId > 0)
            {
                var item = await _dataContext.cor_identification.FindAsync(model.IdentificationId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_identification.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_identification>> GetAllIdentificationAsync()
        {
            return await _dataContext.cor_identification.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_identification> GetIdentificationAsync(int identificationId)
        {
            return await _dataContext.cor_identification.FirstOrDefaultAsync(x => x.IdentificationId == identificationId && x.Deleted == false);
        }

        public async Task<bool> DeleteIdentificationAsync(int identificationId)
        {
            var item = await _dataContext.cor_identification.FindAsync(identificationId);
            _dataContext.cor_identification.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public Task UploadIdentificationAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateExportIdentificationAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUpdateCurrencyAsync(cor_currency model)
        {
            if (model.CurrencyId > 0)
            {
                var item = await _dataContext.cor_currency.FindAsync(model.CurrencyId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_currency.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_currency>> GetAllCurrencyAsync()
        {
            return await _dataContext.cor_currency.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_currency> GetCurrencyAsync(int currencyId)
        {
            return await _dataContext.cor_currency.FirstOrDefaultAsync(x => x.CurrencyId == currencyId && x.Deleted == false);
        }

        public async Task<bool> DeleteCurrencyAsync(int currencyId)
        {
            var item = await _dataContext.cor_currency.FindAsync(currencyId);
            _dataContext.cor_currency.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public Task UploadCurrencyAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateExportCurrencyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<cor_currency> GetCurrencyByCurrencyCodeAsync(string currencyCode)
        {
            return await _dataContext.cor_currency.FirstOrDefaultAsync(x => x.CurrencyCode.Trim().ToLower() == currencyCode .ToLower().Trim() && x.Deleted == false);
        }

     

        //public async Task<IEnumerable<credit_creditbureau>> GetAllCreditBureauAsync()
        //{
        //    return await _dataContext.credit_creditbureau.Where(d => d.Deleted == false).ToListAsync();
        //}

        //public async Task<credit_creditbureau> GetCreditBureauAsync(int creditBureauId)
        //{
        //    return await _dataContext.credit_creditbureau.FirstOrDefaultAsync(x => x.CreditBureauId == creditBureauId && x.Deleted == false);
        //}

        public Task<byte[]> GenerateExportCreditBureauAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadCreditBureauAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

     

        public Task<bool> DeleteCreditBureauAsync(int creditBureauId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUpdateStateAsync(cor_state model)
        {
            if(model.StateId > 0)
            {
                var item = await _dataContext.cor_state.FindAsync(model.StateId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_state.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStateAsync(int stateId)
        {
            var item = await _dataContext.cor_state.FindAsync(stateId);
            _dataContext.cor_state.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public Task UploadStateAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GenerateExportStateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<cor_state> GetStateAsync(int stateId)
        {
            return await _dataContext.cor_state.FindAsync(stateId);
        }

        //public async Task<bool> DeleteCreditBureauAsync(int creditBureauId)
        //{
        //    var item = await _dataContext.credit_creditbureau.FindAsync(creditBureauId);
        //    item.Deleted = true;
        //    _dataContext.Entry(item).CurrentValues.SetValues(item);
        //    return await _dataContext.SaveChangesAsync() > 0;
        //}
    }
}
