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
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.MerchantsExamples.Requests;
using ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
        [SwaggerOperation(
            OperationId = "GetMerchantsList",
            Summary = "Retrieve paginated list of merchants with detailed logistics settings",
            Description = "### 🏢 Merchant Directory & Analytics\n\n" +
                            "This endpoint provides a comprehensive view of all registered business partners (Merchants). It aggregates identity data from `AspNetUsers` with business-specific settings from the `Merchants` table.\n\n" +
                            "#### **⚙️ Data Aggregation Logic:**\n" +
                            "| Layer | Information Resolved |\n" +
                            "| :--- | :--- |\n" +
                            "| **Identity Layer** | Name, Email, Phone, Address, and Hiring (Start) Date. |\n" +
                            "| **Business Layer** | Store Name, Custom Pickup Fees, and Historical Rejection Rates. |\n" +
                            "| **Relationship Layer** | Assigned Branches and Merchant-specific City Price Overrides. |\n\n" +
                            "#### **🔒 Internal Execution Workflow:**\n" +
                            "1. **Filtering**: Applies `MerchantSpecification` for global search and status filtering (Active/Deleted). \n" +
                            "2. **Eager Loading**: Resolves navigation properties (via `Include`) to prevent N+1 queries for `MerchantSpecialPrices` and `UserBranches`. \n" +
                            "3. **DTO Mapping**: Flattens the complex entity graph into a clean, frontend-ready `MerchantDTO`. \n" +
                            "4. **Pagination**: Wraps results in a standard metadata envelope (`TotalCount`, `PageIndex`).\n\n" +
                            "> **Note:** Sorting can be applied by name, store name, or start date.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerRequestExample(typeof(MerchantParams), typeof(MerchantParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllMerchantsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GetMerchantsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<MerchantDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "PostCreateMerchant",
            Summary = "Onboard a new merchant with multi-entity synchronization",
            Description = "### 🏗️ Merchant Onboarding Workflow\n\n" +
                          "This endpoint manages the complex creation of a Merchant account. It utilizes a **Database Transaction** to ensure that either all components are created successfully or none at all (Atomicity).\n\n" +
                          "#### **⚙️ Transactional Steps:**\n" +
                          "1. **Identity Setup**: Creates a record in `AspNetUsers` with a generated unique `UserName` and encrypted password.\n" +
                          "2. **Role Assignment**: Automatically grants the `Merchant` role to the new user.\n" +
                          "3. **Domain Profile**: Creates the `Merchant` entity linked to the user ID containing store settings.\n" +
                          "4. **Branch Mapping**: Populates the `UserBranches` junction table with selected branch IDs.\n" +
                          "5. **Pricing Customization**: (Optional) Inserts custom rate overrides into the `MerchantSpecialPrice` table.\n\n" +
                          "#### **🛡️ Validation Gateways:**\n" +
                          "* **Uniqueness**: Rejects requests if the **Email** is already registered.\n" +
                          "* **Identity Integrity**: If `userManager.CreateAsync` fails (e.g., weak password), the entire transaction is rolled back.\n\n" +
                          "> **Success Note:** Returns a **201 Created** status with a confirmation message upon successful commit.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerRequestExample(typeof(AddMerchantDTO), typeof(AddMerchantRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(AddMerchantSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AddMerchantBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "PutUpdateMerchant",
            Summary = "Synchronize and update merchant profile and settings",
            Description = "### 🔄 Merchant Profile Synchronization\n\n" +
                          "Updates an existing merchant's identity and logistics configuration. The endpoint employs a **Full-Sync Strategy** for related collections to ensure data consistency.\n\n" +
                          "#### **⚙️ Update Strategy per Layer:**\n" +
                          "| Layer | Strategy | Logic Implementation |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Identity (User)** | Partial Patch | Updates only non-null fields (Name, Email, Phone) via `UserManager`. |\n" +
                          "| **Business (Merchant)** | Direct Update | Modifies store name, rejection rates, and pickup fees. |\n" +
                          "| **Branches Mapping** | Replace Logic | **Deletes all** existing branch links and re-inserts the new provided list. |\n" +
                          "| **Price Overrides** | Replace Logic | **Flushes all** custom city prices and re-populates from the request. |\n\n" +
                          "#### **🔒 Transactional Integrity:**\n" +
                          "* **Atomicity**: All updates (User + Merchant + Branches + Prices) are wrapped in a single transaction. \n" +
                          "* **Rollback**: If any sub-operation fails (e.g., Identity validation error), all changes are reverted to the previous state.\n\n" +
                          "> **Note:** Providing an empty list for Branches or Special Prices will effectively clear all existing assignments.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerRequestExample(typeof(UpdateMerchantDTO), typeof(UpdateMerchantRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateMerchantSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateMerchantNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "DeleteMerchantAccount",
            Summary = "Toggle merchant account activation status (Soft-Delete)",
            Description = "### 🛡️ Merchant Lifecycle Protection\n\n" +
                          "This endpoint implements a **Toggle Soft-Delete** mechanism. Instead of permanent database removal, it flips the `IsDeleted` flag on the associated Identity User account.\n\n" +
                          "#### **⚙️ Operational Logic:**\n" +
                          "| Action | Implementation Details |\n" +
                          "| :--- | :--- |\n" +
                          "| **State Toggle** | If the account is active, it becomes 'Deleted'. If already 'Deleted', this action restores it. |\n" +
                          "| **Data Integrity** | Preserves all historical orders, branches, and special prices linked to the merchant for auditing. |\n" +
                          "| **Access Control** | Once `IsDeleted` is true, the merchant will be blocked from authenticating or creating new orders. |\n\n" +
                          "#### **🔒 Internal Execution Workflow:**\n" +
                          "1. **Entity Lookup**: Uses `MerchantSpecification` to retrieve the merchant and their linked `ApplicationUser`. \n" +
                          "2. **Flag Mutation**: Inverts the current value of `user.IsDeleted`. \n" +
                          "3. **Identity Sync**: Updates the user via `userManager.UpdateAsync` to ensure the change is reflected in the security store. \n" +
                          "4. **Consistency**: Triggers `unitOfWork.Save()` to finalize the state change.\n\n" +
                          "> **Note:** Successful execution returns a confirmation message, regardless of whether the state was toggled to 'Deleted' or 'Restored'.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteMerchantSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DeleteMerchantNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "GetMerchantDetailedProfile",
            Summary = "Fetch comprehensive merchant profile by ID",
            Description = "### 🔍 Deep Merchant Inspection\n\n" +
                          "Retrieves a 360-degree view of a merchant's account by aggregating data from the identity system and core logistics modules.\n\n" +
                          "#### **⚙️ Data Composition & Sources:**\n" +
                          "| Section | Source Table | Information Resolved |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Identity Details** | `AspNetUsers` | Legal name, contact info, and hiring date. |\n" +
                          "| **Business Settings** | `Merchants` | Store branding, rejection rates, and custom pickup fees. |\n" +
                          "| **Network Access** | `UserBranches` | List of operational branches linked to the merchant. |\n" +
                          "| **Pricing Strategy** | `MerchantSpecialPrices` | City-specific delivery rate overrides. |\n\n" +
                          "#### **🔒 Internal Retrieval Workflow:**\n" +
                          "1. **Primary Lookup**: Uses `MerchantSpecification` to find the base merchant record. \n" +
                          "2. **Identity Sync**: Fetches the associated `ApplicationUser` via `UserManager`. \n" +
                          "3. **Relationship Mapping**: Resolves linked branches and filters the global `MerchantSpecialPrice` table specifically for this merchant. \n" +
                          "4. **Projection**: Maps the gathered entities into a unified `MerchantDTO` structure.\n\n" +
                          "> **Note:** If the ID is invalid or the merchant doesn't exist, a **404 Not Found** is returned.",
            Tags = new[] { "3. Business Partners (Merchants & Reps)" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetMerchantByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(MerchantNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(MerchantDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
