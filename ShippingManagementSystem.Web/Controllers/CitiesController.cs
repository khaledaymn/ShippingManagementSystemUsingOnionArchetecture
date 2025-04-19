using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.Services;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Cities/GetAll")]
        //[Authorize(Policy = Cities.View)]
        public async Task<IActionResult> GetAllCities([FromQuery] CityParams param)
        {
            var result = await _unitOfWork.CityService.GetAllCitiesAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/Cities/GetCityById/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _unitOfWork.CityService.GetCityByIdAsync(id);
            if (city == null)
                return NotFound($"City with ID {id} not found");
            
            return Ok(city);
        }

        [HttpPost]
        [Route("~/Cities/CreateCity")]
        //[Authorize(Policy = Cities.Create)]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDTO cityDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.CityService.CreateCityAsync(cityDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/Cities/DeleteCity/{id}")]
        //[Authorize(Policy = Cities.Delete)]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var result = await _unitOfWork.CityService.DeleteCityAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/Cities/Edit")]
        //[Authorize(Policy = Cities.Edit)]
        public async Task<IActionResult> EditCity([FromBody] EditCityDTO cityDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isSuccess, result) = await _unitOfWork.CityService.EditCityAsync(cityDTO);

            if (!isSuccess)
            {
                return NotFound(new { message = "City or Governorate not found" });
            }

            return Ok(result);
        }
    }
} 