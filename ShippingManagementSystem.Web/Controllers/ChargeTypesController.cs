using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests;
using ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Success;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
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
        [SwaggerOperation(
            OperationId = "GetChargeTypesList",
            Summary = "Retrieve a list of delivery speed tiers and pricing",
            Description = "### ⏱️ Shipping Speed & Cost Configuration\n\n" +
                          "This endpoint manages the different service levels available for shipping. Each 'Charge Type' defines a delivery window and an associated premium price.\n\n" +
                          "#### **⚙️ Search & Range Filtering:**\n" +
                          "* **Price Range**: Filter tiers by their `ExtraPrice` (Min/Max).\n" +
                          "* **Duration Range**: Search for services based on the number of delivery days (`MinDays`/`MaxDays`).\n" +
                          "* **Status Filtering**: Filter by `IsDeleted` to see active or archived service tiers.\n\n" +
                          "#### **📊 Operational Logic:**\n" +
                          "* **Premium Calculation**: The `ExtraPrice` is added to the base city-to-city shipping cost during order creation.\n" +
                          "* **Performance**: The list is paginated, allowing up to **100 records** per page to cover all possible service variations.\n\n" +
                          "#### **⚠️ Error Handling (Plain Text):**\n" +
                          "Returns a raw string message for the following:\n" +
                          "| Status | Scenario | Sample Message |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **400** | Invalid parameters or null request. | `Invalid charge type parameters` |\n" +
                          "| **400** | Model validation failure. | `Validation error: {message}` |\n" +
                          "| **500** | Unexpected system failure. | `An unexpected error occurred...` |\n\n" +
                          "> **Context:** Essential for populating 'Delivery Speed' dropdowns in the Merchant checkout portal.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(ChargeTypeParams), typeof(ChargeTypeParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllChargeTypesSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ChargeTypeBadRequestStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<ChargeTypeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "GetChargeTypeDetails",
            Summary = "Fetch specific shipping tier details",
            Description = "### 🔍 Precision Logistics Configuration Lookup\n\n" +
                      "Retrieves the granular metadata of a specific delivery tier. This endpoint is a **Critical Dependency** for order validation engines and checkout interfaces, as it provides the specific financial and temporal constants required to calculate delivery expectations.\n\n" +
                      "#### **⚙️ Strategic Data Points:**\n" +
                      "* **Cost Premium (`ExtraPrice`)**: The absolute monetary value to be added to the base shipping rate. This is the key variable in the final invoice calculation.\n" +
                      "* **Time Commitment (`NumOfDay`)**: Defines the service level agreement (SLA) for delivery, used by front-end applications to display 'Expected Arrival' dates to the end-user.\n\n" +
                      "#### **🛡️ Response & Reliability Matrix:**\n" +
                      "This endpoint ensures deterministic behavior by returning raw string messages for all failure states to facilitate rapid error parsing:\n\n" +
                      "| Status | Scenario | Technical Reason | Output Format |\n" +
                      "| :--- | :--- | :--- | :--- |\n" +
                      "| **200 OK** | Success | Entity found and mapped. | `ChargeTypeDTO (JSON)` |\n" +
                      "| **400 Bad** | Invalid Input | ID provided is non-positive (<= 0). | `Plain Text String` |\n" +
                      "| **404 Not** | Missing Record | ID does not exist in the active or archived set. | `Plain Text String` |\n" +
                      "| **500 Err** | Infrastructure | Database connection or mapping exception. | `Plain Text String` |\n\n" +
                      "> **Implementation Note:** This data is often cached on the client side to minimize redundant network calls during high-traffic checkout sessions.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetChargeTypeByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ChargeTypeNotFoundStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ChargeTypeInternalErrorStringExample))]
        [ProducesResponseType(typeof(ChargeTypeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "PostCreateChargeType",
            Summary = "Define a new shipping service level",
            Description = "### 🏗️ Service Catalog Expansion\n\n" +
                          "This endpoint registers a new delivery tier within the logistics framework. Each Charge Type act as a unique product offering that balances **Delivery Speed** against **Service Premium**.\n\n" +
                          "#### **🛡️ Critical Business Rules:**\n" +
                          "* **Uniqueness Constraint**: The system performs a case-insensitive check on the `Name`. Duplicate service names are rejected to prevent configuration ambiguity.\n" +
                          "* **Financial Sanity**: `ExtraPrice` and `NumOfDay` must be greater than zero. The system rejects free or zero-day services through this specific portal.\n\n" +
                          "#### **🔒 Internal Execution Workflow:**\n" +
                          "1. **Validation Phase**: Checks `ModelState` and ensures the payload is not null. \n" +
                          "2. **Conflict Detection**: Queries the repository using an `Any()` specification to ensure name uniqueness. \n" +
                          "3. **State Management**: New tiers are automatically set to an active state (`IsDeleted = false`). \n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text Responses):**\n" +
                          "| Status | Scenario | Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **400 Bad** | Data Validation | Missing Name or Negative Values. | `Validation error: {msg}` |\n" +
                          "| **400 Bad** | Logic Conflict | Service name already exists. | `Charge type '{name}' already exists` |\n" +
                          "| **500 Err** | Infrastructure | Database persistence failure. | `An error occurred while creating...` |\n\n" +
                          "> **Note:** Successful creation returns a **200 OK** with a confirmation string. This tier becomes immediately available for order assignment.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(CreateChargeTypeDTO), typeof(CreateChargeTypeRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ChargeTypeCreateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ChargeTypeCreateErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ChargeTypeInternalErrorStringExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "PutUpdateChargeType",
            Summary = "Modify shipping tier pricing or duration",
            Description = "### 🔄 Dynamic Service Level Adjustment\n\n" +
                          "Updates the operational parameters of an existing shipping tier. This endpoint implements a **Smart Delta Update** logic, ensuring that only explicitly provided and valid data is persisted.\n\n" +
                          "#### **⚙️ Update Mechanics & Guardrails:**\n" +
                          "* **Textual Integrity**: The `Name` is only modified if a non-empty string is provided.\n" +
                          "* **Numeric Safety**: For `ExtraPrice` and `NumOfDay`, the system interprets values **<= 0** as a signal to **retain the current database value**. This prevents accidental zeroing of financial data.\n" +
                          "* **Nullable Types**: Fields set to `null` in the payload are ignored during the mapping phase.\n\n" +
                          "#### **🛡️ Reliability Matrix (Plain Text Responses):**\n" +
                          "| Status | Scenario | Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success | Entity properties synchronized. | `'{name}' updated successfully` |\n" +
                          "| **400 Bad** | Validation | ModelState failure or negative ID. | `Invalid charge type ID` |\n" +
                          "| **400 Bad** | Missing Entity | ID does not exist in the system. | `Charge type with id {id} not found` |\n" +
                          "| **500 Err** | System | Transaction or DB mapping failure. | `An error occurred while updating...` |\n\n" +
                          "> **Warning:** Changes to `ExtraPrice` will reflect immediately on all new orders created after the update, but will not affect already 'Placed' orders.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(ChargeTypeDTO), typeof(UpdateChargeTypeRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ChargeTypeUpdateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ChargeTypeUpdateNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ChargeTypeInternalErrorStringExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "DeleteChargeTypeStatus",
            Summary = "Toggle charge type availability (Soft-Delete)",
            Description = "### 🛡️ Financial Data Preservation\n\n" +
                          "Performs a **Soft-Delete (State Toggle)** on a specific shipping tier. This endpoint is designed to prevent data corruption in historical order records while allowing administrators to retire service levels.\n\n" +
                          "#### **⚙️ Operational Logic:**\n" +
                          "* **Toggle Behavior**: If the tier is active, it becomes 'Deleted' (hidden from new orders). If it's already 'Deleted', this action restores it to the active catalog.\n" +
                          "* **Integrity Lock**: Since many orders are linked to these tiers for pricing, a **Hard Delete** is prohibited to maintain audit trails.\n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text Responses):**\n" +
                          "| Status | Scenario | Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success | Status inverted successfully. | `'Charge type {name} deleted successfully'` |\n" +
                          "| **400 Bad** | Validation | Provided ID is non-positive. | `'Invalid charge type ID'` |\n" +
                          "| **404 Not** | Missing | ID does not exist in the database. | `'Charge type with id {id} not found'` |\n" +
                          "| **500 Err** | System | Internal transaction failure. | `'An error occurred while deleting...'` |\n\n" +
                          "> **Tip:** Use this to temporarily disable a shipping tier (e.g., suspending 'Next Day Delivery' during peak holidays).",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ChargeTypeDeleteSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ChargeTypeDeleteNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ChargeTypeDeleteBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ChargeTypeInternalErrorStringExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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