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
    public class DownloadCompanyDefinitionQuery : IRequest<byte[]>
    {
        public class DownloadCompanyDefinitionQueryHandler : IRequestHandler<DownloadCompanyDefinitionQuery, byte[]>
        {
            private readonly IWorkflowRepository _repo;
            private readonly IAdminRepository _admin;
            private readonly ICompanyRepository _company;
            private readonly DataContext _dataContext;
            public DownloadCompanyDefinitionQueryHandler
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
            public async Task<byte[]> Handle(DownloadCompanyDefinitionQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _company.GetAllCompanyStructureDefinitionAsync(); 

                    DataTable dt = new DataTable();

                    dt.Columns.Add("Name");
                    dt.Columns.Add("Definition"); 
                    dt.Columns.Add("Level");  

                    var _ContractList = _DomainList.Select(a => new CompanyStructureDefinitionObj
                    {   
                        Definition = a.Definition,
                        Description = a.Description,
                        StructureLevel = a.StructureLevel
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["Name"] = itemRow.Definition;
                            row["Definition"] = itemRow.Description;
                            row["Level"] = itemRow.StructureLevel;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CompanyDefinitions");
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
