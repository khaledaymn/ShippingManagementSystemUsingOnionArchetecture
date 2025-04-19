using Microsoft.AspNetCore.Authorization;
using ShippingManagementSystem.Application.Helpers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Filters
{
    //public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    //{
    //    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    //    {
    //        if (context.User == null)
    //            return;

    //        var canAccess = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission);

    //        if (canAccess)
    //        {
    //            context.Succeed(requirement);
    //        }
    //    }

    //}
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private static readonly HashSet<string> ValidRoles = new HashSet<string>
    {
        Roles.Admin,
        Roles.ShippingRepresentative,
        Roles.Merchant
    };

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
                return;

            // Check for permission claim
            bool hasPermission = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission);

            // Check for valid roles in JWT
            bool hasValidRole = context.User.Claims
                .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role) // Handle both "role" and standard role claim
                .Any(c => ValidRoles.Contains(c.Value));

            // Succeed if either condition is met
            if (hasPermission || hasValidRole)
            {
                context.Succeed(requirement);
            }
        }
    }
}