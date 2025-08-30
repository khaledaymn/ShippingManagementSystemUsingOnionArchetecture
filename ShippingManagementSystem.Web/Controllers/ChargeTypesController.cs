using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChargeTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChargeTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #region Get Charge Types

        [HttpGet]
        [Route("~/ChargeTypes/GetAll")]
        [Authorize(Policy =
            $"Permission={ChargeTypes.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetAllChargeTypes([FromQuery] ChargeTypeParams param)
        {
            try
            {
                if (param == null)
                {
                    return BadRequest("Invalid charge type parameters");
                }

                var result = await _unitOfWork.ChargeTypeService.GetAllChargeTypesAsync(param);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception here (e.g., using ILogger)
                return StatusCode(500, "An unexpected error occurred while retrieving charge types");
            }
        }

        #endregion


        #region Get Charge Type By Id

        [HttpGet]
        [Route("~/ChargeTypes/GetById/{id:int}")]
        [Authorize(Policy =
            $"Permission={ChargeTypes.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        public async Task<IActionResult> GetChargeTypeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid charge type ID");
                }

                var chargeType = await _unitOfWork.ChargeTypeService.GetChargeTypeByIdAsync(id);
                if (chargeType == null)
                {
                    return NotFound($"Charge type with ID {id} not found");
                }

                return Ok(chargeType);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, $"An error occurred while retrieving charge type with ID {id}");
            }
        }
        #endregion


        #region Create Charge Type

        [HttpPost]
        [Route("~/ChargeTypes/Create")]
        [Authorize(Policy =
            $"Permission={ChargeTypes.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> CreateChargeType([FromBody] CreateChargeTypeDTO chargeTypeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (chargeTypeDTO == null)
                {
                    return BadRequest("Charge type data is required");
                }

                var result = await _unitOfWork.ChargeTypeService.CreateChargeTypeAsync(chargeTypeDTO);

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
                return StatusCode(500, "An error occurred while creating the charge type");
            }
        }

        #endregion


        #region Update Charge Type

        [HttpPut]
        [Route("~/ChargeTypes/Update/{id:int}")]
        [Authorize(Policy =
            $"Permission={ChargeTypes.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> UpdateChargeType(int id, [FromBody] ChargeTypeDTO chargeTypeDTO)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid charge type ID");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (chargeTypeDTO == null)
                {
                    return BadRequest("Charge type data is required");
                }

                var result = await _unitOfWork.ChargeTypeService.UpdateChargeTypeAsync(id, chargeTypeDTO);

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
                return StatusCode(500, $"An error occurred while updating charge type with ID {id}");
            }
        }

        #endregion


        #region Delete Charge Type
        [HttpDelete]
        [Route("~/ChargeTypes/Delete/{id:int}")]
        [Authorize(Policy =
            $"Permission={ChargeTypes.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<IActionResult> DeleteChargeType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid charge type ID");
                }

                var result = await _unitOfWork.ChargeTypeService.DeleteChargeTypeAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, $"An error occurred while deleting charge type with ID {id}");
            }
        }

        #endregion

    }
}