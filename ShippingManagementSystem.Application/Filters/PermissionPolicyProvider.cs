using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ShippingManagementSystem.Application.Helpers;
using System;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Filters
{
    //public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    //{
    //    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    //    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    //    {
    //        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    //    }

    //    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    //    {
    //        return FallbackPolicyProvider.GetDefaultPolicyAsync();
    //    }

    //    public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    //    {
    //        return Task.FromResult<AuthorizationPolicy>(null);
    //    }

    //    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    //    {
    //        if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
    //        {
    //            var policy = new AuthorizationPolicyBuilder();
    //            policy.RequireAuthenticatedUser();
    //            policy.AddRequirements(new PermissionRequirement(policyName));
    //            return Task.FromResult(policy.Build());
    //        }

    //        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    //    }
    //}


    //public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    //{
    //    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    //    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    //    {
    //        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    //    }

    //    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    //    {
    //        return FallbackPolicyProvider.GetDefaultPolicyAsync();
    //    }

    //    public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    //    {
    //        return Task.FromResult<AuthorizationPolicy>(null);
    //    }

    //    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    //    {
    //        if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
    //        {
    //            var policy = new AuthorizationPolicyBuilder();
    //            policy.RequireAuthenticatedUser();

    //            var allowedRoles = GetRolesForPolicy(policyName);
    //            policy.AddRequirements(new PermissionRequirement(policyName, allowedRoles));
    //            return Task.FromResult(policy.Build());
    //        }

    //        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    //    }

    //    private IEnumerable<string> GetRolesForPolicy(string policyName)
    //    {
    //        // Role mappings for each module
    //        if (policyName.Contains(Medules.ShippingRepresentatives))
    //        {
    //            return new[] { Roles.Admin, Roles.ShippingRepresentative };
    //        }
    //        else if (policyName.Contains(Medules.Merchants))
    //        {
    //            return new[] { Roles.Admin, Roles.Merchant };
    //        }
    //        else if (policyName.Contains(Medules.Orders))
    //        {
    //            // Example: Orders may involve all roles
    //            return new[] { Roles.Admin, Roles.ShippingRepresentative, Roles.Merchant };
    //        }
    //        else if (policyName.Contains(Medules.Branches) ||
    //                 policyName.Contains(Medules.ChargeTypes) ||
    //                 policyName.Contains(Medules.Employees) ||
    //                 policyName.Contains(Medules.Cities) ||
    //                 policyName.Contains(Medules.Governorates) ||
    //                 policyName.Contains(Medules.Permissions) ||
    //                 policyName.Contains(Medules.RejectedReasons) ||
    //                 policyName.Contains(Medules.Settings))
    //        {
    //            // Admin-only for sensitive modules
    //            return new[] { Roles.Admin };
    //        }

    //        // Default to Admin for unhandled policies
    //        return new[] { Roles.Admin };
    //    }
    //}


    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}