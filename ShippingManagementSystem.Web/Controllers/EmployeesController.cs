using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get All Employees

        [HttpGet]
        [Route("~/Employees/GetAll")]
        [Authorize(Policy =
            $"Permission={Employees.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult<IReadOnlyList<EmployeeDTO>>> GetAll([FromQuery] EmployeeParams param)
        {
            try
            {
                var result = await _unitOfWork.EmployeeService.GetAllEmployeesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Add Employee

        [HttpPost]
        [Route("~/Employees/Add")]
        [Authorize(Policy =
            $"Permission={Employees.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> Add(AddEmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (isSuccess, message) = await _unitOfWork.EmployeeService.AddEmployeeAsync(dto);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return CreatedAtAction(nameof(GetAll), null, new { Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Update Employee

        [HttpPut]
        [Route("~/Employees/Update")]
        [Authorize(Policy =
            $"Permission={Employees.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> Update(UpdateEmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
                    return BadRequest(new { Message = "Invalid employee data." });

                var (isSuccess, message) = await _unitOfWork.EmployeeService.UpdateEmployeeAsync(dto);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Delete Employee

        [HttpDelete]
        [Route("~/Employees/Delete/{id}")]
        [Authorize(Policy =
            $"Permission={Employees.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var (isSuccess, message) = await _unitOfWork.EmployeeService.DeleteEmployeeAsync(id);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Get Employee By Id

        [HttpGet]
        [Route("~/Employees/GetById/{id}")]
        [Authorize(Policy =
            $"Permission={Employees.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult<EmployeeDTO>> GetById(string id)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                    return NotFound(new { Message = "Employee not found." });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Change Password

        [HttpPost]
        [Route("~/Account/ChangePassword")]
        [Authorize(Roles =
            $"{Roles.Admin},{Roles.Merchant}," +
            $"{Roles.ShippingRepresentative},{Roles.Employee}")]
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

        [HttpGet]
        [Route("~/Account/GetSpecificUserData/{role}/{id}")]
        [Authorize(Roles =
            $"{Roles.Admin},{Roles.Merchant}," +
            $"{Roles.ShippingRepresentative},{Roles.Employee}")]
        public async Task<IActionResult> GetSpecificUserData(string role, string id)
        {
            try
            {
                var userdata = await _unitOfWork.AuthenticationService.GetSpecificUser(role, id);
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

        [HttpPut]
        [Route("~/Account/EditProfile")]
        [Authorize(Roles =
            $"{Roles.Admin},{Roles.Merchant}," +
            $"{Roles.ShippingRepresentative},{Roles.Employee}")]
        public async Task<IActionResult> UpdateUserData(SpecificUserDataDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
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
