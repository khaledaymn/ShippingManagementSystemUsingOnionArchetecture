using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargeTypesController : ControllerBase
    {
        private readonly IUnitOfWork _uniteOfWork;

        public ChargeTypesController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        [HttpGet]
        [Route("~/ChargeTypes/GetAll")]
        public async Task<IActionResult> GetAllChargeTypes([FromQuery] ChargeTypeParams param)
        {
            var result = await _uniteOfWork.ChargeTypeService.GetAllChargeTypesAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/ChargeTypes/GetById/{id:int}")]
        public async Task<IActionResult> GetChargeTypeById(int id)
        {
            var chargeType = await _uniteOfWork.ChargeTypeService.GetChargeTypeByIdAsync(id);
            if (chargeType == null)
                return NotFound($"Charge type with ID {id} not found");
            
            return Ok(chargeType);
        }

        [HttpPost]
        [Route("~/ChargeTypes/Create")]
        public async Task<IActionResult> CreateChargeType([FromBody] CreateChargeTypeDTO chargeTypeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _uniteOfWork.ChargeTypeService.CreateChargeTypeAsync(chargeTypeDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/ChargeTypes/Update/{id:int}")]
        public async Task<IActionResult> UpdateChargeType(int id, [FromBody] ChargeTypeDTO chargeTypeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _uniteOfWork.ChargeTypeService.UpdateChargeTypeAsync(id, chargeTypeDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/ChargeTypes/Delete/{id:int}")]
        public async Task<IActionResult> DeleteChargeType(int id)
        {
            var result = await _uniteOfWork.ChargeTypeService.DeleteChargeTypeAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
    }
} 