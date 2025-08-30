using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;

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


        //[HttpGet]
        //[Route("~/Account/CheckToken")]
        //public IActionResult CheckToken([FromQuery] List<string> permissions,
        //    [FromQuery] List<string> requiredRoles, [FromQuery] List<string> allowedRoles)
        //{
        //    try
        //    {
        //        var result = PolicyBuilder.BuildPolicy(permissions, requiredRoles, allowedRoles);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "An error occurred while checking the token", error = ex.Message });
        //    }
        //}
    }
}
