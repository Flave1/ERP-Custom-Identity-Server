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
    public class UploadCityCommand : IRequest<FileUploadRespObj>
    {
        public byte[] File { get; set; }
        public class UploadCityCommandHandler : IRequestHandler<UploadCityCommand, FileUploadRespObj>
        {
            private readonly ICommonRepository _repo;
            private readonly UserManager<cor_useraccount> _userManager;
            private readonly IHttpContextAccessor _accessor;
            public UploadCityCommandHandler(
                ICommonRepository commonRepository,
                UserManager<cor_useraccount> userManager,
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = commonRepository;
                _accessor = httpContextAccessor;
                _userManager = userManager;
            }
            public async Task<FileUploadRespObj> Handle(UploadCityCommand request, CancellationToken cancellationToken)
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
                                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                                int totalRows = workSheet.Dimension.Rows;
                                int totatlColumns = workSheet.Dimension.Columns;
                                if(totatlColumns != 3)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = "Three (3) Columns Expected";
                                    return apiResponse;
                                }
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var lkp = new CommonLookupsObj
                                    {
                                        ExcelLineNumber = i,
                                        Code = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                        LookupName = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty,
                                        ParentName = workSheet.Cells[i, 3]?.Value != null ? workSheet.Cells[i, 3]?.Value.ToString() : string.Empty, 
                                    };
                                    uploadedRecord.Add(lkp);
                                }
                            }
                        } 
                    }

                    var _CityList = await _repo.GetAllCityAsync();
                    var _State = await _repo.GetAllStateAsync();
                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var item in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(item.Code))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"City Code can not be empty on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            if (string.IsNullOrEmpty(item.LookupName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"City Name can not be empty on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }

                            if (string.IsNullOrEmpty(item.ParentName))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"State Name can not be empty on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                var state = _State.FirstOrDefault(d => d.StateCode.Trim().ToLower() == item.ParentName.Trim().ToLower());
                               if(state == null)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"State name unidentified on line {item.ExcelLineNumber}";
                                    return apiResponse;
                                }
                                item.ParentId = state.StateId;
                            }
                             
                            var currentCity = _CityList.FirstOrDefault(c => c.CityCode.ToLower() == item.Code.ToLower() && c.Deleted == false);

                            if (currentCity == null)
                            { 
                                var City = new cor_city();
                                City.CityName = item.LookupName;
                                City.StateId = item.ParentId;
                                City.CityCode = item.Code; 
                                await _repo.AddUpdateCityAsync(City);
                            }
                            else
                            {
                                currentCity.CityName = item.LookupName;
                                currentCity.StateId = item.ParentId;
                                currentCity.CityCode = item.Code;
                                await _repo.AddUpdateCityAsync(currentCity);
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
