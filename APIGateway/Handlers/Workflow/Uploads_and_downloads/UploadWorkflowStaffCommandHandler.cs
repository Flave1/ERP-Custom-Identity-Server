using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using GOSLibraries.GOS_API_Response; 
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml; 
using System.IO;
using GODPAPIs.Contracts.GeneralExtension;
using APIGateway.Contracts.Response.Workflow;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODP.APIsContinuation.DomainObjects.Workflow;

namespace APIGateway.Handlers.Workflow
{
    public class UploadWorkflowStaffCommand : IRequest<FileUploadRespObj>
    {
        public class UploadWorkflowStaffCommandHandler : IRequestHandler<UploadWorkflowStaffCommand, FileUploadRespObj>
        {
            private readonly IWorkflowRepository _repo;
            private readonly UserManager<cor_useraccount> _userManger;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IAdminRepository _admin;
            private readonly ICompanyRepository _comprepository;
            public UploadWorkflowStaffCommandHandler(
                IWorkflowRepository repository, 
                UserManager<cor_useraccount> userManager, 
                IHttpContextAccessor httpContext,
                IAdminRepository adminRepository,
                ICompanyRepository comprepository)
            {
                _repo = repository;
                _admin = adminRepository;
                _userManger = userManager;
                _httpContextAccessor = httpContext;
                _comprepository = comprepository;
            }

            public async Task<FileUploadRespObj> Handle(UploadWorkflowStaffCommand request, CancellationToken cancellationToken)
            {
                var apiResponse = new FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                { 
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

                    var uploadedRecord = new List<WorkflowlevelStaffObj>();
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
                                if (totalColumns != 5)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Three (5) Column Expected";
                                    return apiResponse;
                                }
                                //First row is considered as the header
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var lkp = new WorkflowlevelStaffObj
                                    {
                                        ExcelLineNumber = i,
                                        WorkflowGroupName = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                        WorkflowLevelName = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty,
                                        StaffName = workSheet.Cells[i, 3]?.Value != null ? workSheet.Cells[i, 3]?.Value.ToString() : string.Empty,
                                        AccessLevel = workSheet.Cells[i, 4]?.Value != null ? workSheet.Cells[i, 4]?.Value.ToString() : string.Empty,
                                        StaffCode = workSheet.Cells[i, 5]?.Value != null ? workSheet.Cells[i, 5]?.Value.ToString() : string.Empty,
                                    };
                                    uploadedRecord.Add(lkp);
                                }
                            }
                        }
                    }

                    var _DomainList = await _repo.GetAllWorkflowLevelStaffAsync();
                    var _StaffList = await _admin.GetAllStaffAsync();
                    var _Group = await _repo.GetAllWorkflowGroupAsync();
                    var _Level = await _repo.GetAllWorkflowLevelAsync();
                    var _CompStructure = await _comprepository.GetAllCompanyStructureDefinitionAsync();

                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var row in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(row.StaffCode))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Staff Code can not be empty detected on line {row.ExcelLineNumber}";
                                return apiResponse;
                            }
                            if (string.IsNullOrEmpty(row.WorkflowGroupName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Workflow group Name can not be empty detected on line {row.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                var group = _Group.FirstOrDefault(a => a.WorkflowGroupName.Trim().ToLower() == row.WorkflowGroupName.Trim().ToLower());
                                if (group == null)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Unidentified Workflow group detected on line {row.ExcelLineNumber}";
                                    return apiResponse;
                                }
                                row.WorkflowGroupId = group.WorkflowGroupId;
                            }
                            if (string.IsNullOrEmpty(row.WorkflowLevelName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Workflow Level Name can not be empty detected on line {row.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                var level = _Level.FirstOrDefault(a => a.WorkflowLevelName.Trim().ToLower() == row.WorkflowLevelName.Trim().ToLower());
                                if (level == null)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Unidentified Workflow Level detected on line {row.ExcelLineNumber}";
                                    return apiResponse;
                                }
                                row.WorkflowLevelId = level.WorkflowLevelId;
                            }
                            if (string.IsNullOrEmpty(row.StaffName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Staff Name can not be empty detected on line {row.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                var firstname = row.StaffName.Split(" ")[0];
                                var lastname = row.StaffName.Split(" ")[1];
                                var level = _StaffList.FirstOrDefault(a => a.FirstName.Trim().ToLower() == firstname.Trim().ToLower() 
                                && a.LastName.Trim().ToLower() == lastname.Trim().ToLower() && a.StaffCode.Trim().ToLower() == row.StaffCode.Trim().ToLower());
                                if (level == null)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Unidentified Staff Detected on line {row.ExcelLineNumber}";
                                    return apiResponse;
                                }
                                row.StaffId = level.StaffId;
                                row.StaffCode = row.StaffCode;
                            }

                            
                            var currentItem = _DomainList.FirstOrDefault(s => s.WorkflowLevelId == row.WorkflowLevelId 
                            && s.Deleted == false && s.StaffId == row.StaffId 
                            && s.WorkflowGroupId == row.WorkflowGroupId);

                            if (currentItem != null)
                            {
                                currentItem.WorkflowGroupId = row.WorkflowGroupId;
                                currentItem.WorkflowLevelId = row.WorkflowLevelId;
                                currentItem.StaffId = row.StaffId; 
                                await _repo.AddUpdateWorkflowLevelStaffAsync(currentItem);
                            }
                            else
                            {
                                var newItem = new cor_workflowlevelstaff();
                                newItem.WorkflowGroupId = row.WorkflowGroupId;
                                newItem.WorkflowLevelId = row.WorkflowLevelId;
                                newItem.StaffId = row.StaffId;
                                await _repo.AddUpdateWorkflowLevelStaffAsync(newItem);
                            }
                        }
                    }
                    apiResponse.Status.IsSuccessful = true;
                    apiResponse.Status.Message.FriendlyMessage = "Successful";
                    return apiResponse;
                }
                catch (Exception ex)
                {
                    apiResponse.Status.Message.FriendlyMessage = ex?.Message;
                    return apiResponse;
                }
            }
        }
    }
   
}
