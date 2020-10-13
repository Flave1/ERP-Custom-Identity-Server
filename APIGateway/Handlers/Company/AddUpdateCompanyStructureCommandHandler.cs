using APIGateway.Contracts.Commands.Company;
using APIGateway.Data;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.UserAccount;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace APIGateway.Handlers.Ccompany
{
    public class AddUpdateCompanyStructureCommandHandler : IRequestHandler<AddUpdateCompanyStructureCommand, CompanyStructureRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ICompanyRepository _repo;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        public AddUpdateCompanyStructureCommandHandler(ILoggerService loggerService, ICompanyRepository companyRepository,
            UserManager<cor_useraccount> userManger, IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _logger = loggerService;
            _repo = companyRepository;
            _dataContext = dataContext;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CompanyStructureRegRespObj> Handle(AddUpdateCompanyStructureCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);

                var logo = _httpContextAccessor.HttpContext.Request.Form.Files["logo"];
                var fSTemplate = _httpContextAccessor.HttpContext.Request.Form.Files["fSTemplate"];
                 

                var logoBytes = new byte[0];
                var fSTemplateBytes = new byte[0];
                if (logo != null && logo.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await logo.CopyToAsync(ms);
                        logoBytes = ms.ToArray();
                    }
                }

                if (fSTemplate != null && fSTemplate.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await fSTemplate.CopyToAsync(ms);
                        fSTemplateBytes = ms.ToArray();
                    }
                }

                string parentName = null;
                if (request.ParentCompanyID > 0)
                {
                    parentName =  _dataContext.cor_companystructure.FirstOrDefault(x => x.CompanyStructureId == request.ParentCompanyID)?.Name;
                }
              
                var structure = new cor_companystructure
                {
                    Name = request.Name,
                    StructureTypeId = request.StructureTypeId,
                    CountryId = request.CountryId,
                    Address = request.Address,
                    HeadStaffId = request.HeadStaffId,
                    ParentCompanyID = request.ParentCompanyID,
                    Parent = parentName,
                    CompanyId = request.CompanyId??0,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = user.UserName,
                    UpdatedOn = DateTime.Now,
                    CompanyStructureId = request.CompanyStructureId,
                    Address1 = request.Address1,
                    Address2 = request.Address2,
                    ApplyRegistryTemplate = request.ApplyRegistryTemplate,
                    City = request.City,
                    Code = request.Code,
                    CurrencyId = request.CurrencyId,
                    Description = request.Description,
                    Email = request.Email,
                    Fax = request.Fax,
                    IsMultiCompany = request.IsMultiCompany,
                    Logo = logoBytes,
                    FSTemplateName = fSTemplate?.Name,
                    LogoType = request.LogoType,
                    NoOfEmployees = request.NoOfEmployees,
                    PostalCode = request.PostalCode,
                    RegistryTemplate = request.RegistryTemplate,
                    RegistrationNumber = request.RegistrationNumber,
                    State = request.State,
                    TaxId = request.TaxId,
                    Telephone = request.Telephone,
                    WebSite = request.WebSite,
                    ReportCurrencyId = request.ReportCurrencyId,
                    Subsidairy_Level = request.Subsidairy_Level,
                    //FSTemplate = fSTemplateBytes
                };
                await _repo.AddUpdateCompanyStructureAsync(structure);
                var actionTaken = request.CompanyId > 0 ? "updated" : "added";
                return new CompanyStructureRegRespObj
                {
                    CompanyStructureId = structure.CompanyStructureId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Company structure  successfully  {actionTaken}",
                        }
                    }
                };


            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CompanyStructureRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateCompanyStructureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
