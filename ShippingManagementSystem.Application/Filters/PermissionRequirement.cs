using Microsoft.AspNetCore.Authorization;

namespace ShippingManagementSystem.Application.Filters
{
    #region Old Way

    //public class PermissionOrRolesRequirement : IAuthorizationRequirement
    //{
    //    public List<string> Permissions { get; }
    //    public List<string> Roles { get; }
    //    public List<string> SpecificPermissions { get; }
    //    public List<string> RequiredRole { get; }

    //    public PermissionOrRolesRequirement(List<string> permissions, List<string> roles, List<string> specificPermissions, List<string> requiredRole)
    //    {
    //        Permissions = permissions ?? new List<string>();
    //        Roles = roles ?? new List<string>();
    //        SpecificPermissions = specificPermissions ?? new List<string>();
    //        RequiredRole = requiredRole ?? new List<string>();
    //    }
    //}

    #endregion


    public class PermissionOrRolesRequirement : IAuthorizationRequirement
    {
        public List<string> Permissions { get; }
        public List<string> AllowedRoles { get; }
        public List<string> RequiredRoles { get; }

        public PermissionOrRolesRequirement(List<string> permissions, List<string> allowedRoles, List<string> requiredRoles)
        {
            Permissions = permissions ?? new List<string>();
            AllowedRoles = allowedRoles ?? new List<string>();
            RequiredRoles = requiredRoles ?? new List<string>();
        }
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

}
