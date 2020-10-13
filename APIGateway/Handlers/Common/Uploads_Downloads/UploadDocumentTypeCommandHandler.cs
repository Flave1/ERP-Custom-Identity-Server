using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.DomainObjects.Credit;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Company;
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
    public class UploadDocumentTypeCommand : IRequest<FileUploadRespObj>
    {
        public byte[] File { get; set; }
        public class UploadDocumentTypeCommandHandler : IRequestHandler<UploadDocumentTypeCommand, FileUploadRespObj>
        {
            private readonly ICommonRepository _repo; 
            private readonly IHttpContextAccessor _accessor;
            public UploadDocumentTypeCommandHandler(
                ICommonRepository commonRepository, 
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = commonRepository;
                _accessor = httpContextAccessor; 
            }
            public async Task<FileUploadRespObj> Handle(UploadDocumentTypeCommand request, CancellationToken cancellationToken)
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
                                int totalColumns = workSheet.Dimension.Columns;
                                if (totalColumns != 1)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"One (1) Column Expected";
                                    return apiResponse;
                                }
                                //First row is considered as the header
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var lkp = new CommonLookupsObj
                                    { 
                                        ExcelLineNumber = i,
                                        LookupName = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                    };
                                    uploadedRecord.Add(lkp);
                                }
                            }
                        } 
                    }


                    var _DocumentTypeList = await _repo.GetAllDocumentTypeAsync();
                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var item in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(item.LookupName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Document type name can not be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            var currentDocumentType = _DocumentTypeList.FirstOrDefault(c => c.Name.ToLower() == item.LookupName.ToLower() && c.Deleted == false);
                             
                            if(currentDocumentType == null)
                            {
                                var DocumentType = new credit_documenttype(); 
                                DocumentType.Name = item.LookupName;
                                await _repo.AddUpdateDocumentTypeAsync(DocumentType);
                            }
                            else
                            {
                                currentDocumentType.Name = item.LookupName;
                                await _repo.AddUpdateDocumentTypeAsync(currentDocumentType);
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
