using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Response.CompanySetup;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GetExchangeRateQuery : IRequest<CompanyExchangeRateRespObj>
    {
        public int Company { get; set; }
        public int CurrencyId { get; set; }
        public class GetExchangeRateQueryhandler : IRequestHandler<GetExchangeRateQuery, CompanyExchangeRateRespObj>
        {
            private readonly ICompanyRepository _repo;
            private readonly ICommonRepository _comRepo;
            public GetExchangeRateQueryhandler(ICompanyRepository companyRepository, ICommonRepository commonRepository)
            {
                _repo = companyRepository;
                _comRepo = commonRepository;
            }
            public async Task<CompanyExchangeRateRespObj> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken)
            {
                var apiResponse = new CompanyExchangeRateRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                var _Company = await _repo.GetAllCompanyStructureAsync();
                var _Currency = await _comRepo.GetAllCurrencyAsync();
                var _CurrencyRate = await _comRepo.GetAllCurrencyRateAsync();

                var baseCompany = _Company.FirstOrDefault(x => x.CompanyStructureId == request.Company);
                if (baseCompany == null)
                {
                    apiResponse.Status.Message.FriendlyMessage = "CompanyId doesn't exist for this transaction";
                    return apiResponse;
                }
                var currency = _Currency.FirstOrDefault(x => x.CurrencyId == request.CurrencyId);
                 
                if (baseCompany.CurrencyId > 0)
                {
                    var baseCurrency = _Currency.FirstOrDefault(x => x.CurrencyId == baseCompany.CurrencyId);
                    if (currency.CurrencyCode == baseCurrency.CurrencyCode)
                    {
                        apiResponse.BaseCurrencyId = baseCurrency.CurrencyId;
                        apiResponse.CurrencyId = request.CurrencyId;
                        apiResponse.BuyingRate = 1;
                        apiResponse.SellingRate = 1;
                        apiResponse.Date = DateTime.Now;
                        apiResponse.IsBaseCurrency = true;
                        apiResponse.Status.IsSuccessful = true;
                        return apiResponse;
                    }
                    else
                    {
                        var rateInfo = (from x in _CurrencyRate
                                        where x.cor_currency.CurrencyCode == currency.CurrencyCode
                                        //where x.CurrencyId == currencyId //&& x.Date == date.Date
                                        orderby x.CreatedOn descending
                                        select x).FirstOrDefault();
                        if (rateInfo == null)
                        {
                            apiResponse.Status.Message.FriendlyMessage = $"Exchange rate for {rateInfo.CurrencyCode} is not defined. Define the exchange rate and try again";
                            return apiResponse;
                        }

                       // apiResponse.BaseCurrencyId = baseCurrency.CurrencyId;
                        apiResponse.CurrencyId = request.CurrencyId;
                        apiResponse.BuyingRate = 1;
                        apiResponse.SellingRate = 1;
                        apiResponse.Date = DateTime.Now;
                        apiResponse.IsBaseCurrency = true;
                        apiResponse.Status.IsSuccessful = true;
                        return apiResponse;
                    }
                }
                else
                {
                    apiResponse.Status.Message.FriendlyMessage = "CompanyId doesn't exist for this transaction";
                    apiResponse.Status.IsSuccessful = true;
                    return apiResponse;
                }
            }
        }
    } 
}
