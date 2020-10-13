using APIGateway;
using APIGateway.Data;
using APIGateway.Repository.Inplimentation.Common;
using APIGateway.Repository.Inplimentation.Workflow;
using APIGateway.Repository.Interface.Common;
using APIGateway.Repository.Interface.Workflow;
using AutoMapper; 
using GODP.APIsContinuation.DomainObjects.Account;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.Repository.Inplimentation;
using GODP.APIsContinuation.Repository.Inplimentation.Admin;
using GODP.APIsContinuation.Repository.Inplimentation.Setup.Company;
using GODP.APIsContinuation.Repository.Interface;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GODP.APIsContinuation.Installers
{
    public class DbInstaller : IInstaller
    { 
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                   options.UseSqlServer(
                       configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<cor_useraccount>(opt =>
            {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
            })
                .AddRoles<cor_userrole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAutoMapper(typeof(Startup)); 
            services.AddMediatR(typeof(Startup));
            services.AddMvc();

            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IWorkflowRepository, WorkflowRepository>();
            services.AddTransient<ICommonRepository, CommonRepository>();

        }
    }
}
