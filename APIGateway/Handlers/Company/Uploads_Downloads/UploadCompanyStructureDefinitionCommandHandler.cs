using GODP.APIsContinuation.DomainObjects.Ifrs; 
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.Response.CompanySetup;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Company
{
    public class UploadCompanyStructureDefinitionCommand : IRequest<FileUploadRespObj>
    {
        public class UploadCompanyStructureDefinitionCommandHandler : IRequestHandler<UploadCompanyStructureDefinitionCommand, FileUploadRespObj>
        {
            private readonly ICompanyRepository _repo; 
            private readonly IHttpContextAccessor _accessor;
            public UploadCompanyStructureDefinitionCommandHandler(
                ICompanyRepository companyRepository,
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = companyRepository;
                _accessor = httpContextAccessor;
            }
            public async Task<FileUploadRespObj> Handle(UploadCompanyStructureDefinitionCommand request, CancellationToken cancellationToken)
            {
                var response = new FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                List<CompanyStructureDefinitionObj> uploadedRecord = new List<CompanyStructureDefinitionObj>();

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
                    if (byteList.Count() > 0)
                    {
                        foreach (var item in byteList)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (MemoryStream stream = new MemoryStream(item))
                            using (ExcelPackage excelPackage = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                                int totalRows = workSheet.Dimension.Rows;
                                int totalColumns = workSheet.Dimension.Columns;
                                if (totalColumns != 3)
                                {
                                    response.Status.Message.FriendlyMessage = $"Three (3) Column Expected";
                                    return response;
                                }
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    uploadedRecord.Add(new CompanyStructureDefinitionObj
                                    {
                                        ExcelLineNumber = i,
                                        Definition = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                        Description = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                                        StructureLevel = workSheet.Cells[i, 3].Value != null ? Convert.ToInt32(workSheet.Cells[i, 3].Value.ToString()) : 0, 
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Status.Message.FriendlyMessage = $" {ex?.Message}";
                    return response;
                }
                List<cor_companystructuredefinition> StructureDefinitions = new List<cor_companystructuredefinition>();
                 
                var _CompanyStructureDefinition = await _repo.GetAllCompanyStructureDefinitionAsync(); 

                try
                {
                    if (uploadedRecord.Count > 0)
                    {
                        foreach (var item in uploadedRecord)
                        {
                            if (string.IsNullOrEmpty(item.Definition))
                            {
                                response.Status.Message.FriendlyMessage = $"Definition cannot be empty detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                            if (string.IsNullOrEmpty(item.Description))
                            {
                                response.Status.Message.FriendlyMessage = $"Description cannot be empty detected on line {item.ExcelLineNumber}";
                                return response;
                            }  
                            if (item.StructureLevel < 1)
                            {
                                response.Status.Message.FriendlyMessage = $"Structure level cannot be empty detected on line {item.ExcelLineNumber}";
                                return response;
                            } 
                            var compStructFrmRepo = _CompanyStructureDefinition.FirstOrDefault(x => x.Definition.ToLower() == item.Definition.ToLower());
                            if (compStructFrmRepo != null)
                            { 
                                compStructFrmRepo.Definition = item.Definition;
                                compStructFrmRepo.Description = item.Description;
                                compStructFrmRepo.StructureLevel = item.StructureLevel; 
                                await _repo.AddUpdateCompanyStructureDefinitionAsync(compStructFrmRepo);
                            }
                            else
                            {
                                var newStructFrmRepo = new cor_companystructuredefinition();
                                newStructFrmRepo.Definition = item.Definition;
                                newStructFrmRepo.Description = item.Description;
                                newStructFrmRepo.StructureLevel = item.StructureLevel;
                                await _repo.AddUpdateCompanyStructureDefinitionAsync(newStructFrmRepo);
                            }
                        }
                    }
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = "Successful";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = ex?.Message;
                    return response;
                }

            } 
        }
    }
    
}
