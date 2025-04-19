using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RejectedReasonsController : ControllerBase
    {
        private readonly IUnitOfWork _uniteOfWork;

        public RejectedReasonsController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        [HttpGet]
        [Route("~/RejectedReasons/GetAll")]
        //[Authorize(Policy = RejectedReasons.View)]
        public async Task<IActionResult> GetAllRejectedReasons([FromQuery] RejectedReasonParams param)
        {
            var result = await _uniteOfWork.RejectedReasonService.GetAllRejectedReasonsAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/RejectedReasons/GetRejectedReasonById/{id}")]
        public async Task<IActionResult> GetRejectedReasonById(int id)
        {
            var rejectedReason = await _uniteOfWork.RejectedReasonService.GetRejectedReasonByIdAsync(id);
            if (rejectedReason == null)
                return NotFound($"Rejected reason with ID {id} not found");
            
            return Ok(rejectedReason);
        }

        [HttpPost]
        [Route("~/RejectedReasons/CreateRejectedReason")]
        //[Authorize(Policy = RejectedReasons.Create)]
        public async Task<IActionResult> CreateRejectedReason([FromBody] CreateRejectedReasonDTO rejectedReasonDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _uniteOfWork.RejectedReasonService.CreateRejectedReasonAsync(rejectedReasonDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/RejectedReasons/UpdateRejectedReason/{id}")]
        //[Authorize(Policy = RejectedReasons.Edit)]
        public async Task<IActionResult> UpdateRejectedReason(int id, [FromBody] RejectedReasonDTO rejectedReasonDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _uniteOfWork.RejectedReasonService.UpdateRejectedReasonAsync(id, rejectedReasonDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/RejectedReasons/DeleteRejectedReason/{id}")]
        //[Authorize(Policy = RejectedReasons.Delete)]
        public async Task<IActionResult> DeleteRejectedReason(int id)
        {
            var result = await _uniteOfWork.RejectedReasonService.DeleteRejectedReasonAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
    }
} 