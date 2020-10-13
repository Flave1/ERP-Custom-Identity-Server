using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Currency;
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
    public class UploadCurrencyRateCommand : IRequest<FileUploadRespObj>
    {
        public byte[] File { get; set; }
        public class UploadCurrencyRateCommandHandler : IRequestHandler<UploadCurrencyRateCommand, FileUploadRespObj>
        {
            private readonly ICommonRepository _repo;
            private readonly UserManager<cor_useraccount> _userManager;
            private readonly IHttpContextAccessor _accessor;
            public UploadCurrencyRateCommandHandler(
                ICommonRepository commonRepository,
                UserManager<cor_useraccount> userManager,
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = commonRepository;
                _accessor = httpContextAccessor;
                _userManager = userManager;
            }
            public async Task<FileUploadRespObj> Handle(UploadCurrencyRateCommand request, CancellationToken cancellationToken)
            {
                var apiResponse = new FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                { 
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

                    var _currency = await _repo.GetAllCurrencyAsync();

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
                                if (totalColumns != 4)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Four (4) Columns Expected";
                                    return apiResponse;
                                }
                                //First row is considered as the header
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    var lkp = new CommonLookupsObj
                                    {
                                        ExcelLineNumber = i,
                                        Code = workSheet.Cells[i, 1]?.Value != null ? workSheet.Cells[i, 1]?.Value.ToString() : string.Empty,
                                        Description = workSheet.Cells[i, 2]?.Value != null ? workSheet.Cells[i, 2]?.Value.ToString() : string.Empty,
                                        SkillDescription = workSheet.Cells[i, 3]?.Value != null ? workSheet.Cells[i, 3]?.Value.ToString() : string.Empty,
                                        Skills = workSheet.Cells[i, 4]?.Value != null ? workSheet.Cells[i, 4]?.Value.ToString() : string.Empty,
                                    };
                                    uploadedRecord.Add(lkp);
                                }
                            }
                        } 
                    }
                     
                    var _CurrencyRateList = await _repo.GetAllCurrencyRateAsync();
                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var item in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(item.Code))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"SellingRate Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                item.SellingRate = Convert.ToDouble(item.Code);
                            }
                            if (string.IsNullOrEmpty(item.Description))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"BuyingRate Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                item.BuyingRate = Convert.ToDouble(item.Description);
                            }
                            if (string.IsNullOrEmpty(item.SkillDescription))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Currency Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                var currency = _currency.FirstOrDefault(s => s.CurrencyName.ToLower().Trim() == item.SkillDescription.ToLower().ToLower());
                                if(currency == null)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Unable to Identify Currency detected on line {item.ExcelLineNumber}";
                                    return apiResponse;
                                }
                                item.ParentId = currency.CurrencyId;
                            }
                            if (string.IsNullOrEmpty(item.Skills))
                            {
                                apiResponse.Status.Message.FriendlyMessage = $"Job Title Cannot be empty detected on line {item.ExcelLineNumber}";
                                return apiResponse;
                            }
                            else
                            {
                                if(item.Skills.Split('/').Length != 3)
                                {
                                    apiResponse.Status.Message.FriendlyMessage = $"Invalid Date detected on line {item.ExcelLineNumber}";
                                    return apiResponse;
                                }
                                item.Date = Convert.ToDateTime(item.Skills);
                            }
                            var currencyRate = _CurrencyRateList.FirstOrDefault(c => c.CurrencyId == item.ParentId && c.Date == item.Date && c.Deleted == false);

                            if (currencyRate == null) 
                            { 
                                var CurrencyRate = new cor_currencyrate();
                                CurrencyRate.BuyingRate = item.BuyingRate;
                                CurrencyRate.SellingRate = item.SellingRate;
                                CurrencyRate.CurrencyId = item.ParentId;
                                CurrencyRate.Date = item.Date;
                                await _repo.AddUpdateCurrencyRateAsync(CurrencyRate);
                            }
                            else
                            {
                                currencyRate.BuyingRate = item.BuyingRate;
                                currencyRate.SellingRate = item.SellingRate;
                                currencyRate.CurrencyId = item.ParentId;
                                currencyRate.Date = item.Date;
                                await _repo.AddUpdateCurrencyRateAsync(currencyRate);
                            }
                        }
                    }
                    apiResponse.Status.IsSuccessful = true;
                    apiResponse.Status.Message.FriendlyMessage = "Successful";
                    return apiResponse;
                }
                catch (Exception ex)
                {
                    apiResponse.Status.Message.FriendlyMessage = ex.Message;
                    return apiResponse;
                }
            }
        }
    }
    
}
