using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Helpers
{
   public static class EmailExtension
    {
        public static async Task<bool> CheckEmailExists(this string email, UserManager<ApplicationUser> userManager)
        {
            var result = await userManager.FindByEmailAsync(email);
            if (result == null) { return false; }
            return true;
        }
    }
}
