using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Login

        /// <summary>
        /// Authenticates a user and returns authentication details including a token if successful.
        /// </summary>
        /// <param name="model">The login data transfer object containing the user's email and password.</param>
        /// <returns>
        /// Returns an authentication result with a token if successful, or an error message if authentication fails.
        /// </returns>
        /// <remarks>
        /// This endpoint validates the user's credentials and returns an authentication token if the login is successful.
        /// The request body must contain a valid email and password.
        /// 
        /// Example Request:
        /// ```json
        /// {
        ///   "email": "user@example.com",
        ///   "password": "P@ssw0rd123"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">
        /// Returns the authentication details including token when login is successful.
        /// Successful Response (200 OK):
        /// ```json
        /// {
        ///   "id": "12345",
        ///   "message": "Login successful",
        ///   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        ///   "role": "Employee",
        ///   "permissions": [
        ///     "ViewReports",
        ///     "EditProfile"
        ///   ]
        /// }
        /// </response>
        /// <response code="400">
        /// Returned when the login credentials are invalid or the request is malformed.
        /// Bad Request Response (400):
        /// ```json
        /// {
        ///   "message": "Invalid email or password"
        /// }
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs during processing.
        /// Server Error Response (500):
        /// ```json
        /// {
        ///   "message": "An error occurred while processing your request",
        ///   "error": "Exception details here"
        /// }
        /// </response>
        
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
        /// <summary>
        /// Initiates a password reset process for a user based on their email.
        /// </summary>
        /// <param name="model">The data transfer object containing the user's email for password reset.</param>
        /// <returns>
        /// Returns a success message if the password reset request is processed, or an error message if it fails.
        /// </returns>
        /// <remarks>
        /// This endpoint validates the user's email and initiates a password reset process if the email is valid.
        /// The request body must contain a valid email address.
        /// 
        /// Example Request:
        /// ```json
        /// {
        ///   "email": "user@example.com"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">
        /// Returns a confirmation message when the password reset request is successfully processed.
        /// Successful Response (200 OK):
        /// ```json
        /// "Forget password request completed successfully for email: user@example.com"
        /// </response>
        /// <response code="400">
        /// Returned when the request is malformed or the email is invalid.
        /// Bad Request Response (400):
        /// ```json
        /// {
        ///   "errors": {
        ///     "Email": [
        ///       "The Email field is required."
        ///     ]
        ///   }
        /// }
        /// </response>
        /// <response code="404">
        /// Returned when the provided email is not found or the reset request cannot be processed.
        /// Not Found Response (404):
        /// ```json
        /// "User not found or invalid email"
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs during processing.
        /// Server Error Response (500):
        /// ```json
        /// {
        ///   "message": "An error occurred while processing your request",
        ///   "error": "Exception details here"
        /// }
        /// </response>
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
        /// <summary>
        /// Resets a user's password using the provided reset token and new password.
        /// </summary>
        /// <param name="model">The data transfer object containing the reset token, email, and new password.</param>
        /// <returns>
        /// Returns a success message if the password is reset successfully, or an error message if the reset fails.
        /// </returns>
        /// <remarks>
        /// This endpoint validates the reset token and updates the user's password if the token and provided data are valid.
        /// The request body must contain a valid email, reset token, and new password.
        /// 
        /// Example Request:
        /// ```json
        /// {
        ///   "Password": "NewP@ssw0rd123"
        ///   "email": "user@example.com",
        ///   "token": "reset-token-123456",
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">
        /// Returns a confirmation message when the password is reset successfully.
        /// Successful Response (200 OK):
        /// ```json
        /// "Password Reseted Successful"
        /// </response>
        /// <response code="400">
        /// Returned when the request is malformed or the provided data is invalid.
        /// Bad Request Response (400):
        /// ```json
        /// {
        ///   "errors": {
        ///     "NewPassword": [
        ///       "The NewPassword field is required."
        ///     ]
        ///   }
        /// }
        /// </response>
        /// <response code="404">
        /// Returned when the reset token is invalid, expired, or the user is not found.
        /// Not Found Response (404):
        /// ```json
        /// "Invalid or expired reset token"
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs during processing.
        /// Server Error Response (500):
        /// ```json
        /// {
        ///   "message": "An error occurred while resetting your password",
        ///   "error": "Exception details here"
        /// }
        /// </response>        
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


        #region Change Password

        /// <summary>
        /// Changes a user's password based on their user ID, old password, and new password.
        /// </summary>
        /// <param name="model">The data transfer object containing the user ID, old password, and new password.</param>
        /// <returns>
        /// Returns a success message if the password is changed successfully, or an error message if the change fails.
        /// </returns>
        /// <remarks>
        /// This endpoint validates the user's old password and updates it to the new password if the provided data is valid.
        /// The request body must contain a valid user ID, old password, and new password.
        /// 
        /// Example Request:
        /// ```json
        /// {
        ///   "oldPassword": "OldP@ssw0rd123",
        ///   "newPassword": "NewP@ssw0rd123",
        ///   "userId": "12345"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">
        /// Returns a confirmation message when the password is changed successfully.
        /// Successful Response (200 OK):
        /// ```json
        /// "Password changed successfully."
        /// </response>
        /// <response code="400">
        /// Returned when the request is malformed or the provided data is invalid.
        /// Bad Request Response (400):
        /// ```json
        /// {
        ///   "errors": {
        ///     "NewPassword": [
        ///       "New password must be at least 6 characters long."
        ///     ]
        ///   }
        /// }
        /// </response>
        /// <response code="404">
        /// Returned when the user ID is invalid, old password is incorrect, or the new password is invalid.
        /// Not Found Response (404):
        /// ```json
        /// "Old password is incorrect."
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs during processing.
        /// Server Error Response (500):
        /// ```json
        /// {
        ///   "message": "An error occurred while changing your password",
        ///   "error": "Exception details here"
        /// }
        /// </response>
        [HttpPost]
        [Route("~/Account/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _unitOfWork.AuthenticationService.ChangePassword(model);

                if (result != "Password changed successfully.")
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while changing your password", error = ex.Message });
            }
        }
        #endregion


        #region GetSpecificUserData
        /// <summary>
        /// Initiates Get Specific User Data.
        /// </summary>
        /// <param name="id">The id to get specific user data.</param>
        /// <returns>
        /// Returns a user data if request is processed, or an error message if it fails.
        /// </returns>
        /// <remarks>
        /// This endpoint validates the user's id and gets his/her data.
        /// The request body must contain a valid id.
        /// 
        /// Example Request:
        ///
        /// </remarks>
        /// <response code="200">
        /// Returns a confirmation message when the data is successfully processed.
        /// Successful Response (200 OK):
        ///
        /// </response>
        /// <response code="400">
        /// Returned when the request is malformed or the id is invalid.
        /// Bad Request Response (400):
        ///
        /// </response>
        /// <response code="404">
        /// Returned when the provided id is not found or the request cannot be processed.
        /// Not Found Response (404):
        ///
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs during processing.
        /// Server Error Response (500):
        ///
        /// </response>
        [HttpGet]
        [Route("~/Account/GetSpecificUserData/{id}")]
        public async Task<IActionResult> GetSpecificUserData(string id)
        {
            try
            {
                var userdata = await _unitOfWork.AuthenticationService.GetSpecificUser(id);
                if (userdata == null)
                {
                    return NotFound("User not found");
                }
                return Ok(userdata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }
        #endregion


        #region Update User Data
        /// <summary>
        /// Initiates Update User Data.
        /// </summary>
        /// <param name="dto">The dto is the object that contained user updated data .</param>
        /// <returns>
        /// Returns a user data if request is processed, or an error message if it fails.
        /// </returns>
        /// <remarks>
        /// This endpoint validates the object of dto members and updates his/her data.
        /// The request body must contain a valid data .
        /// 
        /// Example Request:
        ///
        /// </remarks>
        /// <response code="200">
        /// Returns a confirmation message when the data is successfully processed.
        /// Successful Response (200 OK):
        ///
        /// </response>
        /// <response code="400">
        /// Returned when the request is malformed or the data is invalid.
        /// Bad Request Response (400):
        ///
        /// </response>
        /// <response code="404">
        /// Returned when the provided id is not found or the request cannot be processed.
        /// Not Found Response (404):
        ///
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs during processing.
        /// Server Error Response (500):
        ///
        /// </response>
        [HttpPut]
        [Route("~/Account/EditProfile")]
        public async Task<IActionResult> UpdateUserData(SpecificUserDataDTo dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.id))
                    return BadRequest(new { Message = "Invalid user data." });

                var (isSuccess, message) = await _unitOfWork.AuthenticationService.UpdateUserData(dto);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(new { Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }
        #endregion

    }
}
