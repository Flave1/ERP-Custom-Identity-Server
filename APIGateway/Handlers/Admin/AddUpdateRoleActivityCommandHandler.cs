using APIGateway.Data;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.Commands.Admin;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.Admin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GOSLibraries.GOS_Error_logger.Service;

namespace GODP.APIsContinuation.Handlers.Admin
{
    public class AddUpdateRoleActivityCommandHandler : IRequestHandler<AddUpdateUserRoleActivityCommand, RoleActivityRegRespObj>
    {
        private readonly IAdminRepository _adminRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<cor_userrole> _roleManager;
        public AddUpdateRoleActivityCommandHandler(IAdminRepository repository, ILoggerService loggerService, IMapper mapper, DataContext dataContext,
            UserManager<cor_useraccount> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<cor_userrole> roleManager)
        {
            _mapper = mapper;
            _adminRepo = repository;
            _logger = loggerService;
            _context = dataContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }
        public async Task<RoleActivityRegRespObj> Handle(AddUpdateUserRoleActivityCommand request, CancellationToken cancellationToken)
        {
            var response = new RoleActivityRegRespObj { Status = new APIResponseStatus { Message = new APIResponseMessage() } };
            try
            {
                var loggedInUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var loggedInUser = await _userManager.FindByIdAsync(loggedInUserId);
                var newActivity = new cor_userroleactivity();
                List<cor_userroleactivity> activities = new List<cor_userroleactivity>();

                using (var _trans = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                         
                        if (request.Activities.Count > 0)
                        {
                            foreach (var item in request.Activities)
                            {

                                if (item.ActivityId > 0 && !item.CanAdd && !item.CanDelete && !item.CanEdit && !item.CanApprove && !item.CanDelete && !item.CanView)
                                {
                                    response.Status.IsSuccessful = false;
                                    response.Status.Message.FriendlyMessage = $"Please activiate at least one action for selected page '{_context.cor_activity.FirstOrDefault(r => r.ActivityId == item.ActivityId)?.ActivityName}'";
                                    return response;
                                        ;
                                }
                                newActivity = new cor_userroleactivity()
                                {
                                    ActivityId = item.ActivityId,
                                    CanAdd = item.CanAdd,
                                    CanEdit = item.CanEdit,
                                    CanApprove = item.CanApprove,
                                    CanDelete = item.CanDelete,
                                    CanView = item.CanView,
                                    CreatedBy = loggedInUser.UserName,
                                    CreatedOn = DateTime.Now,
                                    Deleted = false,
                                    Active = false,
                                    RoleId = request.RoleId
                                    
                                    

                                };
                                activities.Add(newActivity);
                                
                            }
                        }

                        if (!string.IsNullOrEmpty(request.RoleId))
                        {
                            var existingRole = await _roleManager.FindByIdAsync(request.RoleId);

                            if (existingRole != null)
                            {
                                var targetActivities = _context.cor_userroleactivity.Where(x => x.RoleId == existingRole.Id).ToList();

                                if (targetActivities.Any())
                                {
                                    foreach (var item in targetActivities)
                                    {
                                        _context.cor_userroleactivity.Remove(item);
                                    }
                                }
                                existingRole.Name = request.RoleName;
                                existingRole.Active = true;
                                existingRole.UpdatedBy = loggedInUser.UserName;
                                existingRole.UpdatedOn = DateTime.Now;
                                var updated = await _roleManager.UpdateAsync(existingRole);
                                if (!updated.Succeeded)
                                {
                                    return new RoleActivityRegRespObj
                                    {

                                        Status = new APIResponseStatus
                                        {
                                            IsSuccessful = false,
                                            Message = new APIResponseMessage
                                            {
                                                FriendlyMessage = updated.Errors.Select(c => c.Description).FirstOrDefault(),
                                            }
                                        }
                                    };
                                }
                                else
                                {
                                    foreach (var item in activities)
                                    {
                                        item.RoleId = existingRole.Id;
                                        _context.cor_userroleactivity.Add(item);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var newRole = new cor_userrole
                            {
                                Name = request.RoleName,
                                Active = true,
                                Deleted = false,
                                CreatedBy = loggedInUser.UserName,
                                CreatedOn = DateTime.Now,
                                UpdatedBy = loggedInUser.UserName,
                                UpdatedOn = DateTime.Now,

                            };
                            // _context.cor_userrole.Add(user);
                           var creeated =  await _roleManager.CreateAsync(newRole);

                            if (!creeated.Succeeded)
                            {
                                return new RoleActivityRegRespObj
                                {

                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = false,
                                        Message = new APIResponseMessage
                                        {
                                            FriendlyMessage = creeated.Errors.Select(c => c.Description).FirstOrDefault(),
                                        }
                                    }
                                };
                            }
                            else
                            {
                                foreach (var item in activities)
                                {
                                    item.RoleId = newRole.Id;
                                    _context.cor_userroleactivity.Add(item);
                                }
                            }
                                

                        }
                        if (_context.SaveChanges() > 0)
                            await _trans.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await _trans.RollbackAsync();
                        #region Log error to file 
                        var errorCode = ErrorID.Generate(4);
                        _logger.Error($"ErrorID : AddUpdateRoleActivityCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                        return new RoleActivityRegRespObj
                        {

                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Error occured!! Unable to process request",
                                    MessageId = errorCode,
                                    TechnicalMessage = $"ErrorID : AddUpdateRoleActivityCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                                }
                            }
                        };
                        #endregion
                    }
                    finally { await _trans.DisposeAsync(); }
                   
                }

                var actionTaken = string.IsNullOrEmpty(request.RoleId) ? "created" : "updated";
                return new RoleActivityRegRespObj
                {
                    UserRoleActivityId = activities.FirstOrDefault().UserRoleActivityId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Activities Successfully  {actionTaken}",
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdateRoleActivityCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RoleActivityRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateRoleActivityCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }
    }
}
