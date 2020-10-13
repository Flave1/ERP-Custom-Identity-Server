using GODPAPIs.Contracts.RequestResponse;
using GODPAPIs.Contracts.Response.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Admin
{
    public class AddUpdateUserRoleActivityCommand : IRequest<RoleActivityRegRespObj>
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public List<AddUpdateRoleActivityObj> Activities { get; set; }
    }

    public class DeleteRoleCommand : IRequest<DeleteRespObj>
    {
        public List<string> Req { get; set; }
    }
}
