using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.ActivityRequirement
{
    public class CanUpdateRequirement : IAuthorizationRequirement
    {
        public string CanUpdate { get; set; }
        public CanUpdateRequirement(string canUpdate)
        {
            CanUpdate = canUpdate;
        }
    }

    public class CanAddRequirement : IAuthorizationRequirement
    {
        public string CanAdd { get; set; }
        public CanAddRequirement(string canAdd)
        {
            CanAdd = canAdd;
        }
    }

    public class CanDeleteRequirement : IAuthorizationRequirement
    {
        public string CanDelete { get; set; }
        public CanDeleteRequirement(string canDelete)
        {
            CanDelete = canDelete;
        }
    }

    public class CanApproveRequirement : IAuthorizationRequirement
    {
        public string CanApprove { get; set; }
        public CanApproveRequirement(string canApprove)
        {
            CanApprove = canApprove;
        }
    }

    public class CanViewRequirement : IAuthorizationRequirement
    {
        public string CanView { get; set; }
        public CanViewRequirement(string canView)
        {
            CanView = canView;
        }
    }
}
