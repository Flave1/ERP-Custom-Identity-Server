using APIGateway.Contracts.Response.Admin;
using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GODPAPIs.Contracts.Response.Admin
{
    public class UserRoleActivityObj : GeneralEntity
    {
        public int UserRoleActivityId { get; set; }
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanView { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanApprove { get; set; }
    }
    public class RoleActivityRegRespObj
    {
        public int UserRoleActivityId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class UserRoleActivityRespObj
    {
        public List<UserRoleActivityObj> UserRoleActivity { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AddUpdateRoleActivityObj : GeneralEntity
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public int ActivityParentId { get; set; } 
        public int ActivityId { get; set; } 
        public bool CanEdit { get; set; }
        public bool CanAdd { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }

    public class RoleActivityObj : GeneralEntity
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public int ActivityParentId { get; set; }
        public string ActivityParentName { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAdd { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }


  

    public class UserRoleObj
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
      
    }


    public class UserRoleRespObj
    {
        public List<UserRoleObj> UserRoles { get; set; }
        public List<ActivityObj> UserRoleActivities { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class RoleRespObj
    {
        public List<RoleObj> Roles { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class RoleObj
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UserCount { get; set; }
    }

    public class Tracked
    {
        public int MeasureId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
    public class TrackerResp
    { 
        public HttpStatusCode StatusCode{ get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
