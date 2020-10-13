using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using GODP.APIsContinuation.Installers;
using System.IO;
using NLog;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.Options;
using Microsoft.AspNetCore.HttpOverrides;
using APIGateway.Contracts.V1;
using GOSLibraries;

namespace APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

       
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDetection();
            services.InstallServicesInAssembly(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        { 
            app.UseDetection();
            app.Use(async (ctx, next) => {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseRouting();
            app.UseStaticFiles();
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseCors(MyAllowSpecificOrigins);

           
            app.UseSwaggerUI(option => {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
                option.InjectStylesheet("/css/swagger.css");
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy(); 
            app.UseSession();
          
            app.UseExceptionHandler("/errors/500");
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
           
            app.UseMvc();
            CreateRolesAndAdminUser(serviceProvider).Wait();
           
        }



        private async Task CreateRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<cor_userrole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<cor_useraccount>>();
            string[] roleNames = { StaticRoles.GODP  };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new cor_userrole { Name = roleName });
                }
            }

            var userSettings = new UserSettings();
            Configuration.GetSection(nameof(UserSettings)).Bind(userSettings);

            var adminPassword = userSettings.Password;
            var adminUser = new cor_useraccount()
            {
                Email = userSettings.Email,
                UserName = userSettings.UserName,
            };

            var user = await UserManager.FindByEmailAsync(userSettings.Email);
            if (user == null)
            {
                var created = await UserManager.CreateAsync(adminUser, adminPassword);
                if (created.Succeeded) 
                { 
                    var added = await UserManager.AddToRolesAsync(adminUser, roleNames);
                    if (!added.Succeeded)
                    {
                        Console.Write(added.Errors);
                    }
                }
            }
        }
    }
}
