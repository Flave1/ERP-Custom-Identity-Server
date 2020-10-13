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
    public class DownloadCityQuery : IRequest<byte[]>
    {
        public class DownloadCityQueryHandler : IRequestHandler<DownloadCityQuery, byte[]>
        {
            private readonly ICommonRepository _repo;
            public DownloadCityQueryHandler
                (ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<byte[]> Handle(DownloadCityQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _repo.GetAllCityAsync();
                    var _StateList = await _repo.GetAllStateAsync();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("City Code");
                    dt.Columns.Add("City Name");
                    dt.Columns.Add("State Code");

                    var _ContractList = _DomainList.Select(a => new CommonLookupsObj
                    {
                        LookupName = a.CityName,
                        Code = a.CityCode,
                        ParentName = _StateList.FirstOrDefault(s => s.StateId == a.StateId)?.StateName
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["City Code"] = itemRow.Code;
                            row["City Name"] = itemRow.LookupName;
                            row["State Code"] = itemRow.ParentName;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Cities");
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
