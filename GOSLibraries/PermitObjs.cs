using System;
using System.Collections.Generic;
using System.Text;

namespace GOSLibraries
{
    public class UserPermission
    {
        public string UserId { get; set; }
        public IEnumerable<string> UserRoleIds { get; set; }
        public IEnumerable<string> UserRoleNames { get; set; }
        public IEnumerable<string> RoleActivityNames { get; set; }
    }
    public class UserPermissionRespObj
    {
        public UserPermission UserPermission { get; set; }
        public APIResponseStatus Staus { get; set; }
    }

    public class APIResponseStatus
    {
        public bool IsSuccessful { get; set; } = false;
        public string CustomToken { get; set; }
        public string CustomSetting { get; set; }
        public APIResponseMessage Message { get; set; }
    }

    public class APIResponseMessage
    {
        public string FriendlyMessage { get; set; }
        public string TechnicalMessage { get; set; }
        public string MessageId { get; set; }
        public string SearchResultMessage { get; set; }
        public string ShortErrorMessage { get; set; }
    }
}
