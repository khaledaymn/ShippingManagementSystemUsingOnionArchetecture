using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StandardsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Standard/GetAll")]
        public async Task<IActionResult> GetAllStandards()
        {
            var standards = await _unitOfWork.standardServices.GetAllStandardsAsync();
            return Ok(standards);
        }


        [HttpPost]
        [Route("~/Standard/Create")]
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