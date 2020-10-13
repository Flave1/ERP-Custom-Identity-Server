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
    public class DownloadCurrencyRateQuery : IRequest<byte[]>
    {
        public class DownloadCurrencyRateQueryHandler : IRequestHandler<DownloadCurrencyRateQuery, byte[]>
        {
            private readonly ICommonRepository _repo;
            public DownloadCurrencyRateQueryHandler
                (ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<byte[]> Handle(DownloadCurrencyRateQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _repo.GetAllCurrencyRateAsync();
                    var _Currency = await _repo.GetAllCurrencyAsync();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Buying Rate");
                    dt.Columns.Add("Selling Rate");
                    dt.Columns.Add("Currency");
                    dt.Columns.Add("Date");

                    var _ContractList = _DomainList.Select(a => new CommonLookupsObj
                    {
                        BuyingRate = (int)a.BuyingRate,
                        SellingRate = (int)a.SellingRate,
                        ParentName = _Currency.FirstOrDefault(s => s.CurrencyId == a.CurrencyId)?.CurrencyName,
                        Date = a.Date
                    }).ToList();

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow();
                            row["Buying Rate"] = itemRow.BuyingRate;
                            row["Selling Rate"] = itemRow.SellingRate;
                            row["Currency"] = itemRow.ParentName;
                            row["Date"] = itemRow.Date;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CurrencyRates");
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
