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
    public class DownloadCurrencyQuery : IRequest<byte[]>
    {
        public class DownloadCurrencyQueryHandler : IRequestHandler<DownloadCurrencyQuery, byte[]>
        {
            private readonly ICommonRepository _repo;
            public DownloadCurrencyQueryHandler
                (ICommonRepository commonRepository)
            {
                _repo = commonRepository;
            }
            public async Task<byte[]> Handle(DownloadCurrencyQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    byte[] File = new byte[0];
                    var _DomainList = await _repo.GetAllCurrencyAsync();
                    var _StateList = await _repo.GetAllStateAsync();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Currency Code");
                    dt.Columns.Add("Currency Name");
                    dt.Columns.Add("Base Currency");
                    dt.Columns.Add("In Use");

                    var _ContractList = new List<CommonLookupsObj>();
                    foreach (var item in _DomainList)
                    {
                        var res = new CommonLookupsObj();
                        res.Code = item.CurrencyCode != null ? item?.CurrencyCode : " ";
                        res.LookupName = item.CurrencyName != null ? item?.CurrencyName : " ";
                        res.BaseCurrency = item.BaseCurrency != null?(bool)item.BaseCurrency : false;
                        res.Active = item.INUSE;
                        _ContractList.Add(res);
                    } 

                    if (_ContractList.Count() > 0)
                    {
                        foreach (var itemRow in _ContractList)
                        {
                            var row = dt.NewRow(); 
                            row["Currency Code"] = itemRow.Code;
                            row["Currency Name"] = itemRow.LookupName;
                            row["Base Currency"] = itemRow.BaseCurrency;
                            row["In Use"] = itemRow.Active;
                            dt.Rows.Add(row);
                        }

                        if (_ContractList != null)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Currencies");
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
