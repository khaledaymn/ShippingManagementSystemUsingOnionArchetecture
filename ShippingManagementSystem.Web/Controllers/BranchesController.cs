using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.BranchesExamples.Requests;
using ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.Schemas;
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
        [SwaggerOperation(
            OperationId = "GetBranchesList",
            Summary = "Retrieve a paginated list of operational branches",
            Description = "### 🏢 Operational Hub Management\n\n" +
                          "Branches serve as the physical sorting and dispatch centers in the shipping ecosystem. This endpoint allows stakeholders to browse and filter these strategic locations.\n\n" +
                          "#### **⚙️ Search & Filtering Capabilities:**\n" +
                          "* **Multi-Field Search**: Filter by branch name or specific physical location keywords.\n" +
                          "* **City-Based Isolation**: Use `CityId` to list hubs within a specific urban area.\n" +
                          "* **Audit Visibility**: Toggle between active and archived (soft-deleted) branches using `IsDeleted`.\n\n" +
                          "#### **📊 Data Composition:**\n" +
                          "The response returns a flattened `GetBranchDTO` which includes:\n" +
                          "1. **Temporal Data**: `CreationDate` formatted as `yyyy-MM-dd` for UI consistency.\n" +
                          "2. **Geographic Link**: Resolves and displays the `CityName` directly from the linked City entity.\n\n" +
                          "#### **🛡️ Constraints & Validation:**\n" +
                          "* **Performance Clamping**: Page size is limited to **10 items** per request to maintain dashboard responsiveness.\n" +
                          "* **Unauthorized Access**: Returns **401/403** if the user lacks sufficient permissions or roles.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(BranchParams), typeof(BranchParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllBranchesSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BranchBadRequestStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<GetBranchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "GetBranchDetails",
            Summary = "Fetch detailed information of a specific hub",
            Description = "### 🔍 Hub Detail Resolution\n\n" +
                          "Retrieves full operational details for a single branch. This includes physical location and the associated city name, flattened for easy display.\n\n" +
                          "#### **⚙️ Response Architecture:**\n" +
                          "* **Temporal Formatting**: The `CreationDate` is string-formatted as `yyyy-MM-dd` for direct UI binding.\n" +
                          "* **Relational Resolve**: Dynamically joins with the `Cities` table to provide the `CityName` instead of just an ID.\n\n" +
                          "#### **⚠️ Error Handling (Plain Text Response):**\n" +
                          "This endpoint returns raw text messages instead of JSON objects for errors:\n" +
                          "| Status | Scenario | Sample Message |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **400** | ID is zero or negative. | `Invalid branch ID` |\n" +
                          "| **404** | ID doesn't exist. | `Branch with ID {id} not found` |\n" +
                          "| **500** | System failure. | `An error occurred while retrieving branch...` |\n\n" +
                          "> **Note:** Useful for administrative panels when editing branch details or assigning personnel to a specific hub.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBranchByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(BranchNotFoundStringExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BranchCreateBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(GetBranchDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "PostCreateBranch",
            Summary = "Establish a new operational branch",
            Description = "### 🏗️ Hub Infrastructure Deployment\n\n" +
                          "Registers a new physical sorting center in the logistics network. This hub will be responsible for handling all shipments within its assigned city.\n\n" +
                          "#### **✅ Success Response (200 OK):**\n" +
                          "Returns a plain text confirmation message: `Branch '{Name}' created successfully`.\n\n" +
                          "#### **⚠️ Failure Scenarios (400 BadRequest):**\n" +
                          "The API returns a raw string message for the following:\n" +
                          "1. **City Not Found**: If the provided `CityId` does not exist in the database.\n" +
                          "2. **Null Data**: If the request body is empty.\n" +
                          "3. **Validation Error**: If `Name` or `Location` fields fail string length or requirement constraints.\n\n" +
                          "#### **🔒 Business Logic:**\n" +
                          "* **Timestamping**: The system automatically captures the current date/time as the `CreationDate`.\n" +
                          "* **Lifecycle**: Branches are initialized in an active state (`IsDeleted = false`).",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(CreateBranchDTO), typeof(CreateBranchRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BranchCreateSuccessStringExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BranchCreateErrorStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "PutUpdateBranch",
            Summary = "Modify branch details (Partial Update)",
            Description = "### 🔄 Hub Configuration Update\n\n" +
                          "Updates an existing branch's details. This endpoint supports **Partial Updates**, meaning only the fields you provide will be changed.\n\n" +
                          "#### **⚙️ Update Mechanics:**\n" +
                          "* **Name & Location**: Only updated if the string is not null or empty.\n" +
                          "* **City Binding**: Only updated if `CityId` is provided and greater than 0.\n" +
                          "* **Immutable Data**: The `CreationDate` remains untouched to preserve historical records.\n\n" +
                          "#### **⚠️ Failure & Error Handling (Plain Text):**\n" +
                          "Returns a raw string message for errors:\n" +
                          "| Status | Scenario | Sample Message |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **400** | ID is zero or negative. | `Invalid branch ID` |\n" +
                          "| **400** | Business Logic Failure. | `Branch with id {id} not found` |\n" +
                          "| **400** | Validation Exception. | `Validation error: {message}` |\n\n" +
                          "> **Context:** Useful for re-locating hubs or renaming branches without affecting their operational history.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(BranchDTO), typeof(UpdateBranchRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BranchUpdateSuccessStringExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BranchUpdateErrorStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [SwaggerOperation(
            OperationId = "DeleteBranchStatus",
            Summary = "Toggle branch activation status (Soft-Delete)",
            Description = "### 🛡️ Operational Continuity Protection\n\n" +
                          "This endpoint performs a **Soft-Delete (Toggle)**. It does not remove the branch from the database but instead flips the `IsDeleted` status.\n\n" +
                          "#### **⚙️ Toggle Logic:**\n" +
                          "* **Deactivation**: If the branch is active, it will be marked as deleted (hidden from sorting/selection).\n" +
                          "* **Restoration**: If the branch was already deleted, calling this again will restore it to an active state.\n\n" +
                          "#### **⚠️ Failure Scenarios (Plain Text):**\n" +
                          "| Status | Scenario | Sample Message |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **400** | ID is zero or negative. | `Invalid branch ID` |\n" +
                          "| **404** | Branch ID doesn't exist. | `Branch with id {id} not found` |\n" +
                          "| **500** | System Error. | `An error occurred while deleting branch...` |\n\n" +
                          "> **Note:** This is critical for data integrity, as branches may have historical links to delivered shipments.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BranchDeleteSuccessStringExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(BranchDeleteNotFoundStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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

        //[HttpDelete]
        //[Route("~/Branches/HardDelete/{id}")]
        //[Authorize(Policy =
        //    $"Permission={Branches.Delete};" +
        //    $"RequiredRole={Roles.Employee};" +
        //    $"AllowedRole={Roles.Admin}")]
        //public async Task<IActionResult> HardDelete(int id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest("Invalid branch ID");
        //        }

        //        var result = await _unitOfWork.BranchService.Delete(id);

        //        if (!result.IsSuccess)
        //        {
        //            return NotFound(result.Message);
        //        }

        //        return Ok(result.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception here
        //        return StatusCode(500, $"An error occurred while deleting branch with ID {id}");
        //    }
        //}
        #endregion

    }
}