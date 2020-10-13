using APIGateway.Contracts.Response.Common;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Common;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.Repository.Interface;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using MediatR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common.Uploads_Downloads
{
    public class DownloadWorkflowStaffQuery : IRequest<byte[]>
    {
        public class DownloadWorkflowStaffQueryHandler : IRequestHandler<DownloadWorkflowStaffQuery, byte[]>
        {
            private readonly IWorkflowRepository _repo;
            private readonly IAdminRepository _admin;
            private readonly ICompanyRepository _company;
            public DownloadWorkflowStaffQueryHandler
                (IWorkflowRepository workflowRepository,
                IAdminRepository admin,
                ICompanyRepository company)
            {
                _admin = admin;
                _company = company;
                _repo = workflowRepository;
            }
            private async Task<int> GetStaffAccessLevel(int staffId)
            {
                var staffList = await _admin.GetAllStaffAsync();
                var level = staffList.FirstOrDefault(d => d.StaffId == staffId)?.AccessLevel ?? 0;
                return level;
            }
            public async Task<byte[]> Handle(DownloadWorkflowStaffQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _repo.GetAllWorkflowLevelStaffAsync();
                    var _StaffList = await _admin.GetAllStaffAsync();
                    var _Level = await _repo.GetAllWorkflowLevelAsync();
                    var _Group = await _repo.GetAllWorkflowGroupAsync();
                    var _CompStructure = await _company.GetAllCompanyStructureDefinitionAsync();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("Workflow Group");
                    dt.Columns.Add("Workflow Level");
                    dt.Columns.Add("Staff Name");
                    dt.Columns.Add("Access Level");
                    dt.Columns.Add("Staff Code");

                    var _ContractList = _DomainList.Select(a => new WorkflowlevelStaffObj
                    {
                        WorkflowGroupName = _Group.FirstOrDefault(w => w.WorkflowGroupId == a.WorkflowGroupId)?.WorkflowGroupName,
                        WorkflowLevelName = _Level.FirstOrDefault(w => w.WorkflowLevelId == a.WorkflowLevelId)?.WorkflowLevelName,
                        StaffName = $"{_StaffList.FirstOrDefault(s => s.StaffId ==  a.StaffId)?.FirstName} {_StaffList.FirstOrDefault(s => s.StaffId ==  a.StaffId)?.LastName}",
                        StaffCode = _StaffList.FirstOrDefault(s => s.StaffId == a.StaffId)?.StaffCode,
                        AccessLevel = _CompStructure.FirstOrDefault(o => o.StructureDefinitionId == GetStaffAccessLevel(a.StaffId)?.Result)?.Definition
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["Workflow Group"] = itemRow.WorkflowGroupName;
                            row["Workflow Level"] = itemRow.WorkflowLevelName;
                            row["Staff Name"] = itemRow.StaffName;
                            row["Access Level"] = itemRow.AccessLevel;
                            row["Staff Code"] = itemRow.StaffCode;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("WorkflowStaff");
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
