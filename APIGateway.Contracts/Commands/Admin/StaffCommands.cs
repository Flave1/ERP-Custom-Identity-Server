using GODPAPIs.Contracts.RequestResponse;
using GODPAPIs.Contracts.Response.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Admin
{
    public class UpdateStaffCommand : IRequest<StaffRegRespObj>
    {
        public int StaffId { get; set; }
        public string StaffCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public byte[] Photo { get; set; }
        public decimal? StaffLimit { get; set; }
        public int? AccessLevel { get; set; }
        public int? StaffOfficeId { get; set; }

        public string UserName { get; set; }
        public string UserStatus { get; set; }
        public string UserId { get; set; }
        public int AccessLevelId { get; set; }
        public int[] UserAccessLevels { get; set; }

        public string[] UserRoleNames { get; set; }
    }
    public class DeleteStaffCommand : IRequest<DeleteRespObj>
    {
        public List<DeleteItemReqObj> req { get; set; }
    }
}
