using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.GovernorateSpecification;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GovernoratesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GovernoratesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        [Route("~/Governorates/GetAll")]
        [Authorize(Policy =
            $"Permission={Governorates.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetAllGovernorates([FromQuery] GovernorateParams param)
        {
            try
            {
                if (param == null)
                {
                    return BadRequest(new { message = "Invalid governorate parameters" });
                }

                // Validate pagination parameters
                if (param.PageIndex < 1 || param.PageSize < 1)
                {
                    return BadRequest(new { message = "PageIndex and PageSize must be greater than 0" });
                }

                var result = await _unitOfWork.GovernorateService.GetAllGovernoratesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework in production)
                return StatusCode(500, new { message = "An error occurred while retrieving governorates", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/Governorates/GetById/{id:int}")]
        [Authorize(Policy =
            $"Permission={Governorates.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetGovernorateById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid governorate ID" });
                }

                var governorate = await _unitOfWork.GovernorateService.GetGovernorateByIdAsync(id);
                if (governorate == null)
                {
                    return NotFound(new { message = $"Governorate with ID {id} not found" });
                }

                return Ok(governorate);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = $"An error occurred while retrieving governorate with ID {id}", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("~/Governorates/Create")]
        [Authorize(Policy =
            $"Permission={Governorates.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> CreateGovernorate([FromBody] CreateGovernorateDTO governorateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid governorate data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(governorateDTO.Name))
                {
                    return BadRequest(new { message = "Governorate name is required" });
                }

                var (isSuccess, message) = await _unitOfWork.GovernorateService.CreateGovernorateAsync(governorateDTO);

                if (!isSuccess)
                {
                    return BadRequest(new { message });
                }

                return Created("", governorateDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while creating the governorate", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("~/Governorates/Update/{id:int}")]
        [Authorize(Policy =
            $"Permission={Governorates.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> UpdateGovernorate(int id, [FromBody] GovernorateDTO governorateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid governorate data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid governorate ID" });
                }

                if (id != governorateDTO.Id)
                {
                    return BadRequest(new { message = "Governorate ID in URL does not match the DTO ID" });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(governorateDTO.Name))
                {
                    return BadRequest(new { message = "Governorate name cannot be empty or whitespace" });
                }

                var (isSuccess, message) = await _unitOfWork.GovernorateService.UpdateGovernorateAsync(id, governorateDTO);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while updating the governorate", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("~/Governorates/Delete/{id:int}")]
        [Authorize(Policy =
            $"Permission={Governorates.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid governorate ID" });
                }

                var (isSuccess, message) = await _unitOfWork.GovernorateService.DeleteGovernorateAsync(id);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while deleting the governorate", error = ex.Message });
            }
        }
    }
}