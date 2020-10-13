using APIGateway.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Staff;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Repository.Interface.Admin
{
    public interface IAdminRepository
    {
        Task<bool> AddUpdateStaffAsync(cor_staff model);
        Task<IEnumerable<cor_staff>> GetAllStaffAsync();
        Task<cor_staff> GetStaffAsync(int staffId);
        Task<bool> StaffCodeExistAsync(string staffCode);
        Task<bool> DeleteStaffAsync(int staffId);
        Task<IEnumerable<cor_userrole>> GetAllRolesAsync();
        Task<bool> DeleteUserRoleAsync(string roleId);
        Task<byte[]> GenerateExportStaffAsync();
        Task<IEnumerable<cor_staff>> GetStaffLiteDetailAsync();
        Task<IEnumerable<cor_activityparent>> GetAllActivityParentsAsync();
        Task<IEnumerable<cor_activityparent>> GetActivitiesByRoleId(string roleId);

        Task<IEnumerable<cor_userroleadditionalactivity>> GetAllUserroleAdditionalActivities();
        Task<IEnumerable<string>> GetStaffRolesAsync(int staffId);
        Task<bool> AddUpdateUseraccessAsync(List<cor_useraccess> model);

        #region Email
        Task<bool> AddUpdateEmailConfigAsync(cor_emailconfig model); 
        Task<bool> DeleteEmailConfigAsync(int emailConfigId);
        Task<cor_emailconfig> GetEmailConfigAsync(int emailConfigId);
        Task<List<cor_emailconfig>> GetAllEmailConfigAsync();
        #endregion
    }
}
