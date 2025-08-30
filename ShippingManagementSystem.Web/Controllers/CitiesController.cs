using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.Services;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        #region Get All Cities
        [HttpGet]
        [Route("~/Cities/GetAll")]
        [Authorize(Policy =
            $"Permission={Cities.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetAllCities([FromQuery] CityParams param)
        {
            try
            {
                if (param == null)
                {
                    return BadRequest(new { message = "Invalid city parameters" });
                }

                // Validate pagination parameters
                if (param.PageIndex < 1 || param.PageSize < 1)
                {
                    return BadRequest(new { message = "PageIndex and PageSize must be greater than 0" });
                }

                var result = await _unitOfWork.CityService.GetAllCitiesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework in production)
                return StatusCode(500, new { message = "An error occurred while retrieving cities", error = ex.Message });
            }
        }
        #endregion


        #region Get City By Id
        [HttpGet]
        [Route("~/Cities/GetCityById/{id}")]
        [Authorize(Policy =
            $"Permission={Cities.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid city ID" });
                }

                var city = await _unitOfWork.CityService.GetCityByIdAsync(id);
                if (city == null)
                {
                    return NotFound(new { message = $"City with ID {id} not found" });
                }

                return Ok(city);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = $"An error occurred while retrieving city with ID {id}", error = ex.Message });
            }
        }
        #endregion


        #region Create City
        [HttpPost]
        [Route("~/Cities/CreateCity")]
        [Authorize(Policy =
            $"Permission={Cities.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDTO cityDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid city data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(cityDTO.Name))
                {
                    return BadRequest(new { message = "City name is required" });
                }

                if (cityDTO.ChargePrice < 0 || cityDTO.PickUpPrice < 0)
                {
                    return BadRequest(new { message = "Prices cannot be negative" });
                }

                if (cityDTO.GovernorateId <= 0)
                {
                    return BadRequest(new { message = "Invalid Governorate ID" });
                }

                var (isSuccess, message) = await _unitOfWork.CityService.CreateCityAsync(cityDTO);

                if (!isSuccess)
                {
                    return BadRequest(new { message });
                }

                return Created("",  message );
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while creating the city", error = ex.Message });
            }
        }
        #endregion


        #region Edit City
        [HttpPut]
        [Route("~/Cities/Edit")]
        [Authorize(Policy =
            $"Permission={Cities.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> EditCity([FromBody] EditCityDTO cityDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid city data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                if (cityDTO.Id <= 0)
                {
                    return BadRequest(new { message = "Invalid city ID" });
                }

                // Additional validation
                if (!string.IsNullOrEmpty(cityDTO.Name) && string.IsNullOrWhiteSpace(cityDTO.Name))
                {
                    return BadRequest(new { message = "City name cannot be empty or whitespace" });
                }

                if (cityDTO.ChargePrice.HasValue && cityDTO.ChargePrice < 0)
                {
                    return BadRequest(new { message = "Charge price cannot be negative" });
                }

                if (cityDTO.PickUpPrice.HasValue && cityDTO.PickUpPrice < 0)
                {
                    return BadRequest(new { message = "Pick-up price cannot be negative" });
                }

                if (cityDTO.GovernorateId.HasValue && cityDTO.GovernorateId <= 0)
                {
                    return BadRequest(new { message = "Invalid Governorate ID" });
                }

                var (isSuccess, message) = await _unitOfWork.CityService.EditCityAsync(cityDTO);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while updating the city", error = ex.Message });
            }
        }
        #endregion


        #region Delete City

        [HttpDelete]
        [Route("~/Cities/DeleteCity/{id}")]
        [Authorize(Policy =
            $"Permission={Cities.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid city ID" });
                }

                var (isSuccess, message) = await _unitOfWork.CityService.DeleteCityAsync(id);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while deleting the city", error = ex.Message });
            }
        }
        #endregion
    }
}