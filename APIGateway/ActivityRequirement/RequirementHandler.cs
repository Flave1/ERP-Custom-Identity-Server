using APIGateway.AuthGrid;
using APIGateway.Contracts.V1;
using APIGateway.Data;
using APIGateway.Repository.Inplimentation.Cache;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger;
using GOSLibraries.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace APIGateway.ActivityRequirement
{ 
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ERPAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = new MiddlewareResponse { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            string userId = context.HttpContext.User?.FindFirst("userId")?.Value ?? string.Empty;
            StringValues authHeader = context.HttpContext.Request.Headers["Authorization"]; 

            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            if (context == null || hasAllowAnonymous)
            {
                await next();
                return;
            }
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(authHeader))
            {
                context.HttpContext.Response.StatusCode = 401; 
                context.Result = new UnauthorizedObjectResult(response);
                return;
            }
            string token = authHeader.ToString().Replace("Bearer ", "").Trim(); 
            var handler = new JwtSecurityTokenHandler();
            var tokena = handler.ReadJwtToken(token);
            var FromDate = tokena.IssuedAt.AddHours(1);
            var EndDate = tokena.ValidTo.AddHours(1);

            var expieryMatch = DateTime.UtcNow.AddHours(1);
            if (expieryMatch > EndDate)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnauthorizedObjectResult(response);
                return;
            }

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                try
                { 
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    JwtSettings tokenSettings = scopedServices.GetRequiredService<JwtSettings>(); 
                    DataContext _dataContext = scopedServices.GetRequiredService<DataContext>(); 

                    IDetectionService _detectionService = scopedServices.GetRequiredService<IDetectionService>();  
                    if (_detectionService.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
                    {
                        var currentDeviceDetail = _dataContext.Tracker.Where(q => q.UserId == userId && q.Token == token).ToList();
                        if (currentDeviceDetail.Count() == 0)
                        {
                            context.HttpContext.Response.StatusCode = 401;
                            response.Status.Message.FriendlyMessage = "Duplicate Login Detected";
                            context.Result = new UnauthorizedObjectResult(response);
                            return;
                        }
                    }   
                    IMeasureService _measureService = scopedServices.GetRequiredService<IMeasureService>();
                    if(_measureService.CheckForSessionTrailAsync(userId, (int)Modules.CENTRAL).Result.StatusCode == 401)
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        context.Result = new UnauthorizedObjectResult(response);
                        return;
                    }
                    await next();
                    return;
                }
                catch (Exception ex)
                {
                    context.HttpContext.Response.StatusCode = 500;
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = ex.Message;
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    context.Result = new InternalServerErrorObjectResult(response);
                    return;
                }
            } 
        } 
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ERPCacheAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = new MiddlewareResponse { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                try
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    RedisCacheSettings redisSettings = scopedServices.GetRequiredService<RedisCacheSettings>();
                    if (!redisSettings.Enabled)
                    {
                        await next();
                        return;
                    }
                    
                    DataContext _dataContext = scopedServices.GetRequiredService<DataContext>();
                    IResponseCacheService responseCacheService = scopedServices.GetRequiredService<IResponseCacheService>();
                    var cacheKey = Cache.GenerateCacheKeyFromRequest(context.HttpContext.Request);

                    if (context.HttpContext.Request.Method != "GET")
                    {
                        await responseCacheService.ResetCacheAsync(cacheKey);
                    }
                    var cachedResponse = await responseCacheService.GetCacheResponseAsync(cacheKey);

                    if (!string.IsNullOrEmpty(cachedResponse))
                    {
                        var contentResult = new ContentResult
                        {
                            Content = cachedResponse,
                            ContentType = "application/json",
                            StatusCode = 200
                        };
                        context.Result = contentResult;
                        return;
                    }

                    var executedContext = await next();
                    if (executedContext.Result is OkObjectResult okObjectResult)
                    {
                        await responseCacheService.CatcheResponseAsync(cacheKey, okObjectResult, TimeSpan.FromSeconds(1000));
                        context.HttpContext.Response.StatusCode = 200;
                        context.Result = new OkObjectResult(okObjectResult);
                        return;
                    }
                    await next();
                }
                catch (Exception ex)
                {
                    context.HttpContext.Response.StatusCode = 500;
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = ex.Message;
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    context.Result = new InternalServerErrorObjectResult(response);
                    return;
                }
            }
        } 
    }
    public class CurrentUserSesion
    {
        public DateTime SeeionDate { get; set; }
        public DateTime SessionExpiry { get; set; }
        public string UserId { get; set; }
    }

 

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ERPActivityAttribute : Attribute, IAsyncActionFilter
    {
        public int Activity { get; set; }
        public UserActions Action { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = new MiddlewareResponse { Status = new APIResponseStatus { Message = new APIResponseMessage() } };
            var userId = context.HttpContext.User?.FindFirst("userId")?.Value ?? string.Empty;
            IEnumerable<string> thisUserRoleIds = new List<string>();
            IEnumerable<string> thisUserRoleNames = new List<string>();
            IEnumerable<string> roleActivities = new List<string>();
            var thisUserRoleCan = false;

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                try
                {
                    var scopedServices = scope.ServiceProvider;
                    var _dataContext = scopedServices.GetRequiredService<DataContext>();

                    thisUserRoleIds = _dataContext.UserRoles.Where(x => x.UserId == userId).ToList().Select(x => x.RoleId);

                    thisUserRoleNames = (from role in _dataContext.Roles
                                         join userRole in _dataContext.UserRoles
                                         on role.Id.Trim().ToLower() equals userRole.RoleId.Trim().ToLower()
                                         where userRole.UserId == userId
                                         select role.Name).ToList();

                    roleActivities = (from activity in _dataContext.cor_activity
                                      join userActivityRole in _dataContext.cor_userroleactivity on activity.ActivityId equals userActivityRole.ActivityId
                                      select activity.ActivityName).ToList();

                    bool hasMatch = roleActivities.Select(x => x).Intersect(thisUserRoleNames).Any();

                    if (hasMatch)
                    {
                        if (Action == UserActions.Add)
                            thisUserRoleCan = _dataContext.cor_userroleactivity.Any(x => thisUserRoleIds.Contains(x.RoleId) && x.ActivityId == Activity && x.CanAdd == true);
                        if (Action == UserActions.Approve)
                            thisUserRoleCan = _dataContext.cor_userroleactivity.Any(x => thisUserRoleIds.Contains(x.RoleId) && x.ActivityId == Activity && x.CanApprove == true);
                        if (Action == UserActions.Delete)
                            thisUserRoleCan = _dataContext.cor_userroleactivity.Any(x => thisUserRoleIds.Contains(x.RoleId) && x.ActivityId == Activity && x.CanDelete == true);
                        if (Action == UserActions.Update)
                            thisUserRoleCan = _dataContext.cor_userroleactivity.Any(x => thisUserRoleIds.Contains(x.RoleId) && x.ActivityId == Activity && x.CanEdit == true);
                        if (Action == UserActions.View)
                            thisUserRoleCan = _dataContext.cor_userroleactivity.Any(x => thisUserRoleIds.Contains(x.RoleId) && x.ActivityId == Activity && x.CanView == true);
                    }


                    if (!thisUserRoleNames.Contains(StaticRoles.GODP))
                    {
                        if (!thisUserRoleCan)
                        {
                            response.Status.Message.FriendlyMessage = "You don't have privilege to perform this action";
                            var contentResponse = new ContentResult
                            {
                                Content = JsonConvert.SerializeObject(response),
                                ContentType = "application/json",
                                StatusCode = 403 
                            };
                            
                            context.HttpContext.Response.StatusCode = 403;
                            context.Result = contentResponse;
                            return;
                        }
                    }
                    await next();
                    return;
                }
                catch (Exception ex)
                {
                    var contentResponse = new MiddlewareResponse
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = ex?.Message, TechnicalMessage = ex.InnerException?.Message }
                        }
                    };
                    context.HttpContext.Response.StatusCode = 500;
                    context.Result = new BadRequestObjectResult(contentResponse);
                    return;
                }
            }
        }


    }

}
