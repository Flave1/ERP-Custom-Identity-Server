using APIGateway.Contracts.Response.Admin;
using GODPAPIs.Contracts.Response.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Admin
{
    public class GetActivitiesByRoleIdQuery : IRequest<ActivityRespObj>
    {
        public GetActivitiesByRoleIdQuery() { }
        public string RoleId { get; set; }
        public GetActivitiesByRoleIdQuery(string roleId)
        {
            RoleId = roleId;
        }
    }

    public class GetAllActivityParentQuery : IRequest<ActivityParentRespObj> { }

    public class GetAllActivityQuery : IRequest<ActivityRespObj> { }

    public class GetAllRolesQuery : IRequest<RoleRespObj> { }

    public class GetUserRolesQuery : IRequest<UserRoleRespObj> { }
    public class GetUserRoleActivitiesQuery : IRequest<UserRoleRespObj> { }

    public class GetAllStaffQuery : IRequest<StaffRespObj> { }
    public class GetStaffQuery : IRequest<StaffRespObj>
    {
        public GetStaffQuery() { }
        public int StaffId { get; set; }
        public GetStaffQuery(int staffId)
        {
            StaffId = staffId;
        }
    }
}
