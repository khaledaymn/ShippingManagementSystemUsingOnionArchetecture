using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.EmployeesExamples.Requests;
using ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get All Employees
        [HttpGet]
        [Route("~/Employees/GetAll")]
        [Authorize(Policy =
                    $"Permission={Employees.View};" +
                    $"RequiredRole={Roles.Employee};" +
                    $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
                    OperationId = "GetEmployeesList",
                    Summary = "Paginated retrieval of system administrative staff",
                    Description = "### 🛡️ Administrative Identity & Access Management\n\n" +
                                  "Provides a high-level overview of all users with 'Employee' roles. This endpoint integrates data from **Identity Management**, **Branch Associations**, and **Permission Groups**.\n\n" +
                                  "#### **⚙️ Advanced Filtering Capabilities:**\n" +
                                  "* **Global Search**: Scans through `Name`, `Email`, `PhoneNumber`, and `Address` for matching keywords.\n" +
                                  "* **State Management**: Use `IsActive` to filter between currently operational staff and those who have been soft-deleted/deactivated.\n\n" +
                                  "#### **🏗️ Data Architecture & Relationships:**\n" +
                                  "* **Branch Scope**: Every employee is mapped to one or more **Branches**. This relationship determines which shipments the employee can view in other modules.\n" +
                                  "* **Permission Mapping**: The `Permission` field represents the **Security Group (Role)** name. This dictates the `Permission` Claims injected into the user's JWT token.\n\n" +
                                  "#### **⚠️ Error & Response Protocol:**\n" +
                                  "| Status | Scenario | Format |\n" +
                                  "| :--- | :--- | :--- |\n" +
                                  "| **200 OK** | Success. Returns paginated DTOs with nested Branch objects. | `PaginationResponse<EmployeeDTO>` |\n" +
                                  "| **400 Bad** | Application-level exception caught in `try-catch`. | `Object { Message: string }` |\n" +
                                  "| **401 Unauth** | Missing or invalid Bearer Token. | `Empty / Default` |\n" +
                                  "| **403 Forbid** | User lacks `Employees.View` permission or correct Role. | `Empty / Default` |\n" +
                                  "| **500 Err** | Infrastructure failure (Database connection/Identity crash). | `Object { Message: string }` |\n\n" +
                                  "> **Performance Note:** PageSize is strictly clamped to **10 records** per request to optimize server memory during complex identity joins.",
                    Tags = new[] { "5. Administrative Control & Analytics" }
                )]
        [SwaggerRequestExample(typeof(EmployeeParams), typeof(EmployeeParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllEmployeesSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(EmployeeErrorResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(EmployeeErrorResponseExample))]
        [ProducesResponseType(typeof(PaginationResponse<EmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IReadOnlyList<EmployeeDTO>>> GetAll([FromQuery] EmployeeParams param)
        {
            try
            {
                var result = await _unitOfWork.EmployeeService.GetAllEmployeesAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

        #region Add Employee

        [HttpPost]
        [Route("~/Employees/Add")]
        [Authorize(Policy =
            $"Permission={Employees.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PostAddEmployee",
            Summary = "Onboard a new administrative staff member",
            Description = "### 🏗️ Atomic Administrative Onboarding\n\n" +
                          "This endpoint executes a **Multi-Stage Database Transaction** to create a fully operational system user. It bridges Identity Security with Logistics Operations.\n\n" +
                          "#### **⚙️ Internal Execution Workflow:**\n" +
                          "1. **Identity Creation**: Registers the user in the `AspNetUsers` table with a secure password.\n" +
                          "2. **Role Assignment**: Automatically grants the `Employee` role to the user.\n" +
                          "3. **Operational Linking**: Maps the user to one or more physical **Branches** via the `UserBranches` table.\n" +
                          "4. **Dynamic Claims Injection**: Fetches all modules associated with the selected `GroupId` and injects them as **JWT Permission Claims**.\n\n" +
                          "#### **🛡️ Security & Integrity Guardrails:**\n" +
                          "* **Transactional Safety**: If any step fails (e.g., Branch ID doesn't exist), the entire process is **Rolled Back** to prevent 'Ghost Users'.\n" +
                          "* **Unique Constraint**: The system enforces unique emails and auto-generates a unique `UserName` if a conflict occurs.\n" +
                          "* **Mandatory Assets**: A `GroupId` and at least one `BranchId` are strictly required to proceed.\n\n" +
                          "#### **⚠️ Response & Failure Matrix:**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **201 Created** | Employee, Roles, Branches, and Claims successfully provisioned. | `Object { Message: string }` |\n" +
                          "| **400 Bad** | Validation failure (Missing fields) or Logic error (Email exists). | `Object { Message: string }` |\n" +
                          "| **500 Err** | Identity Framework crash or Transaction failure. | `Object { Message: string }` |\n\n" +
                          "> **Note:** Successful creation returns a `201 Created` status with the location header pointing to the list of employees.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerRequestExample(typeof(AddEmployeeDTO), typeof(AddEmployeeRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(AddEmployeeSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AddEmployeeLogicErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(EmployeeErrorResponseExample))]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Add(AddEmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (isSuccess, message) = await _unitOfWork.EmployeeService.AddEmployeeAsync(dto);
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

        #region Update Employee

        [HttpPut]
        [Route("~/Employees/Update")]
        [Authorize(Policy =
            $"Permission={Employees.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PutUpdateEmployee",
            Summary = "Update employee profile and re-sync permissions",
            Description = "### 🔄 Dynamic Permission & Operational Re-sync\n\n" +
                          "Updates an employee's administrative profile. This endpoint is **Highly Sensitive** because it triggers an immediate re-calculation of the user's security context.\n\n" +
                          "#### **⚙️ Smart Sync Mechanics:**\n" +
                          "* **Claim Rotation**: If the `GroupId` is changed, the system **Purges all old Permission Claims** and injects the new group's permissions into the Identity database.\n" +
                          "* **Branch Override**: Providing `BranchIds` will completely replace the previous associations in the `UserBranches` table with the new list.\n\n" +
                          "#### **🛡️ Atomic Transaction Guard:**\n" +
                          "The update runs within a **Database Transaction**. If the Identity update fails or branch mapping hits a conflict, the system performs a **Full Rollback** to maintain data consistency.\n\n" +
                          "#### **⚠️ Response Matrix:**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success. User, Branches, and Claims are synchronized. | `String Message` |\n" +
                          "| **400 Bad** | Model invalid, ID missing, or User/Employee not found. | `Object { Message: string }` |\n" +
                          "| **500 Err** | Identity Framework Update failure or DB deadlock. | `Object { Message: string }` |\n\n" +
                          "> **Important Note:** Upon a successful `GroupId` change, the employee may need to re-authenticate (Login again) to receive the updated JWT with new Claims.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerRequestExample(typeof(UpdateEmployeeDTO), typeof(UpdateEmployeeRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateEmployeeSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateEmployeeErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(EmployeeErrorResponseExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(UpdateEmployeeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
                    return BadRequest(new { Message = "Invalid employee data." });

                var (isSuccess, message) = await _unitOfWork.EmployeeService.UpdateEmployeeAsync(dto);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

        #region Delete Employee

        [HttpDelete]
        [Route("~/Employees/Delete/{id}")]
        [Authorize(Policy =
            $"Permission={Employees.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "DeleteEmployeeStatus",
            Summary = "Deactivate or Restore employee access (Soft-Delete)",
            Description = "### 🛡️ Identity Preservation & Access Control\n\n" +
                          "Performs a **Soft-Delete (Toggle)** on the employee's Identity account. This ensures that historical data (Orders, Financial Transactions) remains intact while preventing system access.\n\n" +
                          "#### **⚙️ Operational Mechanics:**\n" +
                          "* **State Inversion**: Flipping the `IsDeleted` flag. If the employee was active, they are deactivated; if they were deleted, this action restores them.\n" +
                          "* **Access Revocation**: A deactivated employee will be rejected by the Identity Authentication middleware during login attempts.\n\n" +
                          "#### **🏗️ Data Integrity:**\n" +
                          "The employee's mappings in `UserBranches` and their assigned `Group` remain saved, allowing for an immediate 'Recovery' without re-configuring their profile.\n\n" +
                          "#### **⚠️ Response Matrix:**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success. Identity status toggled. | `String Message` |\n" +
                          "| **400 Bad** | Invalid ID format or Employee/User not found. | `Object { Message: string }` |\n" +
                          "| **500 Err** | Infrastructure failure or Identity database lock. | `Object { Message: string }` |\n\n" +
                          "> **Note:** To permanently remove an employee and purge their data, a Database Administrator must perform a manual purge. This API does not support Hard-Delete.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteEmployeeSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DeleteEmployeeErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(EmployeeErrorResponseExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var (isSuccess, message) = await _unitOfWork.EmployeeService.DeleteEmployeeAsync(id);
                if (!isSuccess)
                    return BadRequest(new { Message = message });

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

        #region Get Employee By Id

        [HttpGet]
        [Route("~/Employees/GetById/{id}")]
        [Authorize(Policy =
            $"Permission={Employees.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "GetEmployeeDetails",
            Summary = "Fetch a complete employee profile by ID",
            Description = "### 🔍 Granular Identity & Operational Insight\n\n" +
                          "Retrieves a detailed snapshot of an administrative employee. This endpoint merges **Identity Profile Data** with **Logistics Assignments** to provide a 360-degree view of the user.\n\n" +
                          "#### **⚙️ Data Composition:**\n" +
                          "* **Identity Details**: Personal info like `Email`, `PhoneNumber`, and `HiringDate` (CreationDate).\n" +
                          "* **Operational Scope**: A nested list of all **Branches** the employee is assigned to, used for populating multi-select dropdowns in edit forms.\n" +
                          "* **Security Context**: The `Permission` field returns the assigned **Group Name**, which defines the user's access level in the system.\n\n" +
                          "#### **🛡️ Retrieval Logic:**\n" +
                          "The system validates the ID against both the `Employees` table and the `AspNetUsers` identity store to ensure a consistent record is returned.\n\n" +
                          "#### **⚠️ Response Matrix:**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Full profile retrieved successfully. | `EmployeeDTO (JSON)` |\n" +
                          "| **404 Not** | ID does not exist in Identity or Employee records. | `Object { Message: string }` |\n" +
                          "| **500 Err** | Database connection failure or Identity provider crash. | `Object { Message: string }` |\n\n" +
                          "> **Note:** The `id` parameter must be the Identity User's **GUID**.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEmployeeByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(EmployeeNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(EmployeeErrorResponseExample))]
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeDTO>> GetById(string id)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                    return NotFound(new { Message = "Employee not found." });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

        #region Change Password

        [HttpPost]
        [Route("~/Account/ChangePassword")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative},{Roles.Employee}")]
        [SwaggerOperation(
            OperationId = "PostChangePassword",
            Summary = "Update user password while authenticated",
            Description = "### 🔐 Authenticated Credential Management\n\n" +
                        "Allows logged-in users to update their security credentials by verifying their existing password. This endpoint is protected and requires a valid JWT with appropriate roles.\n\n" +
                        "#### **⚙️ Internal Business Logic:**\n" +
                        "| Step | Action | Logic Source |\n" +
                        "| :--- | :--- | :--- |\n" +
                        "| **1. Request Check** | Data Integrity | Validates that the new password is not identical to the current one. |\n" +
                        "| **2. User Verification** | Account Lookup | Uses `UserManager.FindByIdAsync` to retrieve the user record. Returns **404** if not found. |\n" +
                        "| **3. Security Verification** | Identity Check | Verifies the **OldPassword** using `CheckPasswordAsync`. Returns **400** if verification fails. |\n" +
                        "| **4. Hash Update** | Credential Rotation | Invokes `ChangePasswordAsync` which resets the hash and rotates the `SecurityStamp`. |\n\n" +
                        "#### **🛡️ Security Policies:**\n" +
                        "* **Role Protection**: Accessible by all registered system roles (Admin, Merchant, Delivery, Employee). \n" +
                        "* **Session Termination**: Password rotation triggers an automatic session invalidation across all devices. \n\n" +
                        "> **Note:** Upon success, the user will need to use the new credentials for any future login sessions.",
            Tags = new[] { "1. Security & Identity" }
        )]
        [SwaggerRequestExample(typeof(ChangePasswordDTO), typeof(ChangePasswordRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ChangePasswordSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ChangePasswordBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ChangePasswordNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _unitOfWork.AuthenticationService.ChangePassword(model);

                if (result != "Password changed successfully.")
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while changing your password", error = ex.Message });
            }
        }
        #endregion

        #region GetSpecificUserData

        [HttpGet]
        [Route("~/Account/GetSpecificUserData/{role}/{id}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative},{Roles.Employee}")]
        [SwaggerOperation(
            OperationId = "GetProfileById",
            Summary = "Fetch detailed user profile by role and ID",
            Description = "### 👤 Polymorphic Profile Resolver\n\n" +
                        "This endpoint retrieves extended user data by dynamically resolving the user's role. It aggregates data from multiple domain entities to provide a complete profile view.\n\n" +
                        "#### **⚙️ Internal Business Logic:**\n" +
                        "| Step | Action | Implementation Details |\n" +
                        "| :--- | :--- | :--- |\n" +
                        "| **1. Primary Lookup** | Identity Check | Verifies base user existence in `AspNetUsers`. |\n" +
                        "| **2. Role Dispatch** | Entity Joining | Joins the relevant sub-table (`Employees`, `Merchants`, or `ShippigRepresentatives`) based on the `{role}` parameter. |\n" +
                        "| **3. Permission Matrix** | Claim Parsing | (Employee only) Flattens module-based claims into a structured dictionary. |\n" +
                        "| **4. Special Pricing** | Data Aggregation | (Merchant only) Resolves city-specific pricing overrides from `MerchantSpecialPrices`. |\n\n" +
                        "#### **🛡️ Authorization:**\n" +
                        "* **Role-Based**: Accessible by all authenticated staff and partners. \n" +
                        "* **Restriction**: Users can generally only query their own ID or IDs within their management scope. \n\n" +
                        "> **Note:** If `{role}` is 'Employee', the response will include a `Permissions` dictionary.",
            Tags = new[] { "1. Security & Identity" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEmployeeProfileSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetMerchantProfileExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetRepresentativeProfileExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(UserNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(SpecificUserDataDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSpecificUserData(string role, string id)
        {
            try
            {
                var userdata = await _unitOfWork.AuthenticationService.GetSpecificUser(role, id);
                if (userdata == null)
                {
                    return NotFound("User not found");
                }
                return Ok(userdata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }

        #endregion

        #region Update User Data

        [HttpPut]
        [Route("~/Account/EditProfile")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative},{Roles.Employee}")]
        [SwaggerOperation(
            OperationId = "PutEditProfile",
            Summary = "Update authenticated user personal profile",
            Description = "### 📝 Profile Information Management\n\n" +
                          "Allows authenticated users to modify their personal contact information, address, and system identifiers. This operation interacts directly with the **ASP.NET Core Identity** system to ensure data integrity and normalization.\n\n" +
                          "#### **⚙️ Internal Execution Flow:**\n" +
                          "| Step | Action | Logic |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **1. Identity Validation** | Record Retrieval | Uses `UserManager.FindByIdAsync` to fetch the user by ID. Returns **400 BadRequest** if the user is not found. |\n" +
                          "| **2. Delta Application** | Selective Update | Only fields provided with non-null values (Name, Email, Phone, etc.) are modified on the entity. |\n" +
                          "| **3. Consistency Sync** | Data Normalization | Invokes `userManager.UpdateAsync` to handle email/username normalization and persistent storage updates. |\n" +
                          "| **4. Persistence** | Database Commit | Triggers `unitOfWork.Save()` to finalize changes in the underlying SQL database. |\n\n" +
                          "#### **🛡️ Security Policies:**\n" +
                          "* **Selective Field Update**: Properties like `HireDate` or `Role` are generally ignored to prevent unauthorized privilege escalation. \n" +
                          "* **Identity Synchronization**: Changing the Email will automatically trigger a re-normalization of the security fields. \n\n" +
                          "> **⚠️ Warning:** Modifying the **Email** or **UserName** may affect the user's future login credentials.",
            Tags = new[] { "1. Security & Identity" }
        )]
        [SwaggerRequestExample(typeof(SpecificUserDataDTO), typeof(EditProfileRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EditProfileSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(EditProfileBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserData(SpecificUserDataDTO dto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id))
                    return BadRequest(new { Message = "Invalid user data." });

                var (isSuccess, message) = await _unitOfWork.AuthenticationService.UpdateUserData(dto);
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
