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

        [HttpGet]
        [Route("~/Standard/GetAll")]
        //[Authorize(Policy = Settings.View)]
        public async Task<IActionResult> GetAllStandards()
        {
            var standards = await _unitOfWork.standardServices.GetAllStandardsAsync();
            return Ok(standards);
        }


        [HttpPost]
        [Route("~/Standard/Create")]
        //[Authorize(Policy = Settings.Create)]
        public async Task<IActionResult> CreateStandard([FromBody] CreateStandardDTO standardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.standardServices.CreateStandardAsync(standardDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
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
    }
} 