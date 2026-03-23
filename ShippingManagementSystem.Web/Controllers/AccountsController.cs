using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Login

        [HttpPost]
        [Route("~/Account/Login")]
        [SwaggerOperation(
            OperationId = "PerformLogin",
            Summary = "Authenticate user and issue access token",
            Description = "### 🔐 Secure Authentication & Authorization Engine\n\n" +
              "This endpoint serves as the primary gateway to the system. It validates user credentials and returns a comprehensive identity package, including security tokens and granular permissions.\n\n" +
              "#### **⚙️ Internal Business Logic:**\n" +
              "| Step | Action | Details |\n" +
              "| :--- | :--- | :--- |\n" +
              "| **1. Request Validation** | Model State Check | Ensures Email/Username and Password are provided and correctly formatted. |\n" +
              "| **2. Admin Auto-Seeding** | Emergency Access | If the database is empty and input matches `appsettings.json` Admin credentials, the system automatically creates the root Admin account. |\n" +
              "| **3. Identity Lookup** | Dual-Stream Auth | The system attempts to find the user by **Email** first; if not found, it falls back to **Username** lookup. |\n" +
              "| **4. Role Extraction** | RBAC Analysis | Retrieves all assigned roles (Admin, Merchant, Delivery, Employee) from the Identity system. |\n" +
              "| **5. Permission Mapping** | Employee Claims | For **Employee** roles, it parses claims with the format `Permission.{Module}.{Action}` and groups them into a structured dictionary for the Frontend. |\n" +
              "| **6. Token Generation** | JWT Signing | Issues a cryptographically signed token containing `Email`, `Jti`, and `Role` claims for session management. |\n\n" +
              "#### **🛡️ Security Specifications:**\n" +
              "* **Hashing**: Passwords are verified using ASP.NET Core Identity's default PBKDF2 hashing. \n" +
              "* **Policy**: Implements **Role-Based Access Control (RBAC)** and **Claim-Based Authorization**. \n" +
              "* **Expiration**: Token validity is governed by the `JWT:DurationInDays` configuration (currently set to 30 days). \n\n" +
              "> **⚠️ Important:** The `Token` returned must be included in the header of all protected requests as: `Authorization: Bearer {token}`.",
            Tags = new[] { "1. Security & Identity" }
        )]
        [SwaggerRequestExample(typeof(LoginDTO), typeof(LoginRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginSuccessResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LoginBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(AuthenticationResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _unitOfWork.AuthenticationService.Login(model);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
                var permissionList = result?.Permissions?.Select(p => new
                {
                    name = p.Key,
                    values = p.Value
                }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }

        #endregion

        #region Forget Password

        [HttpPost]
        [Route("~/Account/ForgetPassword")]
        [SwaggerOperation(
            OperationId = "PostForgetPassword",
            Summary = "Initiate password recovery workflow",
            Description = "### 📧 Account Recovery Engine\n\n" +
                          "This endpoint manages the initial phase of the password reset process. It validates user existence and dispatches a secure, time-limited token via the configured SMTP service.\n\n" +
                          "#### **⚙️ Internal Execution Flow:**\n" +
                          "| Step | Action | Logic Source |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **1. Identity Check** | User Validation | Queries `AspNetUsers` using `UserManager.FindByEmailAsync`. If not found, returns **400 BadRequest**. |\n" +
                          "| **2. Token Generation** | Security Token | Generates a specific password reset token cryptographically linked to the user account. |\n" +
                          "| **3. Safe Encoding** | URL Preparation | Performs `HttpUtility.UrlEncode` on the token to ensure safe transmission via email query parameters. |\n" +
                          "| **4. Email Dispatch** | SMTP Integration | Triggers `EmailService.SendEmailAsync` to send an HTML-formatted message using `MailKit`. |\n\n" +
                          "#### **🛡️ Security Safeguards:**\n" +
                          "* **Token Validity**: The token is invalidated immediately upon a successful password change or after its expiration period. \n" +
                          "* **Encryption**: Email transmission is secured via SSL/TLS protocols as defined in the system settings. \n\n" +
                          "> **Note:** Successful requests will return a confirmation string containing the recipient's email address.",
            Tags = new[] { "1. Security & Identity" }
        )]
        [SwaggerRequestExample(typeof(ForgetPasswordDTO), typeof(ForgetPasswordRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ForgetPasswordSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ForgetPasswordBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _unitOfWork.AuthenticationService.ForgetPassword(model);

                if (result != "success")
                {
                    return NotFound(result);
                }

                return Ok($"Forget password request completed successfully for email: {model.Email}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }

        #endregion

        #region Reset Password

        [HttpPost]
        [Route("~/Account/ResetPassword")]
        [SwaggerOperation(
            OperationId = "PostResetPassword",
            Summary = "Finalize password reset using recovery token",
            Description = "### 🔒 Secure Credential Update\n\n" +
                          "This endpoint completes the account recovery cycle. It consumes the cryptographic token generated in the 'Forget Password' phase to authorize a password override.\n\n" +
                          "#### **⚙️ Internal Business Logic:**\n" +
                          "| Step | Action | Logic Source |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **1. Identity Validation** | Account Lookup | Verifies that the provided **Email** corresponds to an active user in the `AspNetUsers` store. |\n" +
                          "| **2. Cryptographic Check** | Token Verification | The `UserManager` validates the **Token** signature, ensuring it hasn't been tampered with or expired. |\n" +
                          "| **3. Policy Enforcement** | Password Complexity | Ensures the new password meets system requirements (Length, Non-Alphanumeric, etc.). |\n" +
                          "| **4. Database Update** | Hash Rotation | If valid, the existing password hash is replaced, and the `SecurityStamp` is updated to invalidate existing sessions. |\n\n" +
                          "#### **🛡️ Security Policies:**\n" +
                          "* **One-Time Consumption**: Once a token is successfully used, it is immediately revoked. \n" +
                          "* **Session Invalidation**: Resetting the password will trigger a logout across all devices for this user account. \n\n" +
                          "> **Note:** If the process fails due to an expired token, the user must restart the 'Forget Password' flow.",
            Tags = new[] { "1. Security & Identity" }
        )]
        [SwaggerRequestExample(typeof(ResetPasswordDTO), typeof(ResetPasswordRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResetPasswordSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ResetPasswordBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _unitOfWork.AuthenticationService.ResetPassword(model);

                if (result != "Password Reseted Successful")
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while resetting your password", error = ex.Message });
            }
        }

        #endregion

    }
}
