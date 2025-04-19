using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BranchesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Branches/GetAll")]
        //[Authorize(Policy = Branches.View)]
        public async Task<IActionResult> GetAllBranches([FromQuery] BranchParams param)
        {
            var result = await _unitOfWork.BranchService.GetAllBranchesAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/Branches/GetById/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _unitOfWork.BranchService.GetBranchByIdAsync(id);
            if (branch == null)
                return NotFound($"Branch with ID {id} not found");
            
            return Ok(branch);
        }

        [HttpPost]
        [Route("~/Branches/Create")]
        //[Authorize(Policy = Branches.Create)]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDTO branchDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.BranchService.CreateBranchAsync(branchDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/Branches/Update/{id}")]
        //[Authorize(Policy = Branches.Edit)]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] BranchDTO branchDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.BranchService.UpdateBranchAsync(id, branchDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/Branches/Delete/{id}")]
        //[Authorize(Policy = Branches.Delete)]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _unitOfWork.BranchService.DeleteBranchAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
    }
} 