using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Response.Email;
using APIGateway.MailHandler;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects;
using GODP.APIsContinuation.DomainObjects.Company;
using GODP.APIsContinuation.DomainObjects.Ifrs;
using GODP.APIsContinuation.DomainObjects.Staff; 
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.Commands.Admin;
using GODPAPIs.Contracts.Commands.Supplier;
using GODPAPIs.Contracts.RequestResponse.Supplier;
using GODPAPIs.Contracts.Response.Admin;
using GODPAPIs.Contracts.Response.CompanySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.MapProfile
{
    public class DomainToRequest : Profile
    {
        public DomainToRequest()
        {
            CreateMap<UpdateStaffCommand, cor_staff>();
            CreateMap<cor_staff, StaffObj>();
            CreateMap<cor_company, CompanyObj>();
            CreateMap<CompanyStructureDefinitionObj, cor_companystructuredefinition>();

            CreateMap<cor_companystructure, CompanyStructureObj>();
            CreateMap<CompanyStructureObj , cor_companystructure>();

            CreateMap<AddUpdateCompanyStructureObj, cor_companystructure>();

            CreateMap<EmailMessage, SendEmailCommand>();
            CreateMap<EmailAddressCommand, EmailAddress>();

            CreateMap<SendEmailCommand, EmailMessage>();

            CreateMap<SendEmailToSpicificOfficersCommand, SendEmailCommand>();
        }
    }
}
