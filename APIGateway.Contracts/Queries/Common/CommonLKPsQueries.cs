using APIGateway.Contracts.Response.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Common
{
    public class GetAllBranchesQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCallMemoTypeQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCityQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCountryQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllDepartmentsQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllDirectorTypeQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllDocumentTypeQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllEmployerTypeQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllGenderQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllGLAccountQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllLoanManagementOperationTypeQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllMaritalStatusQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllModulesQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllProductTypeQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllStateQuery : IRequest<CommonLookupRespObj> { }
    public class GetAllTitleQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllJobTitleQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCurrencyQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCurrencyRateQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllIdentificationQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCreditBureauQuery : IRequest<CommonLookupRespObj> { }

    public class GetAllCityByStateQuery : IRequest<CommonLookupRespObj>
    {
        public GetAllCityByStateQuery() { }
        public int StateId { get; set; }
        public GetAllCityByStateQuery(int stateId)
        {
            StateId = stateId;
        }
    }
    public class GetAllStateInCountryQuery : IRequest<CommonLookupRespObj>
    {
        public GetAllStateInCountryQuery() { }
        public int CountryId { get; set; }
        public GetAllStateInCountryQuery(int countryId)
        {
            CountryId = countryId;
        }
    }

    public class GetCountryQuery : IRequest<CommonLookupRespObj> {
        public GetCountryQuery() { }
        public int CountryId { get; set; }
        public GetCountryQuery(int lookUpId)
        {
            CountryId = lookUpId;
        }
    } 
    public class GetBranchQuery : IRequest<CommonLookupRespObj>
    {
        public GetBranchQuery() { }
        public int BranchId { get; set; }
        public GetBranchQuery(int lookUpId)
        {
            BranchId = lookUpId;
        }
    }
    public class GetDocumenttypeQuery : IRequest<CommonLookupRespObj>
    {
        public GetDocumenttypeQuery() { }
        public int DocumenttypeId { get; set; }
        public GetDocumenttypeQuery(int lookUpId)
        {
            DocumenttypeId = lookUpId;
        }
    }
    public class GetCurrencyRateQuery : IRequest<CommonLookupRespObj>
    {
        public GetCurrencyRateQuery() { }
        public int CurrencyRateId { get; set; }
        public GetCurrencyRateQuery(int lookUpId)
        {
            CurrencyRateId = lookUpId;
        }
    }
    public class GetIdentificationQuery : IRequest<CommonLookupRespObj>
    {
        public GetIdentificationQuery() { }
        public int IdentificationId { get; set; }
        public GetIdentificationQuery(int lookUpId)
        {
            IdentificationId = lookUpId;
        }
    }
    public class GetCurrencyQuery : IRequest<CommonLookupRespObj>
    {
        public GetCurrencyQuery() { }
        public int CurrencyId { get; set; }
        public GetCurrencyQuery(int lookUpId)
        {
            CurrencyId = lookUpId;
        }
    }
    public class GetCreditBureauQuery : IRequest<CommonLookupRespObj>
    {
        public GetCreditBureauQuery() { }
        public int CreditBureauId { get; set; }
        public GetCreditBureauQuery(int lookUpId)
        {
            CreditBureauId = lookUpId;
        }
    }
    public class GetCityQuery : IRequest<CommonLookupRespObj>
    {
        public GetCityQuery() { }
        public int CityId { get; set; }
        public GetCityQuery(int lookUpId)
        {
            CityId = lookUpId;
        }
    }
    public class GetJobTitleQuery : IRequest<CommonLookupRespObj>
    {
        public GetJobTitleQuery() { }
        public int JobTitleId { get; set; }
        public GetJobTitleQuery(int lookUpId)
        {
            JobTitleId = lookUpId;
        }
    }

    public class GetCurrencyRateByCurrrencyQuery : IRequest<CommonLookupRespObj>
    {
        public GetCurrencyRateByCurrrencyQuery() { }
        public int CurrencyId { get; set; }
        public GetCurrencyRateByCurrrencyQuery(int lookUpId)
        {
            CurrencyId = lookUpId;
        }
    }

}
