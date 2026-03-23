using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification;
using ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Requests;
using ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Success;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RejectedReasonsController : ControllerBase
    {
        private readonly IUnitOfWork _uniteOfWork;

        public RejectedReasonsController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        #region Get All Rejected Reasons
        [HttpGet]
        [Route("~/RejectedReasons/GetAll")]
        //[Authorize(Policy = RejectedReasons.View)]
        [SwaggerOperation(
            OperationId = "GetRejectedReasonsList",
            Summary = "Retrieve a paginated list of delivery failure reasons",
            Description = "### 🚫 Standardized Rejection Catalog\n\n" +
                          "This endpoint provides the master list of reasons used to justify delivery failures or order rejections. It acts as the **Data Authority** for the shipping representative's mobile application.\n\n" +
                          "#### **⚙️ Query & Search Logic:**\n" +
                          "* **Full-Text Search**: Use the `Search` parameter to query within the reason strings (e.g., 'Refused').\n" +
                          "* **State Filtering**: Toggle `IsDeleted` to see legacy reasons or only current active ones.\n" +
                          "* **Sorting Tier**: Order results by text or ID to maintain a consistent UI layout.\n\n" +
                          "#### **📊 Performance & Pagination:**\n" +
                          "* **Bulk Retrieval**: Supports a high `PageSize` (up to **100**) to allow admin panels to load the entire configuration at once.\n" +
                          "* **Zero-Lag Architecture**: Uses optimized specifications to count and fetch records in parallel.\n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Response Body |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **400 BadRequest** | Null or malformed params. | `'Invalid rejected reason parameters'` |\n" +
                          "| **500 Internal** | Database/Service failure. | `'An unexpected error occurred...'` |\n\n" +
                          "> **Business Context:** These reasons are mandatory for any order transitioning to 'Failed' or 'Rejected' status.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(RejectedReasonParams), typeof(RejectedReasonParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllRejectedReasonsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RejectedReasonBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(RejectedReasonInternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<RejectedReasonDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRejectedReasons([FromQuery] RejectedReasonParams param)
        {
            var result = await _uniteOfWork.RejectedReasonService.GetAllRejectedReasonsAsync(param);
            return Ok(result);
        }
        #endregion

        #region Get Rejected Reason By ID
        [HttpGet]
        [Route("~/RejectedReasons/GetRejectedReasonById/{id}")]
        [SwaggerOperation(
            OperationId = "GetRejectedReasonDetails",
            Summary = "Fetch a specific rejection reason by its ID",
            Description = "### 🔍 Deep Reason Inspection\n\n" +
                          "Retrieves the full metadata of a single rejection reason. This is a **Lookup Dependency** used primarily in order history logs and customer service dashboards to explain why a specific delivery failed.\n\n" +
                          "#### **⚙️ Data Resolution Logic:**\n" +
                          "* **Entity Lookup**: Performs a direct primary key search in the `RejectedReasons` table.\n" +
                          "* **Mapping**: Projects the database entity into a clean `RejectedReasonDTO` for the UI.\n\n" +
                          "#### **🛡️ Response & Failure Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Technical Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success | Reason found and mapped. | `RejectedReasonDTO (JSON)` |\n" +
                          "| **400 Bad** | Invalid Input | ID is non-positive (<= 0). | `'Invalid rejected reason ID'` |\n" +
                          "| **404 Not** | Missing | ID doesn't exist in the system. | `'Rejected reason with ID {id} not found'` |\n" +
                          "| **500 Err** | Infrastructure | Exception during DB execution. | `'An unexpected error occurred...'` |\n\n" +
                          "> **Business Context:** Crucial for 'Order Return' workflows where the specific rejection text must be displayed to the Merchant.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetRejectedReasonByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(RejectedReasonNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(RejectedReasonByIdInternalErrorExample))]
        [ProducesResponseType(typeof(RejectedReasonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRejectedReasonById(int id)
        {
            var rejectedReason = await _uniteOfWork.RejectedReasonService.GetRejectedReasonByIdAsync(id);
            if (rejectedReason == null)
                return NotFound($"Rejected reason with ID {id} not found");
            
            return Ok(rejectedReason);
        }
        #endregion

        #region Create Rejected Reason
        [HttpPost]
        [Route("~/RejectedReasons/CreateRejectedReason")]
        //[Authorize(Policy = RejectedReasons.Create)]
        [SwaggerOperation(
            OperationId = "PostCreateRejectedReason",
            Summary = "Register a new delivery rejection reason",
            Description = "### 🏗️ Rejection Catalog Onboarding\n\n" +
                          "Adds a new standardized reason to the system's rejection library. This text is **Mission Critical** as it provides the legal and operational justification for returning a shipment to the merchant.\n\n" +
                          "#### **🛡️ Validation Guardrails:**\n" +
                          "* **Text Length**: Must be provided and cannot exceed **500 characters** to ensure compatibility with mobile UI layouts.\n" +
                          "* **ModelState Validation**: Automatically triggers a **400 BadRequest** if the text is missing or exceeds length limits.\n\n" +
                          "#### **📊 Tactical Workflow:**\n" +
                          "1. **Input Shielding**: Checks if the `CreateRejectedReasonDTO` is valid.\n" +
                          "2. **Auto-Initialization**: New reasons are created with `IsDeleted = false` by default.\n" +
                          "3. **Persistence**: Saves the record and returns a success manifest string.\n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Technical Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success | Reason added to the catalog. | `'Rejected reason created successfully'` |\n" +
                          "| **400 Bad** | Model Error | Text is null or exceeds 500 chars. | `ModelState Error (JSON)` |\n" +
                          "| **400 Bad** | Logic Error | Service failed to persist data. | `'Error creating rejected reason: {msg}'` |\n" +
                          "| **500 Err** | System | Database or mapping exception. | `'An unexpected error occurred...'` |\n\n" +
                          "> **Best Practice:** Keep the text concise and professional, as merchants will see this on their 'Returned Orders' reports.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(CreateRejectedReasonDTO), typeof(CreateRejectedReasonRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RejectedReasonCreateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RejectedReasonCreateBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(RejectedReasonCreateInternalErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRejectedReason([FromBody] CreateRejectedReasonDTO rejectedReasonDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _uniteOfWork.RejectedReasonService.CreateRejectedReasonAsync(rejectedReasonDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }
        #endregion

        #region Update Rejected Reason
        [HttpPut]
        [Route("~/RejectedReasons/UpdateRejectedReason/{id}")]
        //[Authorize(Policy = RejectedReasons.Edit)]
        [SwaggerOperation(
            OperationId = "PutUpdateRejectedReason",
            Summary = "Modify an existing rejection reason text",
            Description = "### 🔄 Service Content Refinement\n\n" +
                          "Updates the descriptive text of a specific rejection reason. This endpoint utilizes a **Conditional Update Strategy** to ensure data persistence integrity.\n\n" +
                          "#### **⚙️ Update Mechanics:**\n" +
                          "* **Textual Delta**: The `Text` property is only overwritten if the provided string is not null or empty. \n" +
                          "* **Identifier Binding**: The `id` in the URL must match the target record in the database.\n\n" +
                          "#### **🛡️ Reliability & Response Matrix:**\n" +
                          "| Status | Scenario | Technical Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success | Text updated in the catalog. | `'Rejected reason updated successfully'` |\n" +
                          "| **400 Bad** | Validation | Text exceeds 500 characters. | `ModelState Error (JSON)` |\n" +
                          "| **400 Bad** | Missing | ID does not exist in the database. | `'Rejected reason with id {id} not found'` |\n" +
                          "| **500 Err** | Infrastructure | Database update transaction failed. | `'Error updating rejected reason: {msg}'` |\n\n" +
                          "> **Note:** Updating a reason affects all historical records linked to this ID in the reports. Use with caution for significant meaning changes.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(RejectedReasonDTO), typeof(UpdateRejectedReasonRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RejectedReasonUpdateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RejectedReasonUpdateBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(RejectedReasonUpdateInternalErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRejectedReason(int id, [FromBody] RejectedReasonDTO rejectedReasonDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _uniteOfWork.RejectedReasonService.UpdateRejectedReasonAsync(id, rejectedReasonDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }
        #endregion

        #region Delete Rejected Reason
        [HttpDelete]
        [Route("~/RejectedReasons/DeleteRejectedReason/{id}")]
        //[Authorize(Policy = RejectedReasons.Delete)]
        [SwaggerOperation(
            OperationId = "DeleteRejectedReasonRecord",
            Summary = "Permanently remove a rejection reason",
            Description = "### ⚠️ Permanent Data Erasure (Hard Delete)\n\n" +
                          "This endpoint performs a **Physical Delete** of the specified rejection reason from the database. Unlike soft-delete, this action is irreversible.\n\n" +
                          "#### **🚫 Operational Constraints:**\n" +
                          "* **Referential Integrity**: If this reason ID is currently linked to any historical 'Rejected' or 'Returned' orders, the database may throw a **Foreign Key Constraint** error (500).\n" +
                          "* **Audit Impact**: Deleting a reason will remove the descriptive context from any associated reporting logs that rely on direct joins.\n\n" +
                          "#### **🛡️ Response Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Technical Reason | Output |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success | Record purged from the database. | `'Rejected reason deleted successfully'` |\n" +
                          "| **404 Not** | Missing | ID does not exist in the catalog. | `'Rejected reason with id {id} not found'` |\n" +
                          "| **500 Err** | Infrastructure | Foreign key conflict or DB failure. | `'Error deleting rejected reason: {msg}'` |\n\n" +
                          "> **Recommendation:** It is highly advised to use this only for correcting data entry errors. For retiring valid reasons, consider implementing a Soft-Delete instead.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RejectedReasonDeleteSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(RejectedReasonDeleteNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(RejectedReasonDeleteInternalErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRejectedReason(int id)
        {
            var result = await _uniteOfWork.RejectedReasonService.DeleteRejectedReasonAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
        #endregion
    }
} 