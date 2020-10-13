using APIGateway.Contracts.Response.Common;
using GODPAPIs.Contracts.RequestResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIGateway.Contracts.Commands.Common
{
    public class AddUpdateCountryCommand : IRequest<LookUpRegRespObj>
    {

        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateJobTitleCommand : IRequest<LookUpRegRespObj>
    {
        public int JobTitleId { get; set; }
        public string Name { get; set; }
        public string JobDescription { get; set; }
        public string Skills { get; set; }
        public string SkillDescription { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateCityCommand : IRequest<LookUpRegRespObj>
    {
        public int CityId { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateBranchCommand : IRequest<LookUpRegRespObj>
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }

        public int CompanyId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateDocumenttypeCommand : IRequest<LookUpRegRespObj>
    {
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateCurrencyRateCommand : IRequest<LookUpRegRespObj>
    {
        public int CurrencyRateId { get; set; }

        public int CurrencyId { get; set; }

        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public double? BuyingRate { get; set; }

        public double? SellingRate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateIdentificationCommand : IRequest<LookUpRegRespObj>
    {
        public int IdentificationId { get; set; }

        [Required]
        [StringLength(250)]
        public string IdentificationName { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }
    public class AddUpdateCurrencyCommand : IRequest<LookUpRegRespObj>
    {
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(10)]
        public string CurrencyCode { get; set; }

        [Required]
        [StringLength(250)]
        public string CurrencyName { get; set; }

        public bool? BaseCurrency { get; set; }

        public bool? INUSE { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class AddUpdateCreditBureauCommand : IRequest<LookUpRegRespObj>
    {
        public int CreditBureauId { get; set; }

        [Required]
        [StringLength(300)]
        public string CreditBureauName { get; set; }

        [Column(TypeName = "money")]
        public decimal CorporateChargeAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal IndividualChargeAmount { get; set; }

        public int GLAccountId { get; set; }

        public bool IsMandatory { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class DeleteCountryCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteJobTitleCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    } 
    public class DeleteCityCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }

    public class DeleteStateCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteBranchCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteDocumentTypeCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteCurrencyRateCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteIdentificationCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteCurrencyCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
    public class DeleteCreditBureauCommand : IRequest<DeleteRespObj>
    {
        public List<int> ItemsId { get; set; }
    }
     
}