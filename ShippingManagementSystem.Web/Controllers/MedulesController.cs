using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.MeduleSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MedulesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedulesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Medule/GetAll")]
        public async Task<IActionResult> GetAllMedules([FromQuery] MeduleParams param)
        {
            var result = await _unitOfWork.MeduleService.GetAllMedulesAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/Medule/GetById")]
        public async Task<IActionResult> GetMeduleById(int id)
        {
            var medule = await _unitOfWork.MeduleService.GetMeduleByIdAsync(id);
            if (medule == null)
                return NotFound($"Module with ID {id} not found");
            
            return Ok(medule);
        }
        

    }
} 