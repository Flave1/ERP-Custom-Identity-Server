using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Common;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.DomainObjects.Workflow;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class UploadWorkflowLevelCommandHandler : IRequestHandler<UploadWorkflowLevelCommand, WorkflowLevelRegRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICommonRepository _commonRepo;
        public UploadWorkflowLevelCommandHandler(
            IWorkflowRepository workflowRepository,
            IHttpContextAccessor httpContextAccessor,
            ICommonRepository commonRepository)
        {
            _repo = workflowRepository;
            _commonRepo = commonRepository;
            _accessor = httpContextAccessor;
        }
        public async Task<WorkflowLevelRegRespObj> Handle(UploadWorkflowLevelCommand request, CancellationToken cancellationToken)
        { 
            var response = new WorkflowLevelRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

            var files = _accessor.HttpContext.Request.Form.Files;

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
            try
            {
                List<WorkflowLevelObj> uploadedRecord = new List<WorkflowLevelObj>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                if (byteList.Count() > 0)
                {
                    foreach(var byterow in byteList)
                    {
                        using (MemoryStream stream = new MemoryStream(byterow))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows; 
                            int totalColumns = workSheet.Dimension.Columns;
                            if (totalColumns != 7)
                            {
                                response.Status.Message.FriendlyMessage = $"Seven (7) Column Expected";
                                return response;
                            } 

                            for (int i = 2; i <= totalRows; i++)
                            {
                                uploadedRecord.Add(new WorkflowLevelObj
                                {
                                    ExcelLineNumber = i,
                                    WorkflowLevelName = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                    WorkflowGroupName = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty,
                                    Position = workSheet.Cells[i, 3]?.Value != null ? Convert.ToInt32(workSheet.Cells[i, 3]?.Value.ToString()) : 0,
                                    JobTitleName = workSheet.Cells[i, 4]?.Value != null ? workSheet.Cells[i, 4]?.Value.ToString() : string.Empty,    
                                    RequiredLimit = workSheet.Cells[i, 5]?.Value != null ? Convert.ToBoolean(workSheet.Cells[i, 5]?.Value.ToString()) : false,
                                    LimitAmount = workSheet.Cells[i, 6]?.Value != null ? Convert.ToDecimal( workSheet.Cells[i, 6]?.Value.ToString()) : 0,
                                    CanModify = workSheet.Cells[i, 7]?.Value != null ? Convert.ToBoolean(workSheet.Cells[i, 7]?.Value.ToString()) : false,
                                });
                            }
                        }
                    } 
                }

                var _DomainList = await _repo.GetAllWorkflowLevelAsync();
                var _WorkflowGroupList = await _repo.GetAllWorkflowGroupAsync();
                var _Jobtitles = await _commonRepo.GetAllJobTitleAsync();

                if (uploadedRecord.Count > 0)
                {
                    foreach (var row in uploadedRecord)
                    {

                        if (string.IsNullOrEmpty(row.WorkflowLevelName))
                        {
                            response.Status.Message.FriendlyMessage = $"Workflow Level Name can not be empty detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        if (string.IsNullOrEmpty(row.WorkflowGroupName))
                        {
                            response.Status.Message.FriendlyMessage = $"Workflow group can not be empty detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var wkgroup = _WorkflowGroupList.FirstOrDefault(a => a.WorkflowGroupName.Trim().ToLower() == row.WorkflowGroupName.Trim().ToLower());
                            if(wkgroup == null)
                            {
                                response.Status.Message.FriendlyMessage = $" Unidentified Workflow group  detected on line {row.ExcelLineNumber}";
                                return response;
                            }
                            row.WorkflowGroupId = wkgroup.WorkflowGroupId;
                        }
                        if (row.Position < 1)
                        {
                            response.Status.Message.FriendlyMessage = $"Position can not be empty detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        if (string.IsNullOrEmpty(row.JobTitleName))
                        {
                            response.Status.Message.FriendlyMessage = $"Job Title Name can not be empty detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var jobtitle = _Jobtitles.FirstOrDefault(a => a.Name.Trim().ToLower() == row.JobTitleName.Trim().ToLower());
                            if (jobtitle == null)
                            {
                                response.Status.Message.FriendlyMessage = $"Unidentified jobtitle detected on line {row.ExcelLineNumber}";
                                return response;
                            }
                            row.JobTitleId = Convert.ToString(jobtitle.JobTitleId);
                        }
                        if (row.LimitAmount < 1)
                        {
                            response.Status.Message.FriendlyMessage = $"Limit amount can not be empty detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        if (!row.RequiredLimit && row.RequiredLimit)
                        {
                            response.Status.Message.FriendlyMessage = $"Invalid required amount detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        if (!(bool)row.CanModify && (bool)row.CanModify)
                        {
                            response.Status.Message.FriendlyMessage = $"Invalid CanModify value  detected on line {row.ExcelLineNumber}";
                            return response;
                        }
                        var currentItem = _DomainList.FirstOrDefault(x => x.WorkflowLevelName.Trim().ToLower() == row.WorkflowLevelName.Trim().ToLower()
                        && x.WorkflowGroupId == row.WorkflowGroupId && x.Position ==row.Position);
                        if (currentItem == null)
                        {
                            var newItem = new cor_workflowlevel();
                            newItem.WorkflowGroupId = row.WorkflowGroupId;
                            newItem.CanModify = row.CanModify;
                            newItem.LimitAmount = row.LimitAmount;
                            newItem.RoleId = row.JobTitleId;
                            newItem.RequiredLimit = row.RequiredLimit;
                            newItem.WorkflowLevelName = row.WorkflowLevelName;
                            newItem.Position = row.Position;
                            await _repo.AddUpdateWorkflowLevelAsync(newItem);
                        }
                        else
                        {
                            currentItem.WorkflowGroupId = row.WorkflowGroupId;
                            currentItem.CanModify = row.CanModify;
                            currentItem.LimitAmount = row.LimitAmount;
                            currentItem.RoleId = row.JobTitleId;
                            currentItem.RequiredLimit = row.RequiredLimit;
                            currentItem.WorkflowLevelName = row.WorkflowLevelName;
                            currentItem.Position = row.Position;
                            await _repo.AddUpdateWorkflowLevelAsync(currentItem);
                        }
                    }
                }
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "Successful";
                return response;
            }
            catch (Exception ex)
            {
                response.Status.Message.FriendlyMessage = ex?.Message;
                return response;
            }
        }
    }
}
