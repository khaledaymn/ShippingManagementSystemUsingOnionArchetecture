using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.Settings;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services.IdentityServices
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AdminLogin _adminLogin;
        public AuthenticationServices(UserManager<ApplicationUser> userManager, 
            IOptions<JWT> jwt, IUnitOfWork unitOfWork, IOptions<AdminLogin> adminLogin)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _unitOfWork = unitOfWork;
            _adminLogin = adminLogin.Value;
        }
        
        private async Task<ApplicationUser> AdminLogin()
        {
            var admin = new ApplicationUser
            {
                Name = "Admin",
                Email = _adminLogin.Email,
                Address = "Egypt",
                PhoneNumber = "+201098684485",
                HiringDate = DateTime.Now,
                UserName = _adminLogin.Email
            };
            await _userManager.CreateAsync(admin, _adminLogin.Password);
            await _userManager.AddToRoleAsync(admin, Roles.Admin);
            return admin;
        }

        #region Login
        public async Task<AuthenticationResponseDTO> Login(LoginDTO data)
        {

            if (data == null || string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Password))
                return new AuthenticationResponseDTO
                {
                    Message = "Email and password are required"
                };
            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user == null && data.Email == _adminLogin.Email && data.Password == _adminLogin.Password)
               await AdminLogin();

            user = await _userManager.FindByNameAsync(data.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, data.Password))
                return new AuthenticationResponseDTO
                {
                    Message = "Email or Password is incorrect!",
                };
            try
            {

                var jwtToken = await CreateJWTToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                var result = new AuthenticationResponseDTO();
                result.Role = roles?.FirstOrDefault();
                if (roles.Any(r => r.Contains(Roles.Employee)))
                {
                    var permissions = new Dictionary<string, List<string>>();
                    var employeeClaims = await _userManager.GetClaimsAsync(user);

                    foreach (var claim in employeeClaims)
                    {
                        var parts = claim.Value.Split(".");
                        if (parts.Length == 3 && parts[0] == "Permission")
                        {
                            var moduleName = parts[1]; // e.g., "Branches"
                            var actionName = parts[2]; // e.g., "Create"

                            // If the moduleName doesn't exist in the dictionary, add it with a new list
                            if (!permissions.ContainsKey(moduleName))
                            {
                                permissions[moduleName] = new List<string>();
                            }

                            // Add only the action name to the module's list
                            permissions[moduleName].Add(actionName);
                        }
                    }

                    // Optional: Convert to JSON format


                    result.Permissions = permissions;

                }
                result.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                result.Message = "Login successfully";
                result.IsAuthenticated = true;
                result.Id = user.Id;
                return result;
            }
            catch (Exception ex)
            {
                return new AuthenticationResponseDTO
                {
                    Message = "An error occurred while generating the token: " + ex.Message
                };
            }
        }

        #endregion


        #region Forget Password

        public async Task<string> ForgetPassword(ForgetPasswordDTO dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return "Email is not registered!";
            }

            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var message = await _unitOfWork.EmailService.SendEmailAsync(user.Name, user.Email, token);

                return message;
            }
            catch (Exception ex)
            {
                return "An error occurred while processing your request.";
            }
        }

        #endregion


        #region Reset Password
        public async Task<string> ResetPassword(ResetPasswordDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Token) || string.IsNullOrEmpty(dto.Password))
            {
                return "Email, reset token, and new password are required.";
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return "Email is not registered!";
            }

            try
            {
                var resetResult = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
                if (!resetResult.Succeeded)
                {
                    var errors = string.Join("; ", resetResult.Errors.Select(e => e.Description));
                    return $"Failed to reset password: {errors}";
                }
                return "Password Reseted Successful";
            }
            catch (Exception ex)
            {
                return $"An error occurred while resetting your password: {ex.Message}"; ;
            }
        }


        #endregion


        #region Change Password
        public async Task<string> ChangePassword(ChangePasswordDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.OldPassword) || string.IsNullOrEmpty(dto.NewPassword))
                return "User ID, old password, and new password are required.";

           
            // Ensure new password is not the same as the old password
            if (dto.OldPassword == dto.NewPassword)
                return "New password cannot be the same as the old password.";

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return "User not found!";

            try
            {
                // Verify old password
                var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
                if (!isOldPasswordValid)
                    return "Old password is incorrect.";

                // Change password
                var changeResult = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
                if (!changeResult.Succeeded)
                {
                    var errors = string.Join("; ", changeResult.Errors.Select(e => e.Description));
                    return $"Failed to change password: {errors}";
                }

                return "Password changed successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is implemented)
                return $"An error occurred while changing your password: {ex.Message}";
            }
        }

        #endregion


        #region Craete JWT Token

        private async Task<JwtSecurityToken> CreateJWTToken(ApplicationUser user)
        {
            // Retrieve user claims
            var UserClaims = await _userManager.GetClaimsAsync(user);

            // Retrieve user roles and create role claims
            var Roles = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();

            foreach (var role in Roles)
                RoleClaims.Add(new Claim(ClaimTypes.Role, role));

            // Combine user claims, role claims, and additional claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            }
            .Union(UserClaims)
            .Union(RoleClaims);

            // Define the security key and signing credentials
            SecurityKey securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            SigningCredentials signingCredentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var JWTSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);
            return JWTSecurityToken;
        }

        #endregion

    }
}
