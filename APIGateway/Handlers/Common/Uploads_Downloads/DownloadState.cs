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
    public class DownloadStateQuery : IRequest<byte[]>
    {
        public class DownloadStateQueryHandler : IRequestHandler<DownloadStateQuery, byte[]>
        {
            private readonly ICommonRepository _repo;
            public DownloadStateQueryHandler
                (ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<byte[]> Handle(DownloadStateQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _repo.GetAllStateAsync();
                    var _CountryList = await _repo.GetAllCountry();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("State Name");
                    dt.Columns.Add("State Code");
                    dt.Columns.Add("Country Code");

                    var _ContractList = _DomainList.Select(a => new CommonLookupsObj
                    {
                        LookupName = a.StateName,
                        Code = a.StateCode,
                        ParentName = _CountryList.FirstOrDefault(s => s.CountryId == a.CountryId)?.CountryName
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["State Name"] = itemRow.LookupName;
                            row["State Code"] = itemRow.Code;
                            row["Country Code"] = itemRow.ParentName;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("States");
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
