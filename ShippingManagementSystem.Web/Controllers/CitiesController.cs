using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.Services;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.CitiesExamples_.Requests;
using ShippingManagementSystem.Web.Swagger.CitiesExamples_.Responses.Error;
using ShippingManagementSystem.Web.Swagger.CitiesExamples_.Responses.Success;
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
    public class CitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #region Get All Cities
        [HttpGet]
        [Route("~/Cities/GetAll")]
        [Authorize(Policy =
            $"Permission={Cities.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetCitiesList",
            Summary = "Retrieve a paginated list of cities with pricing data",
            Description = "### 🏙️ City-Level Logistics & Pricing\n\n" +
                          "This endpoint provides a detailed directory of cities. It serves as the primary source for resolving default shipping costs based on the customer's location.\n\n" +
                          "#### **⚙️ Advanced Filtering Engine:**\n" +
                          "* **Regional Filtering**: Use `GovernorateId` to isolate cities within a specific state/region.\n" +
                          "* **Price Range Search**: Filter cities based on their financial impact using `MinChargePrice` and `MaxChargePrice` parameters.\n" +
                          "* **Dynamic Sorting**: Order results by name or price to optimize selection UI.\n\n" +
                          "#### **📊 Data Composition:**\n" +
                          "Each record aggregates data from:\n" +
                          "1. **Core City Data**: Basic name and ID.\n" +
                          "2. **Logistics Engine**: Default `ChargePrice` and `PickUpPrice`.\n" +
                          "3. **Geographic Link**: Resolves the parent `GovernorateName` through eager loading.\n\n" +
                          "#### **🛡️ Validation Constraints:**\n" +
                          "* **Pagination**: `PageIndex` and `PageSize` must be strictly greater than 0.\n" +
                          "* **Performance**: Page size is automatically clamped to a maximum of 20 to ensure sub-second response times.\n\n" +
                          "> **Note:** Merchants and Representatives use this data to calculate total order amounts during the checkout process.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(CityParams), typeof(CityParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllCitiesSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CityPaginationErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CityParamBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<CityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCities([FromQuery] CityParams param)
        {
            try
            {
                if (param == null)
                {
                    return BadRequest(new { message = "Invalid city parameters" });
                }

                // Validate pagination parameters
                if (param.PageIndex < 1 || param.PageSize < 1)
                {
                    return BadRequest(new { message = "PageIndex and PageSize must be greater than 0" });
                }

                var result = await _unitOfWork.CityService.GetAllCitiesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework in production)
                return StatusCode(500, new { message = "An error occurred while retrieving cities", error = ex.Message });
            }
        }
        #endregion

        #region Get City By Id
        [HttpGet]
        [Route("~/Cities/GetCityById/{id}")]
        [Authorize(Policy =
            $"Permission={Cities.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetCityDetails",
            Summary = "Fetch comprehensive city details and logistics pricing",
            Description = "### 🔍 Deep City Inspection\n\n" +
                          "Retrieves full details for a specific city, including its financial configuration and geographic parentage. This is a critical endpoint for verifying delivery costs before order placement.\n\n" +
                          "#### **⚙️ Data Resolution Logic:**\n" +
                          "| Information | Source Table | Resolution Method |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **City Identity** | `Cities` | Direct ID lookup. |\n" +
                          "| **Pricing Model** | `Cities` | Retrieves `ChargePrice` and `PickUpPrice`. |\n" +
                          "| **Parent Region** | `Governorates` | **Eager Loading** via `CitySpecification` to fetch `Governorate.Name`. |\n\n" +
                          "#### **🔒 Internal Retrieval Workflow:**\n" +
                          "1. **Input Shielding**: Blocks IDs <= 0 with a **400 BadRequest**. \n" +
                          "2. **Specification Dispatch**: Invokes `CitySpecification` to perform a single optimized query with `Include(c => c.Governorate)`. \n" +
                          "3. **DTO Projection**: Flattens the complex entity graph into a clean `CityDTO`.\n\n" +
                          "> **Note:** If the city is marked as `IsDeleted`, it will still be returned if requested by ID for historical audit purposes.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCityByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(CityNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(CityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid city ID" });
                }

                var city = await _unitOfWork.CityService.GetCityByIdAsync(id);
                if (city == null)
                {
                    return NotFound(new { message = $"City with ID {id} not found" });
                }

                return Ok(city);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = $"An error occurred while retrieving city with ID {id}", error = ex.Message });
            }
        }
        #endregion

        #region Create City
        [HttpPost]
        [Route("~/Cities/CreateCity")]
        [Authorize(Policy =
            $"Permission={Cities.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PostCreateCity",
            Summary = "Define a new city with specific logistics pricing",
            Description = "### 🏗️ Geographic & Financial Onboarding\n\n" +
                          "Adds a new city to the logistics network. This operation establishes the default pricing model for all orders originating from or destined to this specific location.\n\n" +
                          "#### **⚠️ Validation Guardrails (400 BadRequest):**\n" +
                          "* **Financial Sanity**: `ChargePrice` and `PickUpPrice` cannot be negative values.\n" +
                          "* **Identity Integrity**: The `GovernorateId` must reference an existing record in the `Governorates` table.\n" +
                          "* **String Constraints**: City `Name` must be unique (within the business logic) and not exceed 100 characters.\n\n" +
                          "#### **🔒 Internal Persistence Workflow:**\n" +
                          "1. **Validation Layer**: Checks `ModelState` and explicit business rules (positive pricing). \n" +
                          "2. **Relational Check**: The Service queries the `Governorate` repository to verify parent existence before insertion. \n" +
                          "3. **State Initialization**: Automatically sets `IsDeleted` to `false`. \n" +
                          "4. **Commit**: Finalizes the record in the `Cities` table and returns a success manifest.\n\n" +
                          "> **Note:** Successful creation returns a **201 Created** status. This city will immediately appear in lookup dropdowns for merchants.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerRequestExample(typeof(CreateCityDTO), typeof(CreateCityRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateCitySuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CityGovernorateNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDTO cityDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid city data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(cityDTO.Name))
                {
                    return BadRequest(new { message = "City name is required" });
                }

                if (cityDTO.ChargePrice < 0 || cityDTO.PickUpPrice < 0)
                {
                    return BadRequest(new { message = "Prices cannot be negative" });
                }

                if (cityDTO.GovernorateId <= 0)
                {
                    return BadRequest(new { message = "Invalid Governorate ID" });
                }

                var (isSuccess, message) = await _unitOfWork.CityService.CreateCityAsync(cityDTO);

                if (!isSuccess)
                {
                    return BadRequest(new { message });
                }

                return Created("",  message );
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while creating the city", error = ex.Message });
            }
        }
        #endregion

        #region Edit City
        [HttpPut]
        [Route("~/Cities/Edit")]
        [Authorize(Policy =
             $"Permission={Cities.Edit};" +
             $"RequiredRole={Roles.Employee};" +
             $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
             OperationId = "PutUpdateCity",
             Summary = "Update city properties or financial configuration",
             Description = "### 🔄 Dynamic Patching & Pricing Adjustment\n\n" +
                           "Modifies an existing city record. This endpoint employs a **Partial Update Strategy**, allowing you to change specific fields without providing the entire object state.\n\n" +
                           "#### **⚙️ Update Mechanics:**\n" +
                           "| Field | Strategy | Behavior |\n" +
                           "| :--- | :--- | :--- |\n" +
                           "| **Name** | Null-Check | Updates only if a non-empty string is provided. |\n" +
                           "| **Prices** | HasValue-Check | Updates `ChargePrice` or `PickUpPrice` only if they are explicitly sent. |\n" +
                           "| **Governorate** | Relational Sync | If `GovernorateId` changes, the system verifies the new parent exists before re-linking. |\n\n" +
                           "#### **🛡️ Validation & Safety (400 BadRequest):**\n" +
                           "* **Zero/Negative Check**: Rejects any pricing values less than 0.\n" +
                           "* **Existence Check**: If the city ID does not exist, a **404 NotFound** is returned.\n" +
                           "* **Whitespace Guard**: Prevents updating names to empty strings or spaces.\n\n" +
                           "#### **🔒 Transactional Flow:**\n" +
                           "1. **Identity Phase**: Locates the city via `GetById`. \n" +
                           "2. **Mutation Phase**: Applies changes only to fields present in the request (`HasValue`). \n" +
                           "3. **Relational Phase**: (Optional) Validates and switches the parent Governorate if requested. \n" +
                           "4. **Persistence**: Finalizes through `unitOfWork.Save()`.\n\n" +
                           "> **Note:** To maintain the current value of a field, simply omit it from the JSON body or set it to `null`.",
             Tags = new[] { "4. System Configuration" }
         )]
        [SwaggerRequestExample(typeof(EditCityDTO), typeof(EditCityRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EditCitySuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CityNegativePriceErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(CityNotFoundByIdExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditCity([FromBody] EditCityDTO cityDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid city data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                if (cityDTO.Id <= 0)
                {
                    return BadRequest(new { message = "Invalid city ID" });
                }

                // Additional validation
                if (!string.IsNullOrEmpty(cityDTO.Name) && string.IsNullOrWhiteSpace(cityDTO.Name))
                {
                    return BadRequest(new { message = "City name cannot be empty or whitespace" });
                }

                if (cityDTO.ChargePrice.HasValue && cityDTO.ChargePrice < 0)
                {
                    return BadRequest(new { message = "Charge price cannot be negative" });
                }

                if (cityDTO.PickUpPrice.HasValue && cityDTO.PickUpPrice < 0)
                {
                    return BadRequest(new { message = "Pick-up price cannot be negative" });
                }

                if (cityDTO.GovernorateId.HasValue && cityDTO.GovernorateId <= 0)
                {
                    return BadRequest(new { message = "Invalid Governorate ID" });
                }

                var (isSuccess, message) = await _unitOfWork.CityService.EditCityAsync(cityDTO);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while updating the city", error = ex.Message });
            }
        }
        #endregion

        #region Delete City

        [HttpDelete]
        [Route("~/Cities/DeleteCity/{id}")]
        [Authorize(Policy =
            $"Permission={Cities.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "DeleteCityStatus",
            Summary = "Toggle city activation status (Soft-Delete)",
            Description = "### 🛡️ Logistics Integrity Guard\n\n" +
                          "Performs a **Soft-Delete (Toggle)** on a specific city. This ensures that historical orders and active branches linked to this city remain consistent in the database.\n\n" +
                          "#### **⚙️ Operational Behavior:**\n" +
                          "* **State Inversion**: If the city is active, it becomes 'Deleted' (hidden from new orders). If already 'Deleted', this action restores it.\n" +
                          "* **Audit Preservation**: Keeps all financial and delivery manifests intact for future reporting.\n\n" +
                          "#### **⚠️ Failure Scenarios & Errors:**\n" +
                          "| Status | Scenario | Response Object |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **400 BadRequest** | ID is zero or negative. | `{ \"message\": \"Invalid city ID\" }` |\n" +
                          "| **404 NotFound** | City ID does not exist. | `{ \"message\": \"City with id {id} not found\" }` |\n" +
                          "| **500 Error** | Database connection issue. | `{ \"message\": \"An error occurred...\", \"error\": \"...\" }` |\n\n" +
                          "> **Tip:** Use this endpoint to temporarily suspend shipping to a specific city during holidays or operational maintenance.",
            Tags = new[] { "4. System Configuration" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteCitySuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(DeleteCityNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid city ID" });
                }

                var (isSuccess, message) = await _unitOfWork.CityService.DeleteCityAsync(id);

                if (!isSuccess)
                {
                    return NotFound(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while deleting the city", error = ex.Message });
            }
        }
        #endregion
    }
}