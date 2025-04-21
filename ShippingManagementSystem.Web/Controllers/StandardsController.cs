using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StandardsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StandardsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPut]
        [Route("~/Standard/Update/{id}")]
        //[Authorize(Policy = Settings.Edit)]
        public async Task<IActionResult> UpdateStandard(int id, [FromBody] UpdateStandardDTO standardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.standardServices.UpdateStandardAsync(id, standardDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpGet]
        [Route("~/Standard/GetSetting")]
        //[Authorize(Policy = Settings.View)]
        public async Task<IActionResult> GetSetting()
        {
            var standards = await _unitOfWork.standardServices.GetSettingAsync();

            if (standards == null || standards.Count == 0)
                return NotFound("No standards found");

            return Ok(standards);
        }
    }
} 