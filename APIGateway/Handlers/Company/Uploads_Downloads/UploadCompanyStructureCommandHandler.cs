using APIGateway.Contracts.Commands.Company;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.Repository.Interface.Admin;
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
    public class UploadCompanyStructureCommandHandler : IRequestHandler<UploadCompanyStructureCommand, FileUploadRespObj>
    {
        private readonly ICompanyRepository _repo;
        private readonly ICommonRepository _comRepo;
        private readonly IAdminRepository _adminRepo;
        private readonly IHttpContextAccessor _accessor;
        public UploadCompanyStructureCommandHandler(
            ICompanyRepository companyRepository, 
            ICommonRepository commonRepository,
            IAdminRepository adminRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _adminRepo = adminRepository;
            _comRepo = commonRepository;
            _repo = companyRepository;
            _accessor = httpContextAccessor;
        }
        public async Task<FileUploadRespObj> Handle(UploadCompanyStructureCommand request, CancellationToken cancellationToken)
        {
            var response = new  FileUploadRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                
            List <CompanyStructureObj> uploadedRecord = new List<CompanyStructureObj>();

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
                if(byteList.Count() > 0)
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
                            if (totalColumns != 6)
                            {
                                response.Status.Message.FriendlyMessage = $"Six (6) Column Expected";
                                return response;
                            }
                            for (int i = 2; i <= totalRows; i++)
                            {
                                uploadedRecord.Add(new CompanyStructureObj
                                {
                                    ExcelLineNumber = i,
                                    Name = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    StructureTypeName = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                                    CountryName = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                                    ParentCompanyName = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,
                                    HeadStaffName = workSheet.Cells[i, 5].Value != null ? workSheet.Cells[i, 5].Value.ToString() : null,
                                    StaffCode = workSheet.Cells[i, 6].Value != null ? workSheet.Cells[i, 6].Value.ToString() : null,
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
            List<cor_companystructure> structures = new List<cor_companystructure>();

            var _CompanyStructureDefinition = await _repo.GetAllCompanyStructureDefinitionAsync();
            var _CompanyStructure = await _repo.GetAllCompanyStructureAsync();
            var _Common = await _comRepo.GetAllCountryAsync(); 
            var _StaffList = await _adminRepo.GetAllStaffAsync();

            try
            {
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        if (string.IsNullOrEmpty(item.Name))
                        {
                            response.Status.Message.FriendlyMessage = $"Company Name cannot be empty detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        if (string.IsNullOrEmpty(item.StaffCode))
                        {
                            response.Status.Message.FriendlyMessage = $"Staff Code cannot be empty detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        if (item.StructureTypeName != null)
                        {
                            var data = _CompanyStructureDefinition.FirstOrDefault(x => x.Definition.ToLower().Contains(item.StructureTypeName.ToLower()));
                            if (data == null)
                            {
                                response.Status.Message.FriendlyMessage = $"UnIdentified Company Structure type detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.StructureTypeId = data.StructureDefinitionId;
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"Structure Type name cannot be empty detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        if (item.CountryName != null)
                        {
                            var data = _Common.FirstOrDefault(x => x.CountryName.ToLower().Contains(item.CountryName.ToLower()));
                            if (data == null)
                            {
                                response.Status.Message.FriendlyMessage = $"UnIdentified  Country name detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.CountryId = data.CountryId;
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = $"Country name cannot be empty detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        if (string.IsNullOrEmpty(item.HeadStaffName))
                        {
                            response.Status.Message.FriendlyMessage = $"Staff Name can not be empty detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var firstname = item.HeadStaffName.Split(" ")[0];
                            var lastname = item.HeadStaffName.Split(" ")[1];
                            var staff = _StaffList.FirstOrDefault(a => a.FirstName.Trim().ToLower() == firstname.Trim().ToLower()
                            && a.LastName.Trim().ToLower() == lastname.Trim().ToLower() && a.StaffCode.Trim().ToLower() == item.StaffCode.Trim().ToLower());
                            if (staff == null)
                            {
                                response.Status.Message.FriendlyMessage = $"Unidentified Head Staff Detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.HeadStaffId = staff.StaffId;
                        }
                        if (string.IsNullOrEmpty(item.ParentCompanyName) && item.ExcelLineNumber > 2)
                        {
                            response.Status.Message.FriendlyMessage = $"Parent Company Name can not be empty detected on line {item.ExcelLineNumber}";
                            return response;
                        }
                        else
                        {
                            var title = _CompanyStructure.FirstOrDefault(x => x.Name.ToLower().Contains(item.ParentCompanyName.ToLower()));
                            if (title == null)
                            {
                                response.Status.Message.FriendlyMessage = $"Parent Company name detected on line {item.ExcelLineNumber}";
                                return response;
                            }
                            item.ParentCompanyID = title.CompanyStructureId;
                        }
                        var compStructFrmRepo = _CompanyStructure.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower());
                        if (compStructFrmRepo != null)
                        {
                            compStructFrmRepo.CompanyStructureId = compStructFrmRepo.CompanyStructureId;
                            compStructFrmRepo.Name = item.Name;
                            compStructFrmRepo.StructureTypeId = item.StructureTypeId;
                            compStructFrmRepo.HeadStaffId = item.HeadStaffId;
                            compStructFrmRepo.CountryId = item.CountryId;
                            compStructFrmRepo.ParentCompanyID = item.ParentCompanyID;
                            await _repo.AddUpdateCompanyStructureAsync(compStructFrmRepo);
                        }
                        else
                        {
                            var newStructFrmRepo = new cor_companystructure();
                            newStructFrmRepo.Name = item.Name;
                            newStructFrmRepo.StructureTypeId = item.StructureTypeId;
                            newStructFrmRepo.HeadStaffId = item.HeadStaffId;
                            newStructFrmRepo.CountryId = item.CountryId;
                            newStructFrmRepo.ParentCompanyID = item.ParentCompanyID;
                            await _repo.AddUpdateCompanyStructureAsync(newStructFrmRepo);
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
