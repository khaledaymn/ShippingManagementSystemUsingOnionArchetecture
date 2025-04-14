using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Cities/GetAll")]
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
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDTO cityDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.CityService.CreateCityAsync(cityDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/Cities/UpdateCity/{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] CityDTO cityDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.CityService.UpdateCityAsync(id, cityDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/Cities/DeleteCity/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var result = await _unitOfWork.CityService.DeleteCityAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
    }
} 