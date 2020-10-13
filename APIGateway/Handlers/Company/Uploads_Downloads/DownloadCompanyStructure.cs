using APIGateway.Contracts.Response.Common;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.Repository.Interface;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common.Uploads_Downloads
{
    public class DownloadCompanyStructureQuery : IRequest<byte[]>
    {
        public class DownloadCompanyStructureQueryHandler : IRequestHandler<DownloadCompanyStructureQuery, byte[]>
        {
            private readonly IWorkflowRepository _repo;
            private readonly IAdminRepository _admin;
            private readonly ICompanyRepository _company;
            private readonly DataContext _dataContext;
            public DownloadCompanyStructureQueryHandler
                (IWorkflowRepository workflowRepository,
                IAdminRepository admin,
                ICompanyRepository company,
                DataContext dataContext)
            {
                _admin = admin;
                _company = company;
                _dataContext = dataContext;
                _repo = workflowRepository;
            } 
            public async Task<byte[]> Handle(DownloadCompanyStructureQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _company.GetAllCompanyStructureAsync();
                    var _StaffList = await _admin.GetAllStaffAsync(); 
                    var _struct = await _company.GetAllCompanyStructureDefinitionAsync();
                    var _country = await _dataContext.cor_country.Where(s => s.Deleted == false).ToListAsync();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("Name");
                    dt.Columns.Add("Structure Type"); 
                    dt.Columns.Add("Country"); 
                    dt.Columns.Add("Pareant Company");
                    dt.Columns.Add("Head Staff");
                    dt.Columns.Add("Staff Code");

                    var _ContractList = _DomainList.Select(a => new CompanyStructureObj
                    {   
                        Name = a.Name,
                        StructureTypeName = _struct.FirstOrDefault(w => w.StructureDefinitionId == a.StructureTypeId)?.Definition,
                        HeadStaffName = $"{_StaffList.FirstOrDefault(s => s.StaffId ==  a.HeadStaffId)?.FirstName} {_StaffList.FirstOrDefault(s => s.StaffId ==  a.HeadStaffId)?.LastName}",
                        CountryName = _country.FirstOrDefault(s => s.CountryId == a.CountryId)?.CountryName,
                        ParentCompanyName = a?.Parent,
                        StaffCode = _StaffList.FirstOrDefault(s => s.StaffId == a.HeadStaffId)?.StaffCode
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["Name"] = itemRow.Name;
                            row["Structure Type"] = itemRow.StructureTypeName;
                            row["Country"] = itemRow.CountryName;
                            row["Pareant Company"] = itemRow.ParentCompanyName;
                            row["Head Staff"] = itemRow.HeadStaffName;
                            row["Staff Code"] = itemRow.StaffCode;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CompanyStructures");
                                ws.DefaultColWidth = 20;
                                ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                                File = pck.GetAsByteArray();
                            }
                        }
                    }
                    return File;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
