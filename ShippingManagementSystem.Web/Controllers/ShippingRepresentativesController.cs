using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Exptions;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class ShippingRepresentativesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShippingRepresentativesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #region Get All Shipping Representatives

        [HttpGet]
        [Route("~/ShippingRepresentatives/GetAll")]
        //[Authorize(Policy = ShippingRepresentatives.View)]
        public async Task<ActionResult<PaginationResponse<ShippingRepresentativeDTO>>> GetAll([FromQuery] ShippingRepresentativeParams param)
        {
            try
            {
                var result = await _unitOfWork.ShippingRepresentativeServices.GetAllShippingRepresentativesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Get Shipping Representative By Id

        [HttpGet]
        [Route("~/ShippingRepresentatives/GetById/{id}")]
        public async Task<ActionResult<ShippingRepresentativeDTO>> GetById(string id)
        {
            try
            {
                var shippingRepresentative = await _unitOfWork.ShippingRepresentativeServices.GetShippingRepresentativeByIdAsync(id);
                if (shippingRepresentative == null)
                    return NotFound(new { Message = "Shipping representative not found." });

                return Ok(shippingRepresentative);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Add Shipping Representative

        [HttpPost]
        [Route("~/ShippingRepresentatives/Add")]
        //[Authorize(Policy = ShippingRepresentatives.Create)]
        public async Task<ActionResult> Add(AddShippingRepresentativeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (isSuccess, message) = await _unitOfWork.ShippingRepresentativeServices.AddShippingRepresentativeAsync(dto);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return CreatedAtAction(nameof(GetAll), null, new { Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Update Shipping Representative

        [HttpPut]
        [Route("~/ShippingRepresentatives/Update")]
        //[Authorize(Policy = ShippingRepresentatives.Edit)]
        public async Task<ActionResult> Update(UpdateShippingRepresentativeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
                    return BadRequest(new { Message = "Invalid shipping representative data." });

                var (isSuccess, message) = await _unitOfWork.ShippingRepresentativeServices.UpdateShippingRepresentativeAsync(dto);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(new { Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Delete Shipping Representative

        [HttpDelete]
        [Route("~/ShippingRepresentatives/Delete/{id}")]
        //[Authorize(Policy = ShippingRepresentatives.Delete)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var (isSuccess, message) = await _unitOfWork.ShippingRepresentativeServices.DeleteShippingRepresentativeAsync(id);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(new { Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

    }
}