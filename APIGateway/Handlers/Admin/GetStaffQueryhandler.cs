using APIGateway.Contracts.Queries.Admin;
using APIGateway.Data;
using GODP.APIsContinuation.DomainObjects.UserAccount;

using GOSLibraries.GOS_Error_logger.Service;
using GODPAPIs.Contracts.Response.Admin;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Admin
{
    public class GetStaffQueryhandler
    {
    }

    
    public class GetStaffQueryHandler : IRequestHandler<GetStaffQuery, StaffRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly UserManager<cor_useraccount> _userManager;
        public GetStaffQueryHandler(ILoggerService loggerService, DataContext dataContext, UserManager<cor_useraccount> userManager)
        {
            _logger = loggerService;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        private IList<string> GetAllRoleIds(string userId)
        {
            //var user =  _userManager.FindByIdAsync(userId);
            return _dataContext.UserRoles.Where(s => s.UserId == userId).Select(c => c.RoleId).ToList();
        }
        private  IEnumerable<int> GetAccessLevels(int staffId)
        {
            var userAccessList =  _dataContext.cor_useraccess.Where(d => d.Deleted == false).ToList();

            var user = _userManager.Users.FirstOrDefault(d => d.StaffId == staffId);
            if(user != null)
            {
              return userAccessList.Where(x => x.UserId == user.Id).Select(g => g.AccessLevelId).ToList().Distinct();
            }
            return new List<int>();
        }
        private IList<string> GetAllRoleNames(IList<string> roleIds)
        {
            //var user =  _userManager.FindByIdAsync(userId);
            return _dataContext.Roles.Where(s => roleIds.Contains(s.Id)).Select(c => c.Name).ToList();
        }

        public async Task<StaffRespObj> Handle(GetStaffQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // var countryList = await _dataContext.cor_country.Where(s => s.Deleted == false).ToListAsync();
                var JobList = _dataContext.cor_jobtitles.Where(x => x.Deleted == false).ToList();
                var companystructureList = _dataContext.cor_companystructure.Where(c => c.Deleted == false).ToList();
                //  var statesList = await _dataContext.cor_state.Where(d => d.Deleted == false).ToListAsync();
                //  var roleList = await _dataContext.UserRoles.ToListAsync();

                var staff = await _dataContext.cor_staff.FirstOrDefaultAsync(c => c.StaffId == request.StaffId);
                var userDetail = await _dataContext.Users.FirstOrDefaultAsync(f => f.StaffId == request.StaffId);

                var respList = new List<StaffObj>();
                if (staff != null)
                {
                    var item = new StaffObj
                    {
                        FirstName = staff.FirstName,
                        AccessLevel = staff.AccessLevel,
                        Email = staff.Email,
                        Gender = staff.Gender,
                        JobTitle = staff.JobTitle,
                        LastName = staff.LastName,
                        MiddleName = staff.MiddleName,
                        PhoneNumber = staff.PhoneNumber,
                        Photo = staff.Photo,
                        Address = staff.Address,
                        CountryId = staff.CountryId,
                        StaffCode = staff.StaffCode,
                        StaffId = staff.StaffId,
                        StateId = staff.StateId,
                        StaffLimit = staff.StaffLimit,
                        AccessLevelId = (int)staff.AccessLevel,
                        DateOfBirth = staff.DateOfBirth,
                        StaffOfficeId = staff.StaffOfficeId,
                        UserAccessLevels = GetAccessLevels(staff.StaffId),
                        //CountryName = countryList.Count() > 0 ? countryList.FirstOrDefault(x => staff.CountryId == x.CountryId).CountryName : string.Empty,
                        //JobTitleName = JobList.Count() > 0 || staff.JobTitle > 0 ? JobList.FirstOrDefault(x => staff.JobTitle == x.JobTitleId).Name : string.Empty,
                        //StaffOfficeName = companystructureList.Count() > 0 || staff.StaffOfficeId > 0 ? companystructureList.FirstOrDefault(x => x.CompanyStructureId == staff.StaffOfficeId).Name : string.Empty,
                        //StateName = statesList.Count() > 0 ? statesList.FirstOrDefault(x => x.StateId == staff.StateId).StateName : string.Empty,
                        UserRoleIds = GetAllRoleIds(userDetail.Id),
                        UserName = userDetail.UserName,
                        UserId = userDetail.Id,
                        UserStatus = userDetail.Active,
                        UserRoleNames = GetAllRoleNames(GetAllRoleIds(userDetail.Id))

                    };
                    item.UserAccessLevels.Distinct();
                    respList.Add(item);
                }
                
                return new StaffRespObj
                {
                    Staff = respList,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = respList.Count() == 0 ? "Search Complete! No Record Found" : null
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : GetStaffQueryHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new StaffRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : GetStaffQueryHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
