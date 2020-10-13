using APIGateway.Contracts.Response.Workflow;
using APIGateway.DomainObjects.Workflow;
using GODP.APIsContinuation.DomainObjects.Operation;
using GODP.APIsContinuation.DomainObjects.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Repository.Interface.Workflow
{
    public interface IWorkflowRepository
    {
        #region Workflow Group
        Task<bool> AddUpdateWorkflowGroupAsync(cor_workflowgroup model);
        Task<IEnumerable<cor_workflowgroup>> GetAllWorkflowGroupAsync();
        Task<cor_workflowgroup> GetWorkflowGroupAsync(int workflowGroupId);
        Task<bool> DeleteWorkflowGroupAsync(int workflowGroupId);
        Task<IEnumerable<cor_operationtype>> GetAllOperationTypesAsync();
        Task<IEnumerable<cor_operation>> GetAllOperationAsync();  
        Task<bool> UploadWorkflowGroupAsync(byte[] record, string createdBy);
        Task<byte[]> GenerateExportWorkflowGroupAsync();
        Task<bool> WorkflowGroupExistAsync(string name);
        #endregion
         
        #region Workflow Level
        Task<bool> AddUpdateWorkflowLevelAsync(cor_workflowlevel model);
        Task<IEnumerable<cor_workflowlevel>> GetAllWorkflowLevelAsync();
        Task<cor_workflowlevel> GetWorkflowLevelAsync(int workflowLevelId);
        Task<bool> DeleteWorkflowLevelAsync(int workflowLevelId);
        Task<IEnumerable<cor_workflowlevel>> GetWorkflowLevelsByWorkflowGroupAsync(int workflowGroupId);
        Task<bool> UploadWorkflowLevelAsync(byte[] record, string createdBy);
        Task<byte[]> GenerateExportWorkflowLevelAsync();
        #endregion
         
        #region Workflow operation

        Task<IEnumerable<cor_operation>> GetAllWorkflowOperationAsync();
        Task<bool> UpdateWorkflowOperationAsync(List<cor_operation> entity);

        #endregion

        #region workflow level staff
        Task<bool> AddUpdateWorkflowLevelStaffAsync(cor_workflowlevelstaff model);
        Task<IEnumerable<cor_workflowlevelstaff>> GetAllWorkflowLevelStaffAsync();
        Task<cor_workflowlevelstaff> GetWorkflowLevelStaffAsync(int workflowLevelStaffId);
        Task<bool> DeleteWorkflowLevelStaffAsync(int workflowLevelStaffId);
        Task<cor_workflowlevelstaff> GetWorkflowLevelStaffsByStaffAsync(int staffId);
        Task<IEnumerable<cor_workflowlevelstaff>> GetWorkflowLevelStaffsByWorkflowLevelAsync(int workflowLevelId); 
        Task<byte[]> GenerateExportWorkflowLevelStaffAsync();
        Task<bool> UploadWorkflowLevelStaffAsync(byte[] record, string createdBy);

        #endregion
         
        #region Workflow Mapping
        Task<IEnumerable<cor_workflow>> GetWorkflowByOperationAsync(int operationId);
        Task<cor_workflow> GetSingleWorkflowAsync(int workflowId);
        Task<bool> AddUpdateWorkflowAsync(cor_workflow model);
        Task<bool> CheckIfOperationAndAccessExistAsync(int operationId, int[] accessLevelIds);
        bool DeleteWorkflowAsync(int workflowId);
        #endregion

        #region Workflow Access
        Task<bool> AddWorkFlowDetailsAccessAsync();
        Task<IEnumerable<cor_workflowaccess>> GetAllWorkflowAccessAsync();

        Task<IEnumerable<cor_workflowdetails>> GetAllWorkflowdetailsAsync();
        #endregion

        #region Workflow task
        Task<bool> CreateUpdateWorkflowTaskAsync(cor_workflowtask model);
        Task<IEnumerable<cor_workflowtask>> GetAllWorkflowTaskAsync();
        public Task<IEnumerable<cor_workflowtask>> GetCurrentWorkTaskAsync(int operationId, int targetId, int workflowId, string token);

        Task<IEnumerable<cor_workflowtask>> GetAnApproverWorkflowTaskAsync(string email, int accessId); 

        #endregion

        #region Workflow Details
        Task<IEnumerable<cor_workflowdetails>> GetWorkflowdetailsAsync(int workflowId);
        Task<IEnumerable<cor_workflow>> GetAllWorkflowAsync();
        Task<IEnumerable<cor_workflowdetailsaccess>> GetAllWorkflowdetailAccessAsync();
        #endregion

    }
}
