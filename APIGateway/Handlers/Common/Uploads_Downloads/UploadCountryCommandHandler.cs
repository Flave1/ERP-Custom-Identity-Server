using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
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
    public class UploadCountryCommandHandler : IRequestHandler<UploadCountryCommand, FileUploadRespObj>
    {
        private readonly ICommonRepository _repo;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IHttpContextAccessor _accessor;
        public UploadCountryCommandHandler(
            ICommonRepository commonRepository,
            UserManager<cor_useraccount> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = commonRepository;
            _accessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<FileUploadRespObj> Handle(UploadCountryCommand request, CancellationToken cancellationToken)
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

                var _Country = await _repo.GetAllCountryAsync();

                var uploadedRecord = new List<CommonLookupsObj>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                if(byteList.Count() > 0)
                {
                    foreach(var byteItem in byteList)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            //Use first sheet by default
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;
                            int totalColumns = workSheet.Dimension.Columns;
                            if(totalColumns != 2)
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Two (2) Columns Expected";
                                return apiResponse;
                            }
                            //First row is considered as the header
                            for (int i = 2; i <= totalRows; i++)
                            {
                                var lkp = new CommonLookupsObj
                                {
                                    ExcelLineNumber = i,
                                    LookupName = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                    Code = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty,
                                };
                                uploadedRecord.Add(lkp);
                            }
                        }
                    } 
                }

                var _CountryList = await _repo.GetAllCountry();
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        if (string.IsNullOrEmpty(item.Code))
                        {
                            apiResponse.Status.Message.FriendlyMessage = $"Country Code can not be empty on line {item.ExcelLineNumber}";
                            return apiResponse;
                        }
                        if (string.IsNullOrEmpty(item.LookupName))
                        {
                            apiResponse.Status.Message.FriendlyMessage = $"Country Name can not be empty on line {item.ExcelLineNumber}";
                            return apiResponse;
                        }

                        var currentCountry = _CountryList.FirstOrDefault(c => c.CountryCode.ToLower() == item.Code.ToLower() && c.Deleted == false);

                        if (currentCountry == null)
                        {
                            var country = new cor_country
                            {
                                CountryName = item.LookupName,
                                CountryCode = item.Code, 
                            };
                            await _repo.AddUpdateCountryAsync(country);
                        }
                        else
                        { 
                            currentCountry.CountryName = item.LookupName;
                            currentCountry.CountryCode = item.Code; 
                            await _repo.AddUpdateCountryAsync(currentCountry);
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
