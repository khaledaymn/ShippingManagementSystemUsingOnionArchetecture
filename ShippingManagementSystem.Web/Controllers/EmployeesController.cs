using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        //[Authorize(Policy = Employees.View)]
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
        //[Authorize(Policy = Employees.Create)]
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
        //[Authorize(Policy = Employees.Edit)]
        public async Task<ActionResult> Update(UpdateEmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
                    return BadRequest(new { Message = "Invalid employee data." });

                var (isSuccess, message) = await _unitOfWork.EmployeeService.UpdateEmployeeAsync(dto);
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


        #region Delete Employee

        [HttpDelete]
        [Route("~/Employees/Delete/{id}")]
        //[Authorize(Policy = Employees.Delete)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var (isSuccess, message) = await _unitOfWork.EmployeeService.DeleteEmployeeAsync(id);
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


        #region Get Employee By Id

        [HttpGet]
        [Route("~/Employees/GetById/{id}")]
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
    }
}
