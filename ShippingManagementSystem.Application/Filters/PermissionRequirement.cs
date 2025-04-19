using Microsoft.AspNetCore.Authorization;

namespace ShippingManagementSystem.Application.Filters
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
    //public class PermissionRequirement : IAuthorizationRequirement
    //{
    //    public string Permission { get; private set; }
    //    public IEnumerable<string> Roles { get; private set; }

    //    public PermissionRequirement(string permission, IEnumerable<string> roles = null)
    //    {
    //        Permission = permission;
    //        Roles = roles ?? Enumerable.Empty<string>();
    //    }
    //}
}
