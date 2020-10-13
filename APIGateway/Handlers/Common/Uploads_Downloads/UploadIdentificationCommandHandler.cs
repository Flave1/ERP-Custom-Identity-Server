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
    public class UploadIdentificationCommand : IRequest<FileUploadRespObj>
    {
        public byte[] File { get; set; }
        public class UploadIdentificationCommandHandler : IRequestHandler<UploadIdentificationCommand, FileUploadRespObj>
        {
            private readonly ICommonRepository _repo;
            private readonly UserManager<cor_useraccount> _userManager;
            private readonly IHttpContextAccessor _accessor;
            public UploadIdentificationCommandHandler(
                ICommonRepository commonRepository,
                UserManager<cor_useraccount> userManager,
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = commonRepository;
                _accessor = httpContextAccessor;
                _userManager = userManager;
            }
            public async Task<FileUploadRespObj> Handle(UploadIdentificationCommand request, CancellationToken cancellationToken)
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
                     
                    var _Identification = await _repo.GetAllIdentificationAsync();     

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
                                    apiResponse.Status.Message.FriendlyMessage = $"One (1) Columns Expected";
                                    return apiResponse;
                                }
                                //First row is considered as the header
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var lkp = new CommonLookupsObj
                                    { 
                                        ExcelLineNumber = i,
                                        LookupName = workSheet.Cells[i, 1].Value.ToString()
                                    };
                                    uploadedRecord.Add(lkp);
                                }
                            }
                        }

                    }


                    var _IdentificationList = await _repo.GetAllIdentificationAsync();
                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var item in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(item.LookupName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Document type name can not be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            var currentIdentification = _IdentificationList.FirstOrDefault(c => c.IdentificationName.ToLower().Trim() == item.LookupName.ToLower().Trim() && c.Deleted == false);

                            if (currentIdentification == null)
                            {
                                var Identification = new cor_identification(); 
                                Identification.IdentificationName = item.LookupName; 
                                await _repo.AddUpdateIdentificationAsync(Identification);
                            }
                            else
                            {
                                currentIdentification.IdentificationName = item.LookupName;
                                await _repo.AddUpdateIdentificationAsync(currentIdentification);
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
