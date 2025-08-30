using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ShippingManagementSystem.Application.Helper;

namespace ShippingManagementSystem.Application.Filters
{
    #region Old Way
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
    //        var specificPermissions = new List<string>
    //        {
    //            ChargeTypes.View,
    //            Branches.View,
    //            Cities.View,
    //            Governorates.View,
    //            Orders.View,
    //            Orders.Create,
    //            Orders.Edit,
    //            Orders.Delete,
    //            RejectedReasons.View,
    //            RejectedReasons.Create,
    //            RejectedReasons.Edit,
    //            Helper.Settings.View
    //        };

    //        var allPermissions = new List<string>
    //        {
    //            Branches.View,
    //            Branches.Create,
    //            Branches.Edit,
    //            Branches.Delete,
    //            ChargeTypes.View,
    //            ChargeTypes.Create,
    //            ChargeTypes.Edit,
    //            ChargeTypes.Delete,
    //            Employees.View,
    //            Employees.Create,
    //            Employees.Edit,
    //            Employees.Delete,
    //            Cities.View,
    //            Cities.Create,
    //            Cities.Edit,
    //            Cities.Delete,
    //            Governorates.View,
    //            Governorates.Create,
    //            Governorates.Edit,
    //            Governorates.Delete,
    //            Permissions.View,
    //            Permissions.Create,
    //            Permissions.Edit,
    //            Permissions.Delete,
    //            Merchants.View,
    //            Merchants.Create,
    //            Merchants.Edit,
    //            Merchants.Delete,
    //            Orders.View,
    //            Orders.Create,
    //            Orders.Edit,
    //            Orders.Delete,
    //            ShippingRepresentatives.View,
    //            ShippingRepresentatives.Create,
    //            ShippingRepresentatives.Edit,
    //            ShippingRepresentatives.Delete,
    //            RejectedReasons.View,
    //            RejectedReasons.Create,
    //            RejectedReasons.Edit,
    //            RejectedReasons.Delete,
    //            Helper.Settings.View,
    //            Helper.Settings.Create,
    //            Helper.Settings.Edit,
    //            Helper.Settings.Delete
    //        };

    //        if (policyName == "Group1")
    //        {
    //            var policy = new AuthorizationPolicyBuilder();
    //            policy.RequireAuthenticatedUser();
    //            policy.AddRequirements(new PermissionOrRolesRequirement(
    //                permissions: allPermissions,
    //                roles: new List<string> { Roles.Admin },
    //                specificPermissions: new List<string>(),
    //                requiredRole: new List<string> { Roles.Employee }
    //            ));
    //            return Task.FromResult(policy.Build());
    //        }

    //        if (policyName == "Group2")
    //        {
    //            var policy = new AuthorizationPolicyBuilder();
    //            policy.RequireAuthenticatedUser();
    //            policy.AddRequirements(new PermissionOrRolesRequirement(
    //                permissions: specificPermissions,
    //                roles: new List<string> { Roles.Admin, Roles.ShippingRepresentative, Roles.Merchant },
    //                specificPermissions: new List<string>(),
    //                requiredRole: new List<string> { Roles.Employee }
    //            ));
    //            return Task.FromResult(policy.Build());
    //        }

    //        if (policyName == "Group3")
    //        {
    //            var policy = new AuthorizationPolicyBuilder();
    //            policy.RequireAuthenticatedUser();
    //            policy.AddRequirements(new PermissionOrRolesRequirement(
    //                permissions: new List<string>(),
    //                roles: new List<string> { Roles.Admin,Roles.Employee,Roles.ShippingRepresentative, Roles.Merchant },
    //                specificPermissions: new List<string>(),
    //                requiredRole: new List<string>()
    //            ));
    //            return Task.FromResult(policy.Build());
    //        }

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
    #endregion

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
            var validPermissions = new List<string>
            {
                Branches.View, 
                Branches.Create, 
                Branches.Edit, 
                Branches.Delete,

                ChargeTypes.View, 
                ChargeTypes.Create, 
                ChargeTypes.Edit, 
                ChargeTypes.Delete,

                Employees.View, 
                Employees.Create, 
                Employees.Edit, 
                Employees.Delete,

                Cities.View, 
                Cities.Create, 
                Cities.Edit, 
                Cities.Delete,

                Governorates.View, 
                Governorates.Create, 
                Governorates.Edit, 
                Governorates.Delete,
                
                Permissions.View, 
                Permissions.Create, 
                Permissions.Edit, 
                Permissions.Delete,
                
                Merchants.View, 
                Merchants.Create, 
                Merchants.Edit, 
                Merchants.Delete,
                
                Orders.View, 
                Orders.Create, 
                Orders.Edit, 
                Orders.Delete,
                
                ShippingRepresentatives.View, 
                ShippingRepresentatives.Create, 
                ShippingRepresentatives.Edit, 
                ShippingRepresentatives.Delete,
                
                RejectedReasons.View, 
                RejectedReasons.Create, 
                RejectedReasons.Edit, 
                RejectedReasons.Delete,
                
                Helper.Settings.View, 
                Helper.Settings.Create,
                Helper.Settings.Edit, 
                Helper.Settings.Delete
            };

            var validRoles = new List<string>
            {
                Roles.Admin,
                Roles.Employee,
                Roles.ShippingRepresentative,
                Roles.Merchant
            };

            if (policyName.Contains("Permission="))
            {
                var parts = policyName.Split(';');
                List<string> permissions = new List<string>();
                List<string> requiredRoles = new List<string>();
                List<string> allowedRoles = new List<string>();

                foreach (var part in parts)
                {
                    if (part.StartsWith("Permission="))
                    {
                        permissions = part.Substring("Permission=".Length).Split(',').ToList();
                    }
                    else if (part.StartsWith("RequiredRole="))
                    {
                        requiredRoles = part.Substring("RequiredRole=".Length).Split(',').ToList();
                    }
                    else if (part.StartsWith("AllowedRole="))
                    {
                        allowedRoles = part.Substring("AllowedRole=".Length).Split(',').ToList();
                    }
                }

                if (permissions.Any(p => !validPermissions.Contains(p)))
                    return Task.FromResult<AuthorizationPolicy>(null);
                if (requiredRoles.Any(r => !validRoles.Contains(r)))
                    return Task.FromResult<AuthorizationPolicy>(null);
                if (allowedRoles.Any(r => !validRoles.Contains(r)))
                    return Task.FromResult<AuthorizationPolicy>(null);

                var policy = new AuthorizationPolicyBuilder();
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new PermissionOrRolesRequirement(
                    permissions: permissions,
                    allowedRoles: allowedRoles,
                    requiredRoles: requiredRoles
                ));
                return Task.FromResult(policy.Build());
            }

            if (validRoles.Contains(policyName))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new PermissionOrRolesRequirement(
                    permissions: new List<string>(),
                    allowedRoles: new List<string> { policyName },
                    requiredRoles: new List<string>()
                ));
                return Task.FromResult(policy.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }

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
}