using APIGateway.Data;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.Staff;
using GODP.APIsContinuation.DomainObjects.UserAccount;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.Commands.Admin;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.Admin;
using GODPCloud.Helpers.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APIGateway.Contracts.Commands.Email;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using GOSLibraries.Enums;

namespace GODP.APIsContinuation.Handlers.Admin
{
    public class AddUpdateEmployeeCommandHandler : IRequestHandler<UpdateStaffCommand, StaffRegRespObj>
    {
        private readonly IAdminRepository _adminRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly UserManager<cor_useraccount> _userManager; 
        private readonly IEmailService _email;
        private readonly IEmailConfiguration _emailConfig;
        public AddUpdateEmployeeCommandHandler(
            IAdminRepository repository, 
            ILoggerService loggerService, 
            IMapper mapper,
            IEmailService emailService,
            DataContext dataContext,
            UserManager<cor_useraccount> userManager, 
            IHttpContextAccessor httpContextAccessor,
            IEmailConfiguration emailConfiguration)
        {
            _mapper = mapper;
            _email = emailService;
            _adminRepo = repository;
            _logger = loggerService;
            _dataContext = dataContext;
            _userManager = userManager; 
            _emailConfig = emailConfiguration;
        }
        public async Task<StaffRegRespObj> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
        {
            var response = new StaffRegRespObj { Status = new APIResponseStatus { IsSuccessful =false, Message = new APIResponseMessage() } };
            try
            {  
                var userFromRepoExist = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.Trim().ToLower() == request.UserName.Trim().ToLower());
                var actionTaken = request.StaffId > 0 ? "updated" : "created";
                String tempUserId = String.Empty;
                if (request.StaffId < 1)
                {
                    if (userFromRepoExist != null)
                    { 
                        response.Status.Message.FriendlyMessage = "Username Already assigned to a staff";
                        return response;
                    }
                }

                if (request.StaffId < 1)
                {
                    if (await _userManager.Users.AnyAsync(x => x.Email.Trim().ToLower() == request.Email.Trim().ToLower()))
                    { 
                        response.Status.Message.FriendlyMessage = "Staff email already belongs to a user";
                        return response; 
                    }
                }
                IdentityResult AddUpdateAsUser = new IdentityResult();
                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    var stCode = StaffCode.Generate(_dataContext.cor_staff.Where(s => s.Deleted == false).OrderBy(d => d.StaffId).LastOrDefault()?.StaffId ?? 1);
                    try
                    {
                        var stf = new cor_staff();
                        stf.AccessLevel = request.AccessLevel;
                        stf.Address = request.Address;
                        stf.CountryId = request.CountryId; 
                        stf.PhoneNumber = request.PhoneNumber; 
                        stf.Email = request.Email;
                        stf.StateId = request.StateId; 
                        stf.DateOfBirth = request.DateOfBirth;
                        stf.FirstName = request.FirstName;
                        stf.MiddleName = request.MiddleName;
                        stf.Gender = request.Gender;
                        stf.JobTitle = request.JobTitle;
                        stf.LastName = request.LastName;
                        stf.Photo = request.Photo ?? null;
                        stf.StaffCode = request.StaffId > 0 ? request.StaffCode : stCode;
                        stf.StaffLimit = request.StaffLimit;
                        stf.StaffOfficeId = request.StaffOfficeId; 
                        stf.StaffId = request.StaffId;
                        stf.Active = true;
                        stf.Deleted = false;
                        stf.Active = true;
                        var staffIsCreated = await _adminRepo.AddUpdateStaffAsync(stf);
                       
                        if (staffIsCreated)
                        {
                            var user = new cor_useraccount();

                            user.PhoneNumber = request.PhoneNumber;
                            user.UserName = request.UserName;
                            user.Email = request.Email;
                            user.IsFirstLoginAttempt = true;
                            user.Active = true;
                            user.Deleted = false;
                            user.CreatedBy = "admin";
                            user.CreatedOn = DateTime.Now; 
                            user.StaffId = stf.StaffId;
                            user.IsActive = request.UserStatus.Trim().ToLower() == "true"? true : false;
                            user.LastLoginDate = DateTime.Now;

                            if (userFromRepoExist == null)
                            {
                                AddUpdateAsUser = await _userManager.CreateAsync(user, "Password@1");
                                tempUserId = user.Id;
                                if (!AddUpdateAsUser.Succeeded)
                                {
                                    await _transaction.RollbackAsync();
                                    response.Status.Message.FriendlyMessage = AddUpdateAsUser.Errors.FirstOrDefault().Description;
                                    return response;
                                }
                            }
                            else
                            {
                                cor_useraccount alreadExistingUser = new cor_useraccount();
                                alreadExistingUser = await _userManager.FindByNameAsync(userFromRepoExist.UserName);
                                alreadExistingUser.PhoneNumber = request.PhoneNumber;
                                alreadExistingUser.UserName = request.UserName;
                                alreadExistingUser.Email = request.Email;
                                alreadExistingUser.IsFirstLoginAttempt = false;
                                alreadExistingUser.StaffId = stf.StaffId;
                                tempUserId = alreadExistingUser.Id;
                                AddUpdateAsUser = await _userManager.UpdateAsync(alreadExistingUser);
                                if (!AddUpdateAsUser.Succeeded)
                                {
                                    await _transaction.RollbackAsync();
                                    response.Status.Message.FriendlyMessage = AddUpdateAsUser.Errors.FirstOrDefault().Description;
                                    return response; 
                                }
                            }
                            
                            if (AddUpdateAsUser.Succeeded)
                            {
                                if (request.UserAccessLevels.Length > 0)
                                {
                                    List<cor_useraccess> useAccesses = new List<cor_useraccess>();
                                    foreach (var acessLevelId in request.UserAccessLevels)
                                    {
                                        var access = new cor_useraccess();
                                        access.AccessLevelId = acessLevelId; 
                                        access.UserId = tempUserId; 
                                        useAccesses.Add(access);
                                    }
                                    await _adminRepo.AddUpdateUseraccessAsync(useAccesses);
                                }
                                if (request.UserRoleNames.Length > 0)
                                {
                                    if(userFromRepoExist != null)
                                    {
                                        var removed = await _userManager.RemoveFromRolesAsync(userFromRepoExist, await _userManager.GetRolesAsync(userFromRepoExist));
                                        if (!removed.Succeeded)
                                        {
                                            await _transaction.RollbackAsync();
                                            response.Status.Message.FriendlyMessage = removed.Errors.FirstOrDefault().Description;
                                            return response; 
                                        }
                                        var updated = await _userManager.AddToRolesAsync(userFromRepoExist, request.UserRoleNames);
                                        if (!updated.Succeeded)
                                        {
                                            await _transaction.RollbackAsync();
                                            response.Status.Message.FriendlyMessage = updated.Errors.FirstOrDefault().Description;
                                            return response;
                                        }
                                    }
                                    else
                                    {
                                        var UpdateUserRoles = await _userManager.AddToRolesAsync(user, request.UserRoleNames);

                                        if (!UpdateUserRoles.Succeeded)
                                        {
                                            await _transaction.RollbackAsync();
                                            response.Status.Message.FriendlyMessage = UpdateUserRoles.Errors.FirstOrDefault().Description;
                                            return response; 
                                        }
                                    }
                                    
                                }
                                 await SendStaffAccountDetailMail(request.Email, request.UserName, request.FirstName +" "+ request.LastName, actionTaken);
                                await _transaction.CommitAsync();
                            }
                           }
                        }
                    catch (Exception ex)
                    {
                        await _transaction.RollbackAsync();
                        #region Log error to file 
                        var errorCode = ErrorID.Generate(4);
                        _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                        return new StaffRegRespObj
                        {

                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = ex?.Message,
                                    MessageId = errorCode,
                                    TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                                }
                            }
                        };
                        #endregion
                    }
                    finally { await _transaction.DisposeAsync(); }
                    
                    return new StaffRegRespObj
                    {

                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Staff Details have successfully been {actionTaken}",
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new StaffRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        private  async Task SendStaffAccountDetailMail(string email, string username, string name, string action)
        { 
            var sm = new SendEmailCommand();
            sm.Subject = $"Account details {action}";
            sm.Content = $"Dear { name } <br/>" +
                $"Below is your account login details <br/> " +
                $"<b>Username : {username} <br/>" +
                $"<b>Password : Password@1 <br/>" +
                $"Please be sure to change your password on first login";
            sm.ToAddresses.Add(new EmailAddressCommand { Address = email, Name = name });
            var mailSent = await _email.BuildAndSaveEmail(sm);

            
            EmailMessage em = new EmailMessage {
                Subject = sm.Subject,
                Content = sm.Content,
                FromAddresses = mailSent.FromAddresses,
                ToAddresses = _mapper.Map<List<EmailAddress>>(sm.ToAddresses),
            };
            sm.SendIt = true;
            em.Module = (int)Modules.CENTRAL;
            await _email.Send(em);
        }
    }
}
