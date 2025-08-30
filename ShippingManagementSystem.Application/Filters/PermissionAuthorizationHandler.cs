using Microsoft.AspNetCore.Authorization;
using ShippingManagementSystem.Application.Helper;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Filters
{
    #region Old Code

    //public class PermissionOrRolesHandler : AuthorizationHandler<PermissionOrRolesRequirement>
    //{
    //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionOrRolesRequirement requirement)
    //    {
    //        if (requirement.Roles.Any(role => context.User.IsInRole(role)))
    //        {
    //            context.Succeed(requirement);
    //            return Task.CompletedTask;
    //        }

    //        if (context.User.Claims.Any(c => c.Type == "Permission" && requirement.SpecificPermissions.Contains(c.Value)))
    //        {
    //            context.Succeed(requirement);
    //            return Task.CompletedTask;
    //        }

    //        if (requirement.RequiredRole.Any() && 
    //            requirement.RequiredRole.Any(role => context.User.IsInRole(role)) &&
    //            context.User.Claims.Any(c => c.Type == "Permission" && requirement.Permissions.Contains(c.Value)))
    //        {
    //            context.Succeed(requirement);
    //            return Task.CompletedTask;
    //        }

    //        return Task.CompletedTask;
    //    }
    //}

    #endregion

    public class PermissionOrRolesHandler : AuthorizationHandler<PermissionOrRolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionOrRolesRequirement requirement)
        {
            if (requirement.AllowedRoles.Any(role => context.User.IsInRole(role)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (requirement.Permissions.Any() &&
                requirement.Permissions.All(p => context.User.Claims.Any(c => c.Type == "Permission" && c.Value == p)) &&
                (requirement.RequiredRoles.Count == 0 || requirement.RequiredRoles.All(role => context.User.IsInRole(role))))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "Permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    //public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    //{
    //    private static readonly HashSet<string> ValidRoles = new HashSet<string>
    //    {
    //        Roles.Admin,
    //        Roles.ShippingRepresentative,
    //        Roles.Merchant
    //    };

    //    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    //    {
    //        if (context.User == null || !context.User.Identity.IsAuthenticated)
    //            return;

    //        // Check for permission claim
    //        bool hasPermission = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission);

    //        // Check for valid roles in JWT
    //        bool hasValidRole = context.User.Claims
    //            .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role) // Handle both "role" and standard role claim
    //            .Any(c => ValidRoles.Contains(c.Value));

    //        // Succeed if either condition is met
    //        if (hasPermission || hasValidRole)
    //        {
    //            context.Succeed(requirement);
    //        }
    //    }
    //}
}