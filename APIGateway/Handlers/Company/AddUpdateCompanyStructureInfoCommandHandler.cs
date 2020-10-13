using APIGateway.Contracts.Commands.Company;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.UserAccount;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Ccompany
{
    public class AddUpdateCompanyStructureInfoCommandHandler : IRequestHandler<AddUpdateCompanyStructureInfoCommand, CompanyStructureRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ICompanyRepository _repo;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddUpdateCompanyStructureInfoCommandHandler(
            ILoggerService loggerService, 
            ICompanyRepository companyRepository,
            UserManager<cor_useraccount> userManger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerService;
            _repo = companyRepository;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CompanyStructureRegRespObj> Handle(AddUpdateCompanyStructureInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);

                var structure = new cor_companystructure
                {
                    Code = request.CompanyCode,
                    CountryId = request.CountryId,
                    Address = request.Address1,
                    HeadStaffId = request.HeadStaffId,
                    CompanyId = request.CompanyId,
                    Address1 = request.Address1,
                    Address2 = request.Address2,
                    Telephone = request.Telephone,
                    Fax = request.Fax,
                    Email = request.Email,
                    RegistrationNumber = request.RegistrationNumber,
                    TaxId = request.TaxId,
                    NoOfEmployees = request.NoOfEmployees,
                    WebSite = request.WebSite,
                    Logo = request.Logo,
                    LogoType = request.LogoType,
                    State = request.State,
                    City = request.City,
                    CurrencyId = request.CurrencyId,
                    ReportCurrencyId = request.ReportingCurrencyId,
                    ApplyRegistryTemplate = request.ApplyRegistryTemplate,
                    RegistryTemplate = request.RegistryTemplate,
                    PostalCode = request.PostalCode,
                    IsMultiCompany = request.IsMultiCompany,
                    Subsidairy_Level = request.Subsidairy_Level,
                    Description = request.Description,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                    ParentCompanyID = request.ParentCompanyID,
                    Name = request.CountryName

                };

                if (request.CompanyStructureId > 1) request.CompanyStructureId = request.CompanyStructureId;
                await _repo.AddUpdateCompanyStructureInfoAsync(structure);
                var actionTaken = request.CompanyId > 0 ? "updated" : "added";
                return new CompanyStructureRegRespObj
                {
                    CompanyStructureId = structure.CompanyStructureId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Company structure successfully  {actionTaken}",
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdateCompanyStructureInfoCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CompanyStructureRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateCompanyStructureInfoCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }
    }
}
