using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            var canAccess = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission);
            
            if (canAccess)
            {
                context.Succeed(requirement);
            }
        }

    }
}