using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Others;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.GeneralExtension;
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

namespace APIGateway.Handlers.Common.Uploads_Downloads
{
    public class UploadJobTitleCommand : IRequest<FileUploadRespObj>
    {
        public byte[] File { get; set; }
        public class UploadJobTitleCommandHandler : IRequestHandler<UploadJobTitleCommand, FileUploadRespObj>
        {
            private readonly ICommonRepository _repo;
            private readonly UserManager<cor_useraccount> _userManager;
            private readonly IHttpContextAccessor _accessor;
            public UploadJobTitleCommandHandler(
                ICommonRepository commonRepository,
                UserManager<cor_useraccount> userManager,
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = commonRepository;
                _accessor = httpContextAccessor;
                _userManager = userManager;
            }
            public async Task<FileUploadRespObj> Handle(UploadJobTitleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var apiResponse = new FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

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
                    var userId = _accessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                    var user = await _userManager.FindByIdAsync(userId);

                    var _JobTitle = await _repo.GetAllJobTitleAsync();
                    var _State = await _repo.GetAllStateAsync();

                    var uploadedRecord = new List<CommonLookupsObj>();
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
                                //First row is considered as the header
                                int totalColumns = workSheet.Dimension.Columns;
                                if (totalColumns != 4)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Four (4) Columns Expected";
                                    return apiResponse;
                                }
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var lkp = new CommonLookupsObj
                                    {
                                        ExcelLineNumber = i,
                                        LookupName = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                        Description = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty,
                                        Skills = workSheet.Cells[i, 3]?.Value != null ? workSheet.Cells[i, 3]?.Value.ToString() : string.Empty,
                                        SkillDescription = workSheet.Cells[i, 4]?.Value != null ? workSheet.Cells[i, 4]?.Value.ToString() : string.Empty,
                                    };
                                    uploadedRecord.Add(lkp);
                                }
                            }
                        }

                    }


                    var _JobTitleList = await _repo.GetAllJobTitleAsync();
                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var item in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(item.LookupName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Job Title Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            if (string.IsNullOrEmpty(item.Description))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Description Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            if (string.IsNullOrEmpty(item.Skills))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Skills Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            if (string.IsNullOrEmpty(item.SkillDescription))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Skill Description Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            var currentJobTitle = _JobTitleList.FirstOrDefault(c => c.Name.ToLower() == item.LookupName.ToLower() && c.Deleted == false);

                            if (currentJobTitle == null)
                            { 
                                var JobTitle = new cor_jobtitles();
                                JobTitle.Name = item.LookupName;
                                JobTitle.JobDescription = item.Description;
                                JobTitle.Skills = item.Skills;
                                JobTitle.SkillDescription = item.SkillDescription;
                                await _repo.AddUpdateJobTitleAsync(JobTitle);
                            }
                            else
                            {
                                currentJobTitle.Name = item.LookupName;
                                currentJobTitle.JobDescription = item.Description;
                                currentJobTitle.Skills = item.Skills;
                                currentJobTitle.SkillDescription = item.SkillDescription;
                                await _repo.AddUpdateJobTitleAsync(currentJobTitle);
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
    
}
