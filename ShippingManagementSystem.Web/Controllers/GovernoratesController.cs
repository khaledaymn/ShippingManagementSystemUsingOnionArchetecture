using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.GovernorateSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernoratesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GovernoratesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Governorates/GetAll")]
        public async Task<IActionResult> GetAllGovernorates([FromQuery] GovernorateParams param)
        {
            var result = await _unitOfWork.GovernorateService.GetAllGovernoratesAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/Governorates/GetById/{id:int}")]
        public async Task<IActionResult> GetGovernorateById(int id)
        {
            var governorate = await _unitOfWork.GovernorateService.GetGovernorateByIdAsync(id);
            if (governorate == null)
                return NotFound($"Governorate with ID {id} not found");
            
            return Ok(governorate);
        }

        [HttpPost]
        [Route("~/Governorates/Create")]
        public async Task<IActionResult> CreateGovernorate([FromBody] CreateGovernorateDTO governorateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.GovernorateService.CreateGovernorateAsync(governorateDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/Governorates/Update/{id:int}")]
        public async Task<IActionResult> UpdateGovernorate(int id, [FromBody] GovernorateDTO governorateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.GovernorateService.UpdateGovernorateAsync(id, governorateDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/Governorates/Delete/{id:int}")]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            var result = await  _unitOfWork.GovernorateService.DeleteGovernorateAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
    }
} 