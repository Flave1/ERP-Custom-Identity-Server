using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Ifrs;
using GODPAPIs.Contracts.Response.CompanySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure
{
    public interface ICompanyRepository
    {


        #region Company  Setup
        Task<bool> UpdateCompanyAsync(cor_company model);
        Task<IList<cor_company>> GetAllCompanyAsync();
        Task<cor_company> GetCompanyByIdAsync(int companyId);
        Task<bool> CompanyExistAsync(string email, string name);
        #endregion

        #region Company Structure Definition
        Task<bool> AddUpdateCompanyStructureDefinitionAsync(cor_companystructuredefinition model);
        Task<IEnumerable<cor_companystructuredefinition>> GetAllCompanyStructureDefinitionAsync();
        Task<cor_companystructuredefinition> GetCompanyStructureDefinitionAsync(int Id);
        Task<bool> DeleteCompanyStructureDefinitionAsync(int Id);
        Task<byte[]> GenerateExportCompanyStructureDefinitionAsync();
        Task<bool> UploadCompanyStructureDefinitionAsync(byte[] record, string createdBy);
        Task<CompanyStructureRespObj> GetCompanyStructureByAccessIdAsync(int accessId); 
        #endregion

        Task<bool> AddUpdateCompanyStructureAsync(cor_companystructure model);
        Task<IEnumerable<cor_companystructure>> GetAllCompanyStructureAsync();
        Task<cor_companystructure> GetCompanyStructureAsync(int companyStructureId);
        Task<CompanyStructureRespObj> GetCompanyStructureByDefinitionAsync(int structureDefinitionId);
        Task<CompanyStructureRespObj> GetCompanyStructureByStaffIdAsync(int staffId);
        Task<bool> DeleteCompanyStructureAsync(int companyStructureId);
        Task<bool> UploadCompanyStructureAsync(byte[] record, string createdBy);
        Task<bool> UploadCompanyFSTemplateAsync(string filename, int companyId, string createdby);
        Task<byte[]> GenerateExportCompanyStructureAsync();
        Task<IEnumerable<DataChildrenObj>> GetCompanyStructureChartAsync();
        Task<bool> AddUpdateCompanyStructureInfoAsync(cor_companystructure model);
        Task<CompanyAdditionalStructureInfoObj> GetCompanyStructureByDefinitionInfoAsync(int structureDefinitionId);
        Task<bool> CompanyStructureExistAsync(string email, string name);
        Task<bool> UpdateCompanystructuredefinitionAsync(cor_companystructuredefinition model);
    }
}
