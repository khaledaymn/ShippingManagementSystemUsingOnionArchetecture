using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.GovernorateSpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Requests;
using ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Responses.Success;
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
    public class GovernoratesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GovernoratesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #region Get All Governorates
        [HttpGet]
        [Route("~/Governorates/GetAll")]
        [Authorize(Policy =
            $"Permission={Governorates.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetGovernoratesList",
            Summary = "Retrieve a paginated list of all governorates",
            Description = "### 🌍 Geographic Foundations\n\n" +
                          "Governorates represent the top-level administrative divisions in the system. This endpoint allows all primary stakeholders to browse available delivery regions.\n\n" +
                          "#### **⚙️ Features & Capabilities:**\n" +
                          "* **Full-Text Search**: Filter regions by partial name matches using the `Search` parameter.\n" +
                          "* **Soft-Delete Filtering**: Toggle between active and archived regions using the `IsDeleted` flag.\n" +
                          "* **Performance Optimized**: Results are paginated and clamped to a maximum of 100 records per request to ensure low latency.\n\n" +
                          "#### **🛡️ Validation Rules (400 BadRequest):**\n" +
                          "The request will fail if:\n" +
                          "1. `PageIndex` or `PageSize` is less than 1.\n" +
                          "2. Invalid sorting parameters are provided.\n\n" +
                          "> **Context:** These governorates serve as the parent entities for all Cities and Branches in the logistics chain.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(GovernorateParams), typeof(GovernorateParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllGovernoratesSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<GovernorateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGovernorates([FromQuery] GovernorateParams param)
        {
            try
            {
                if (param == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = 400,
                        Message = "Invalid governorate parameters"
                    });
                }

                // Validate pagination parameters
                if (param.PageIndex < 1 || param.PageSize < 1)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = 400,
                        Message = "PageIndex and PageSize must be greater than 0"
                    });
                }

                var result = await _unitOfWork.GovernorateService.GetAllGovernoratesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework in production)
                return StatusCode(500, new { message = "An error occurred while retrieving governorates", error = ex.Message });
            }
        }
        #endregion

        #region Get Governorate By ID
        [HttpGet]
        [Route("~/Governorates/GetById/{id:int}")]
        [Authorize(Policy =
            $"Permission={Governorates.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetGovernorateDetails",
            Summary = "Fetch detailed information of a single governorate",
            Description = "### 🔍 Regional Anchor Lookup\n\n" +
                          "Retrieves the unique record for a specific governorate. In the logistics hierarchy, this ID is critical as it acts as the parent reference for associated cities and shipping zones.\n\n" +
                          "#### **⚙️ Execution Workflow:**\n" +
                          "1. **Input Shielding**: Validates that the `{id}` is a positive integer (> 0). \n" +
                          "2. **Specification Dispatch**: Utilizes `GovernorateSpecification` to encapsulate the query logic, ensuring high performance and decoupling. \n" +
                          "3. **Entity-to-DTO Projection**: Transforms the internal domain entity into a clean `GovernorateDTO` for secure data exposure.\n\n" +
                          "#### **🛡️ Response Matrix:**\n" +
                          "| Status | Scenario | Output Content |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | ID matches a record. | Complete `GovernorateDTO` object. |\n" +
                          "| **400 BadRequest** | ID is <= 0. | Error message: 'Invalid governorate ID'. |\n" +
                          "| **404 NotFound** | ID does not exist in DB. | Error message: 'Governorate with ID {id} not found'. |\n\n" +
                          "> **Note:** This endpoint is accessible to all internal roles as it is fundamental for address selection in orders.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGovernorateByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(GovernorateNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(GovernorateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGovernorateById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid governorate ID" });
                }

                var governorate = await _unitOfWork.GovernorateService.GetGovernorateByIdAsync(id);
                if (governorate == null)
                {
                    return NotFound(new { message = $"Governorate with ID {id} not found" });
                }

                return Ok(governorate);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = $"An error occurred while retrieving governorate with ID {id}", error = ex.Message });
            }
        }

        #endregion

        #region Create Governorate
        [HttpPost]
        [Route("~/Governorates/Create")]
        [Authorize(Policy =
            $"Permission={Governorates.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PostCreateGovernorate",
            Summary = "Define a new geographical region (Governorate)",
            Description = "### 🏗️ Territory Expansion Logic\n\n" +
                          "Adds a new top-level administrative region to the system. This is a foundational operation that enables the subsequent definition of Cities and Branches within this region.\n\n" +
                          "#### **📝 Input Validation Rules:**\n" +
                          "* **Mandatory Name**: The `Name` field cannot be null, empty, or consist only of whitespace.\n" +
                          "* **Length Constraint**: Name must adhere to the defined `StringLength` (Max 100 characters).\n\n" +
                          "#### **🔒 Internal Business Workflow:**\n" +
                          "1. **State Initialization**: Newly created governorates are automatically set to `IsDeleted = false` by default.\n" +
                          "2. **Identity Generation**: The system assigns a unique integer ID upon successful persistence in the database.\n" +
                          "3. **Relational Readiness**: Once committed, this region becomes instantly available as a parent reference for new City entities.\n\n" +
                          "#### **🛡️ Authorization Context:**\n" +
                          "> **Restricted Access**: This operation is limited to **System Administrators** to ensure centralized control over the logistics network expansion.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(CreateGovernorateDTO), typeof(CreateGovernorateRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateGovernorateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateGovernorateValidationErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGovernorate([FromBody] CreateGovernorateDTO governorateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid governorate data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(governorateDTO.Name))
                {
                    return BadRequest(new { message = "Governorate name is required" });
                }

                var (isSuccess, message) = await _unitOfWork.GovernorateService.CreateGovernorateAsync(governorateDTO);

                if (!isSuccess)
                {
                    return BadRequest(new { message });
                }

                return Created("", governorateDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while creating the governorate", error = ex.Message });
            }
        }
        #endregion

        #region Update Governorate
        [HttpPut]
        [Route("~/Governorates/Update/{id:int}")]
        [Authorize(Policy =
            $"Permission={Governorates.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PutUpdateGovernorate",
            Summary = "Modify an existing governorate's properties",
            Description = "### 🔄 Master Data Synchronization\n\n" +
                          "Updates the details of an existing governorate. This operation ensures data consistency across the geographic hierarchy.\n\n" +
                          "#### **⚠️ Critical Validation Rules (400 BadRequest):**\n" +
                          "The request must satisfy the following security and integrity checks:\n" +
                          "* **ID Integrity**: The `{id}` in the URL **must exactly match** the `Id` inside the JSON body.\n" +
                          "* **Data Presence**: The `Name` cannot be empty or consist solely of whitespace.\n" +
                          "* **Existence**: If the ID is valid but doesn't exist in the database, a **404 NotFound** is returned.\n\n" +
                          "#### **⚙️ Update Logic:**\n" +
                          "1. **Partial Update**: The system only updates the `Name` if a non-empty string is provided.\n" +
                          "2. **Identity Preservation**: The primary key (Id) remains immutable to protect relational integrity with Cities and Branches.\n\n" +
                          "> **Note:** Changes here will be reflected globally across all shipping addresses linked to this region.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(GovernorateDTO), typeof(UpdateGovernorateRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GovernorateUpdateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GovernorateIdMismatchExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(GovernorateNotFoundExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGovernorate(int id, [FromBody] GovernorateDTO governorateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid governorate data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid governorate ID" });
                }

                if (id != governorateDTO.Id)
                {
                    return BadRequest(new { message = "Governorate ID in URL does not match the DTO ID" });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(governorateDTO.Name))
                {
                    return BadRequest(new { message = "Governorate name cannot be empty or whitespace" });
                }

                var (isSuccess, message) = await _unitOfWork.GovernorateService.UpdateGovernorateAsync(id, governorateDTO);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while updating the governorate", error = ex.Message });
            }
        }
        #endregion

        #region Delete Governorate
        [HttpDelete]
        [Route("~/Governorates/Delete/{id:int}")]
        [Authorize(Policy =
            $"Permission={Governorates.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "DeleteGovernorateStatus",
            Summary = "Toggle governorate activation status (Soft-Delete)",
            Description = "### 🛡️ Regional Lifecycle Protection\n\n" +
                          "This endpoint implements a **Toggle Soft-Delete** mechanism. Instead of permanent removal, it flips the `IsDeleted` flag of the governorate.\n\n" +
                          "#### **⚙️ Operational Logic:**\n" +
                          "| Action | Implementation Details |\n" +
                          "| :--- | :--- |\n" +
                          "| **State Toggle** | If active, it marks as 'Deleted'. If already 'Deleted', it restores the record. |\n" +
                          "| **Data Integrity** | Prevents breaking foreign key constraints with existing Orders, Cities, or Branches. |\n" +
                          "| **Visibility** | Soft-deleted governorates are typically filtered out from client-side dropdowns. |\n\n" +
                          "#### **🔒 Internal Execution Workflow:**\n" +
                          "1. **Lookup**: Fetches the entity using the provided `{id}`. \n" +
                          "2. **Inversion**: Sets `IsDeleted = !IsDeleted`. \n" +
                          "3. **Persistence**: Updates the record and commits the change via `unitOfWork.Save()`. \n\n" +
                          "> **Note:** Successful execution returns a confirmation message containing the governorate name.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteGovernorateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(DeleteGovernorateNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid governorate ID" });
                }

                var (isSuccess, message) = await _unitOfWork.GovernorateService.DeleteGovernorateAsync(id);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while deleting the governorate", error = ex.Message });
            }
        }

        #endregion
    }
}