using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BranchesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public BranchesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        
        #region GetAllBranches

        [HttpGet]
        [Route("~/Branches/GetAll")]
        [Authorize(Policy =
            $"Permission={Branches.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetAllBranches([FromQuery] BranchParams param)
        {
            try
            {
                if (param == null)
                {
                    return BadRequest("Invalid branch parameters");
                }

                var result = await _unitOfWork.BranchService.GetAllBranchesAsync(param);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception here (e.g., using ILogger)
                return StatusCode(500, "An unexpected error occurred while retrieving branches");
            }
        }

        #endregion


        #region GetBranchById

        [HttpGet]
        [Route("~/Branches/GetById/{id}")]
        [Authorize(Policy =
            $"Permission={Branches.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid branch ID");
                }

                var branch = await _unitOfWork.BranchService.GetBranchByIdAsync(id);
                if (branch == null)
                {
                    return NotFound($"Branch with ID {id} not found");
                }

                return Ok(branch);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, $"An error occurred while retrieving branch with ID {id}");
            }
        }

        #endregion


        #region CreateBranch

        [HttpPost]
        [Route("~/Branches/Create")]
        [Authorize(Policy =
            $"Permission={Branches.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDTO branchDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (branchDTO == null)
                {
                    return BadRequest("Branch data is required");
                }

                var result = await _unitOfWork.BranchService.CreateBranchAsync(branchDTO);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "An error occurred while creating the branch");
            }
        }
        #endregion


        #region UpdateBranch

        [HttpPut]
        [Route("~/Branches/Update/{id}")]
        [Authorize(Policy =
            $"Permission={Branches.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] BranchDTO branchDTO)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid branch ID");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (branchDTO == null)
                {
                    return BadRequest("Branch data is required");
                }

                var result = await _unitOfWork.BranchService.UpdateBranchAsync(id, branchDTO);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, $"An error occurred while updating branch with ID {id}");
            }
        }

        #endregion


        #region DeleteBranch

        [HttpDelete]
        [Route("~/Branches/Delete/{id}")]
        [Authorize(Policy =
            $"Permission={Branches.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid branch ID");
                }

                var result = await _unitOfWork.BranchService.DeleteBranchAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, $"An error occurred while deleting branch with ID {id}");
            }
        }
        #endregion


        #region DeleteBranch

        [HttpDelete]
        [Route("~/Branches/HardDelete/{id}")]
        [Authorize(Policy =
            $"Permission={Branches.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid branch ID");
                }

                var result = await _unitOfWork.BranchService.Delete(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, $"An error occurred while deleting branch with ID {id}");
            }
        }
        #endregion

    }
}