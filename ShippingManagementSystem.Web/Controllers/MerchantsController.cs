using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDtos;
using ShippingManagementSystem.Domain.DTOs.MerchantDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MerchantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get All Merchants

        [HttpGet]
        [Route("~/Merchants/GetAll")]
        [Authorize(Policy =
            $"Permission={Merchants.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> GetAll([FromQuery] MerchantParams param)
        {
            try
            {
                var result = await _unitOfWork.MerchantService.GetAllMerchantsAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion


        #region Add Merchant

        [HttpPost]
        [Route("~/Merchants/Add")]
        [Authorize(Policy =
            $"Permission={Merchants.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> Add(AddMerchantDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (isSuccess, message) = await _unitOfWork.MerchantService.AddMerchantAsync(dto);
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


        #region Update Merchant

        [HttpPut]
        [Route("~/Merchants/Update")]
        [Authorize(Policy =
            $"Permission={Merchants.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> Update(UpdateMerchantDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
                    return BadRequest(new { Message = "Invalid merchant data." });

                var (isSuccess, message) = await _unitOfWork.MerchantService.UpdateMerchantAsync(dto);
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


        #region Delete Merchant

        [HttpDelete]
        [Route("~/Merchants/Delete/{id}")]
        [Authorize(Policy =
            $"Permission={Merchants.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var (isSuccess, message) = await _unitOfWork.MerchantService.DeleteMerchantAsync(id);
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


        #region Get Merchant By Id

        [HttpGet]
        [Route("~/Merchants/GetById/{id}")]
        [Authorize(Policy =
            $"Permission={Merchants.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        public async Task<ActionResult<MerchantDTO>> GetById(string id)
        {
            try
            {
                var merchant = await _unitOfWork.MerchantService.GetMerchantByIdAsync(id);

                if (merchant == null)
                    return NotFound(new { Message = "Merchant not found." });

                return Ok(merchant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

    }
}
