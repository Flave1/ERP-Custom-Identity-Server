using APIGateway.DomainObjects.Credit;
using GODP.APIsContinuation.DomainObjects.Account;
using GODP.APIsContinuation.DomainObjects.Company;  
using GODP.APIsContinuation.DomainObjects.Currency;
using GODP.APIsContinuation.DomainObjects.Operation;
using GODP.APIsContinuation.DomainObjects.Others;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGateway.Repository.Interface.Common
{
    public interface ICommonRepository
    {
        Task<IEnumerable<cor_gender>> GetAllGenderAsync();
        Task<IEnumerable<cor_title>> GetAllTitleAsync();
        Task<IEnumerable<cor_employertype>> GetAllEmployerTypeAsync();
        Task<IEnumerable<cor_maritalstatus>> GetAllMaritalStatusAsync();
        Task<IEnumerable<cor_city>> GetAllCityAsync();
        Task<IEnumerable<cor_city>> GetAllCityByStateAsync(int stateId);
        
        Task<IEnumerable<cor_country>> GetAllCountryAsync();
        Task<IEnumerable<cor_state>> GetAllStateInCountryAsync(int countryId);
        Task<IEnumerable<cor_state>> GetAllStateAsync();
        Task<cor_state> GetStateAsync(int stateId);
        Task<IEnumerable<credit_documenttype>> GetAllDocumentTypeAsync();
        //Task<IEnumerable<credit_directortype>> GetAllDirectorTypeAsync();
        //Task<IEnumerable<credit_producttype>> GetAllProductTypeAsync();
        //Task<IEnumerable<cor_chartofaccount>> GetAllGLAccountAsync();
        //Task<IEnumerable<credit_callmemotype>> GetAllCallMemoTypeAsync();
        Task<IEnumerable<cor_operation>> GetAllLoanManagementOperationTypeAsync();
        Task<IEnumerable<cor_department>> GetAllDepartmentsAsync();
        Task<IEnumerable<cor_branch>> GetAllBranchesAsync();
        Task<IEnumerable<cor_activityparent>> GetAllModulesAsync();

        //..........
        Task<bool> AddUpdateCountryAsync(cor_country model);
        Task<IEnumerable<cor_country>> GetAllCountry();
        Task<cor_country> GetCountryAsync(int countryId);
        Task<bool> DeleteCountryAsync(int countryId);
        Task<cor_country> GetCountryByCountryCodeAsync(string c);
        Task UploadCountry(byte[] record, string createdBy);
        Task<byte[]> GenerateExportCountry();
        //............

        #region Job Title Setup
        Task<bool> AddUpdateJobTitleAsync(cor_jobtitles model);
        Task<IEnumerable<cor_jobtitles>> GetAllJobTitleAsync();
        Task<cor_jobtitles> GetJobTitlAsync(int jobTitleId);
        Task<bool> DeleteJobTitleAsync(int jobTitleId);
        Task<byte[]> GenerateExportJobTitle();
        Task<bool> UploadJobTitle(byte[] record, string createdBy);
        #endregion

        #region State
        Task<bool> AddUpdateStateAsync(cor_state model); 
        Task<bool> DeleteStateAsync(int stateId); 
        Task UploadStateAsync(byte[] record, string createdBy);
        Task<byte[]> GenerateExportStateAsync();
        #endregion

        #region City
        Task<bool> AddUpdateCityAsync(cor_city model); 
        Task<cor_city> GetCityAsync(int cityId);
        Task<bool> DeleteCityAsync(int cityId);
        Task<IEnumerable<cor_city>> GetCitiesByStateAsync(int stateId);
        Task UploadCityAsync(byte[] record, string createdBy);
        Task<byte[]> GenerateExportCityAsync(); 
        #endregion

        #region Branches
        Task<bool> AddUpdateBranchAsync(cor_branch model);
        Task<IEnumerable<cor_branch>> GetAllBranchAsync();
        Task<cor_branch> GetBranchAsync(int branchId);
        Task<bool> DeleteBranchAsync(int branchId);
        Task<cor_branch> GetBranchsByCompanyAsync(int companyId);
        #endregion

        #region Document type
        Task<bool> AddUpdateDocumentTypeAsync(credit_documenttype model);
        Task<bool> UploadDocumentTypeAsync(byte[] record, string createdBy); 
        Task<bool> DeleteDocumentTypeAsync(int documentTypeId);
        Task<credit_documenttype> GetDocumenttypeAsync(int documentTypeId);
        #endregion

        #region Currency Rate
        Task<bool> AddUpdateCurrencyRateAsync(cor_currencyrate model);
        Task<IEnumerable<cor_currencyrate>> GetAllCurrencyRateAsync();
        Task<cor_currencyrate> GetCurrencyRateAsync(int currencyRateId);
        Task<bool> DeleteCurrencyRateAsync(int currencyRateId); 
        Task<cor_currencyrate> GetCurrencyRatesByCurrencyAsync(int currencyId);
        Task UploadCurrencyRateAsync(byte[] record, string createdBy);
        byte[] GenerateExportCurrencyRateAsync();
        #endregion

        #region Identification
        Task<bool> AddUpdateIdentificationAsync(cor_identification model);
        Task<IEnumerable<cor_identification>> GetAllIdentificationAsync();
        Task<cor_identification> GetIdentificationAsync(int identificationId);
        Task<bool> DeleteIdentificationAsync(int identificationId);
        //Task UploadIdentificationAsync(byte[] record, string createdBy);
        //byte[] GenerateExportIdentificationAsync();
        #endregion

        #region Currency

        Task<bool> AddUpdateCurrencyAsync(cor_currency model);
        Task<IEnumerable<cor_currency>> GetAllCurrencyAsync();
        Task<cor_currency> GetCurrencyAsync(int currencyId);
        Task<bool> DeleteCurrencyAsync(int currencyId);
        Task UploadCurrencyAsync(byte[] record, string createdBy);
        byte[] GenerateExportCurrencyAsync(); 
        Task<cor_currency> GetCurrencyByCurrencyCodeAsync(string currencyCode);

        #endregion

        #region Credit bureau
        //Task<bool> AddUpdateCreditBureauAsync(credit_creditbureau entity);
        //Task<IEnumerable<credit_creditbureau>> GetAllCreditBureauAsync();
        //Task<credit_creditbureau> GetCreditBureauAsync(int creditBureauId);
        Task<byte[]> GenerateExportCreditBureauAsync();
        Task<bool> UploadCreditBureauAsync(byte[] record, string createdBy);
        Task<bool> DeleteCreditBureauAsync(int creditBureauId); 
        #endregion
    }
}
