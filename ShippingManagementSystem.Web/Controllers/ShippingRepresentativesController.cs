using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Exptions;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.Schemas;
using ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Requests;
using ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Responses.Success;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
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
        [Authorize(Policy =
            $"Permission={ShippingRepresentatives.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "GetShippingRepsList",
            Summary = "Retrieve a paginated list of shipping representatives with fleet details",
            Description = "### 🚛 Fleet Management & Representative Directory\n\n" +
                          "This endpoint provides access to the delivery personnel directory. It aggregates base identity data with specialized commission structures and assigned operational territories.\n\n" +
                          "#### **⚙️ Data Resolution Logic:**\n" +
                          "| Layer | Resolved Information |\n" +
                          "| :--- | :--- |\n" +
                          "| **Identity Layer** | Name, Contact (Email/Phone), Residential Address, and Hiring Date. |\n" +
                          "| **Logistics Layer** | Commission Format (`DiscountType`) and Financial Rate (`CompanyPercentage`). |\n" +
                          "| **Coverage Layer** | A collection of Governorate names assigned for delivery operations. |\n\n" +
                          "#### **🔒 Internal Execution Workflow:**\n" +
                          "1. **Parameter Binding**: Processes `ShippingRepresentativeParams` including search terms and regional filters. \n" +
                          "2. **Specification Pattern**: Uses `ShippingRepresentativeSpecification` to build dynamic queries with Eager Loading for `User` and `ShippingRepGovernorates`. \n" +
                          "3. **Projection**: Transforms internal entities into a clean `ShippingRepresentativeDTO`. \n" +
                          "4. **Pagination Wrapper**: Returns the results within a standard metadata envelope.\n\n" +
                          "> **Note:** The `PageSize` is strictly clamped to a maximum of 10 to ensure performance stability.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerRequestExample(typeof(ShippingRepresentativeParams), typeof(ShippingRepParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllShippingRepsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GetShippingRepsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<ShippingRepresentativeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policy =
            $"Permission={ShippingRepresentatives.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "GetShippingRepProfile",
            Summary = "Fetch detailed profile of a shipping representative",
            Description = "### 🔍 Representative Profile Inspection\n\n" +
                          "Retrieves a complete profile of a delivery personnel member by merging identity data with logistics-specific configurations.\n\n" +
                          "#### **⚙️ Information Architecture:**\n" +
                          "| Component | Source | Details Captured |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Core Identity** | `AspNetUsers` | Legal Name, Contact Info, Address, and Hiring Date. |\n" +
                          "| **Financial Logic** | `ShippigRepresentative` | Commission Type (`Fixed`/`Percentage`) and assigned rate. |\n" +
                          "| **Service Area** | `ShippingRepGovernorates` | List of geographical territories (Governorates) assigned to the representative. |\n\n" +
                          "#### **🔒 Internal Retrieval Workflow:**\n" +
                          "1. **Primary Search**: Invokes `ShippingRepresentativeSpecification` to locate the record using the Unique ID. \n" +
                          "2. **Relationship Resolution**: Executes Eager Loading for the `User` navigation property and the `Governorates` collection. \n" +
                          "3. **Data Flattening**: Transforms the multi-table result into a consolidated `ShippingRepresentativeDTO`.\n\n" +
                          "> **Note:** If the ID does not match any active or inactive representative, a **404 Not Found** response is returned.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetShippingRepByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ShippingRepNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(ShippingRepresentativeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policy =
            $"Permission={ShippingRepresentatives.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PostCreateShippingRep",
            Summary = "Onboard a new delivery representative to the fleet",
            Description = "### 🏗️ Representative Onboarding Lifecycle\n\n" +
                          "This endpoint orchestrates the creation of a shipping representative account. It uses a **Database Transaction** to ensure a multi-step atomic operation involving identity, roles, and geographical assignments.\n\n" +
                          "#### **⚙️ Transactional Steps:**\n" +
                          "1. **Identity Provisioning**: Generates a unique `UserName` (handling collisions with a random suffix) and creates the `ApplicationUser` account.\n" +
                          "2. **Role Authorization**: Assigns the `ShippingRepresentative` security role for access control.\n" +
                          "3. **Logistics Profile**: Creates the core `ShippigRepresentative` record with commission settings (`DiscountType`/`CompanyPercentage`).\n" +
                          "4. **Territory Mapping**: Links the representative to multiple operational regions in the `ShippingRepGovernorate` junction table.\n\n" +
                          "#### **🛡️ Safety & Rollback Logic:**\n" +
                          "* **Atomicity**: If `userManager.CreateAsync` fails or any database error occurs during the governorate assignment, the transaction is **Rolled Back** immediately.\n" +
                          "* **Collision Handling**: Automatically appends a random numeric suffix to the `UserName` if a representative with the same name already exists.\n\n" +
                          "> **Note:** A successful commit returns a **201 Created** status, signaling the representative is now ready for order assignment.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerRequestExample(typeof(AddShippingRepresentativeDTO), typeof(AddShippingRepRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(AddShippingRepSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AddShippingRepBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policy =
            $"Permission={ShippingRepresentatives.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PutUpdateShippingRep",
            Summary = "Update representative profile and territorial coverage",
            Description = "### 🔄 Representative Synchronization Engine\n\n" +
                          "Updates an existing shipping representative's identity and logistics configuration. The endpoint is designed to handle **Incremental Updates** for profile data and **Append-Only Logic** for regional coverage.\n\n" +
                          "#### **⚙️ Update Strategies:**\n" +
                          "| Layer | Strategy | Implementation Details |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Identity Layer** | Null-Coalescing | Only non-null fields (Name, Email, Phone, Address) are updated via `UserManager`. |\n" +
                          "| **Logistics Layer** | Direct Override | Updates `DiscountType` and `CompanyPercentage` if provided in the payload. |\n" +
                          "| **Territory Layer** | Non-Duplicate Append | Checks for existing `GovernorateIds` and adds only those not already linked to the representative. |\n\n" +
                          "#### **🔒 Transactional Integrity:**\n" +
                          "* **Full Rollback**: Uses `BeginTransactionAsync`. If identity update fails (e.g., Email conflict) or database save fails, all changes are reverted. \n" +
                          "* **Identity Sync**: Changes to core user data are synchronized with the Identity store before committing logistics changes.\n\n" +
                          "> **Note:** The `Id` field is mandatory to locate the representative record.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerRequestExample(typeof(UpdateShippingRepresentativeDTO), typeof(UpdateShippingRepRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateShippingRepSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateShippingRepBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policy =
            $"Permission={ShippingRepresentatives.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "DeleteShippingRepAccount",
            Summary = "Toggle representative activation status (Soft-Delete)",
            Description = "### 🛡️ Fleet Availability Control\n\n" +
                          "This endpoint performs a **Soft-Delete (Toggle)** on a shipping representative's account. It modifies the `IsDeleted` flag in the Identity store without removing the physical record.\n\n" +
                          "#### **⚙️ Operational Mechanics:**\n" +
                          "| Feature | Impact Details |\n" +
                          "| :--- | :--- |\n" +
                          "| **Status Toggle** | Inverts the current `IsDeleted` state. Can be used for both suspension and restoration. |\n" +
                          "| **Order Continuity** | Historical delivery records remain intact and linked to the representative for financial auditing. |\n" +
                          "| **Access Revocation** | When `IsDeleted` is true, the representative is blocked from using the mobile application or system dashboard. |\n\n" +
                          "#### **🔒 Internal Execution Workflow:**\n" +
                          "1. **Entity Retrieval**: Fetches the `ShippigRepresentative` and their linked `ApplicationUser` via `ShippingRepresentativeSpecification`. \n" +
                          "2. **State Mutation**: Flips the boolean value of `user.IsDeleted`. \n" +
                          "3. **Identity Synchronization**: Calls `userManager.UpdateAsync` to update the security metadata in the database. \n" +
                          "4. **Persistence**: Confirms changes through `unitOfWork.Save()`.\n\n" +
                          "> **Note:** If the operation fails during the Identity update (e.g., concurrency issues), a **400 BadRequest** with detailed errors is returned.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteShippingRepSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DeleteShippingRepNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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