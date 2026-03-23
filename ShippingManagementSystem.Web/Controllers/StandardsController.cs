using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.StandardExamples.Requests;
using ShippingManagementSystem.Web.Swagger.StandardExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.StandardExamples.Responses.Success;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StandardsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StandardsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPut]
        [Route("~/Standard/Update/{id}")]
        [Authorize(Policy =
            $"Permission={Settings.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PutUpdateShippingStandard",
            Summary = "Modify global shipping pricing and weight rules",
            Description = "### ⚖️ Global Pricing Calibration\n\n" +
                          "Updates the core mathematical constants used for order cost calculation. This endpoint supports **Delta Updates**, meaning only the fields you provide will be modified.\n\n" +
                          "#### **⚙️ Operational Logic:**\n" +
                          "* **Weight Threshold**: Adjusting `StandardWeight` redefines the 'Free/Base Weight' for all new orders.\n" +
                          "* **Extra KG Rate**: `KGprice` determines the penalty fee for overweight shipments.\n" +
                          "* **Rural Surcharge**: `VillagePrice` sets the fixed overhead for remote area deliveries.\n\n" +
                          "#### **🛡️ Validation & Persistence:**\n" +
                          "* **Null Safety**: Fields sent as `null` are ignored, preserving existing values in the database.\n" +
                          "* **Data Integrity**: Uses `standard.Id` to ensure the correct configuration record is targeted.\n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Standards successfully synchronized. | `'Standard updated successfully'` |\n" +
                          "| **400 Bad** | Provided ID does not exist or invalid numeric data. | `'Standard with id {id} not found'` |\n" +
                          "| **500 Err** | Infrastructure or Database write failure. | `'Error updating standard: {msg}'` |\n\n" +
                          "> **Warning:** Changes here affect the pricing of ALL orders created after this update. Existing orders retain their original calculated costs.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(UpdateStandardDTO), typeof(UpdateStandardRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(StandardUpdateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(StandardUpdateErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupByIdErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policy =
            $"Permission={Settings.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetShippingStandards",
            Summary = "Retrieve global pricing and weight configuration",
            Description = "### 📋 Global Logistics Configuration\n\n" +
                          "Fetches the active shipping standards that govern cost calculations across the entire platform. This is a **Shared Reference** used by Admins, Merchants, and Couriers.\n\n" +
                          "#### **⚙️ Key Configuration Points:**\n" +
                          "* **Base Weight**: The standard weight (KG) included in the initial shipping fee.\n" +
                          "* **Overweight Penalty**: The cost per extra KG beyond the base limit.\n" +
                          "* **Rural Surcharge**: Extra fees applied for deliveries outside urban centers.\n\n" +
                          "#### **👥 Multi-Role Utility:**\n" +
                          "* **Admin**: For auditing and reviewing current pricing policies.\n" +
                          "* **Merchant**: For estimating shipping costs before dispatching orders.\n" +
                          "* **Representative**: For validating weight-based charges during pickup.\n\n" +
                          "#### **⚠️ Response Matrix (Plain Text & JSON):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Configuration found and returned. | `List<StandardDTO> (JSON)` |\n" +
                          "| **404 Not** | Standards table is empty or record not found. | `'No standards found' (String)` |\n" +
                          "| **500 Err** | Infrastructure or Database connectivity failure. | `'Error retrieving standards: {msg}' (String)` |\n",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetSettingsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(SettingsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupByIdErrorExample))]
        [ProducesResponseType(typeof(List<StandardDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSetting()
        {
            var standards = await _unitOfWork.standardServices.GetSettingAsync();

            if (standards == null || standards.Count == 0)
                return NotFound("No standards found");

            return Ok(standards);
        }
    }
} 