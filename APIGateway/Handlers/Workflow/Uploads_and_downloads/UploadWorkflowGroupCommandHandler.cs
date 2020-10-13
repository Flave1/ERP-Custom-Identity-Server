using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.Commands.Workflow;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;
using GODP.APIsContinuation.DomainObjects.Workflow;
using System.IO;
using GODPAPIs.Contracts.GeneralExtension;

namespace APIGateway.Handlers.Workflow
{
    public class UploadWorkflowGroupCommandHandler : IRequestHandler<UploadWorkflowGroupCommand, FileUploadRespObj>
    {
        private readonly IWorkflowRepository _repo; 
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UploadWorkflowGroupCommandHandler(IWorkflowRepository repository, UserManager<cor_useraccount> userManager, IHttpContextAccessor httpContext)
        {
            _repo = repository;
            _userManger = userManager;
            _httpContextAccessor = httpContext;
        }

        public async Task<FileUploadRespObj> Handle(UploadWorkflowGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var apiResponse = new FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

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

                var uploadedRecord = new List<WorkflowGroupObj>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                if (byteList.Count() > 0)
                {
                    foreach (var byteItem in byteList)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            //Use first sheet by default
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;
                            int totalColumns = workSheet.Dimension.Columns;
                            if (totalColumns != 1)
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"One (1) Column Expected";
                                return apiResponse;
                            }
                            //First row is considered as the header
                            for (int i = 2; i <= totalRows; i++)
                            {
                                var lkp = new WorkflowGroupObj
                                {
                                    ExcelLineNumber = i,
                                    WorkflowGroupName = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty, 
                                };
                                uploadedRecord.Add(lkp);
                            }
                        }
                    } 
                }
                 
                var _DomainList = await _repo.GetAllWorkflowGroupAsync();
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {

                        if (string.IsNullOrEmpty(item.WorkflowGroupName))
                        {
                            apiResponse.Status.Message.FriendlyMessage = $"Workflow Group Name can not be empty detected on line {item.ExcelLineNumber}";
                            return apiResponse;
                        }
                        var currentItem = _DomainList.FirstOrDefault(s => s.WorkflowGroupName.Trim().ToLower() == item.WorkflowGroupName.Trim().ToLower() && s.Deleted == false);

                        if (currentItem != null)
                        {
                            currentItem.WorkflowGroupName = currentItem.WorkflowGroupName;
                            await _repo.AddUpdateWorkflowGroupAsync(currentItem);
                        }
                        else
                        {
                            var newItem = new cor_workflowgroup();
                            newItem.WorkflowGroupName = item.WorkflowGroupName; 
                            await _repo.AddUpdateWorkflowGroupAsync(newItem);
                        }
                    }
                }
                apiResponse.Status.IsSuccessful = true;
                apiResponse.Status.Message.FriendlyMessage = "Successful";
                return apiResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
