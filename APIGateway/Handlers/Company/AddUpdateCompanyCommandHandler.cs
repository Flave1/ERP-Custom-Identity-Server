using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.UserAccount;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Commands.Company;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Company
{
    public class AddUpdateCompanyCommandHandler : IRequestHandler<AddUpdateCompanyCommand, CompanyRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ICompanyRepository _repo;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddUpdateCompanyCommandHandler(ILoggerService loggerService, ICompanyRepository companyRepository,
            UserManager<cor_useraccount> userManger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerService;
            _repo = companyRepository;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CompanyRegRespObj> Handle(AddUpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                if (request.CompanyId < 1)
                {
                    if (await _repo.CompanyExistAsync(request.Email, request.Name))
                        return new CompanyRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = $"Company with this email or or name already exist"
                                }
                            }
                        };
                }

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);
                var company = new cor_company
                {
                    Name = request.Name,
                    Code = request.Code,
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
                    CountryId = request.CountryId,
                    ApplyRegistryTemplate = request.ApplyRegistryTemplate,
                    RegistryTemplate = request.RegistryTemplate,
                    PostalCode = request.PostalCode,
                    IsMultiCompany = request.IsMultiCompany,
                    Subsidairy_Level = request.Subsidairy_Level,
                    Description = request.Description,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = request.CompanyId > 0 ? request.CreatedOn : DateTime.Now,
                    UpdatedBy = user.UserName,
                    UpdatedOn = DateTime.Now,
                    
                };
                if (request.CompanyId > 0) company.CompanyId = request.CompanyId;

                await _repo.UpdateCompanyAsync(company);
                var actionTaken = request.CompanyId > 0 ? "updated" : "added";
                return new CompanyRegRespObj
                {
                    CompanyId = company.CompanyId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Company  successfully  {actionTaken}",
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdateCompanyCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CompanyRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateCompanyCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
