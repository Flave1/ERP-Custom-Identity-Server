using APIGateway.Contracts.Response.Common;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.Repository.Interface;
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
    public class DownloadJobTitleQuery : IRequest<byte[]>
    {
        public class DownloadJobTitleQueryHandler : IRequestHandler<DownloadJobTitleQuery, byte[]>
        {
            private readonly ICommonRepository _repo;
            public DownloadJobTitleQueryHandler
                (ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<byte[]> Handle(DownloadJobTitleQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _repo.GetAllJobTitleAsync(); 

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Name");
                    dt.Columns.Add("JobDescription");
                    dt.Columns.Add("Skills");
                    dt.Columns.Add("SkillDescription");

                    var _ContractList = _DomainList.Select(a => new CommonLookupsObj
                    {
                        LookupName = a.Name,
                        Description = a.JobDescription,
                        Skills = a.Skills,
                        SkillDescription = a.SkillDescription
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["Name"] = itemRow.LookupName;
                            row["JobDescription"] = itemRow.Description;
                            row["Skills"] = itemRow.Skills;
                            row["SkillDescription"] = itemRow.SkillDescription; 
                            dt.Rows.Add(row);
                        } 
                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("JobTitle");
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
