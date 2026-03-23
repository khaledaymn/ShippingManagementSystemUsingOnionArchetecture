using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.Specifications.GroupSpecification;
using ShippingManagementSystem.Web.Swagger.GroupsExamples.Requests;
using ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Success;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get All
        [HttpGet]
        [Route("~/Group/GetAll")]
        [Authorize(Policy =
            $"Permission={Permissions.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "GetPermissionGroupsList",
            Summary = "Browse all security and permission groups",
            Description = "### 🔐 Access Control Catalog\n\n" +
                          "Retrieves a paginated collection of security groups defined in the system. These groups serve as **Role Templates** that aggregate specific module permissions.\n\n" +
                          "#### **⚙️ Advanced Filtering & Sorting:**\n" +
                          "* **Temporal Filtering**: Use `FromDate` and `ToDate` to find groups created during specific configuration sprints.\n" +
                          "* **Keyword Search**: Filters by group name (e.g., 'Finance', 'Logistics').\n" +
                          "* **Pagination Control**: Optimized for administrative tables with a default page size of **100 records**.\n\n" +
                          "#### **📊 Contextual Usage:**\n" +
                          "This list is typically used in the **Employee Creation/Update** forms to assign a security tier to a new staff member.\n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Groups fetched and paginated successfully. | `PaginationResponse<GroupDTO>` |\n" +
                          "| **400 Bad** | Application-level exception or invalid sort parameter. | `'Error retrieving groups: {msg}'` |\n" +
                          "| **500 Err** | Critical infrastructure or DB connectivity issue. | `'Error retrieving groups: {msg}'` |\n\n" +
                          "> **Tip:** Use the `Sort` parameter with values like `name_asc` or `date_desc` to organize your management table.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerRequestExample(typeof(GroupParams), typeof(GroupParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllGroupsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GroupErrorStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupInternalServerErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<GroupDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGroups([FromQuery] GroupParams param)
        {
            try
            {
                var result = await _unitOfWork.GroupService.GetAllGroupsAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving groups: {ex.Message}");
            }
        }
        #endregion

        #region Get By Id
        [HttpGet]
        [Route("~/Group/GetById/{id}")]
        [Authorize(Policy =
            $"Permission={Permissions.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "GetPermissionGroupDetails",
            Summary = "Fetch a group's full permission matrix",
            Description = "### 🔍 Granular Security Mapping\n\n" +
                          "Retrieves the complete configuration of a security group, specifically its **Module-to-Permission Mapping**. This is the primary data source for Role-Based Access Control (RBAC) management interfaces.\n\n" +
                          "#### **⚙️ Data Architecture:**\n" +
                          "* **Permissions Matrix**: Returned as a `Dictionary<string, List<int>>` where:\n" +
                          "  * **Key**: The logical name of the system Module (e.g., 'Orders', 'Users').\n" +
                          "  * **Value**: A list of integer IDs representing the active permissions (mapped to the `Permission` Enum).\n\n" +
                          "#### **🏗️ UI Implementation Guide:**\n" +
                          "Developers should use this matrix to pre-check the corresponding 'Checkboxes' in the permissions grid when a user opens the 'Edit Group' modal.\n\n" +
                          "#### **⚠️ Failure Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Success. Returns ID, Name, and Permissions Matrix. | `GroupPermissionsDTO (JSON)` |\n" +
                          "| **400 Bad** | Managed Exception during data retrieval. | `'Error retrieving group: {msg}'` |\n" +
                          "| **404 Not** | Provided ID does not exist in the Database. | `'Group with ID {id} not found'` |\n" +
                          "| **500 Err** | Database connectivity or Infrastructure failure. | `'Error retrieving group: {msg}'` |\n\n" +
                          "> **Note:** The `Date` field is formatted as `dd/MM/yyyy` for display purposes.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GroupByIdErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(GroupNotFoundStringExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupByIdErrorExample))]
        [ProducesResponseType(typeof(GroupPermissionsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGroupById(int id)
        {
            try
            {
                var group = await _unitOfWork.GroupService.GetGroupByIdAsync(id);
                if (group == null)
                    return NotFound($"Group with ID {id} not found");

                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving group: {ex.Message}");
            }
        }

        #endregion

        #region Create Group

        [HttpPost]
        [Route("~/Group/Create")]
        [Authorize(Policy =
            $"Permission={Permissions.Create};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PostCreatePermissionGroup",
            Summary = "Define a new security group with specific module access",
            Description = "### 🏗️ Atomic Security Provisioning\n\n" +
                          "Creates a new permission group and maps it to various system modules. This operation is wrapped in a **Database Transaction** to ensure that no 'Empty Groups' are created without their associated permissions.\n\n" +
                          "#### **⚙️ Permission Structure:**\n" +
                          "* **Module Identification**: Each item in the `Permissions` list must have a valid `Id` corresponding to a system Module.\n" +
                          "* **Bitwise/Enum Values**: The `Values` array should contain the integer representations of the `Permission` Enum (e.g., 1 for View, 2 for Create).\n\n" +
                          "#### **🛡️ Validation Guardrails:**\n" +
                          "* **Empty Protection**: A group cannot be created without at least one assigned permission; otherwise, a **400 BadRequest** is returned.\n" +
                          "* **Transactional Integrity**: If the system fails to save any individual module link, the entire group creation is **Rolled Back**.\n\n" +
                          "#### **⚠️ Response Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **201 Created** | Group and its permission matrix successfully persisted. | `'Group {name} created successfully'` |\n" +
                          "| **400 Bad** | Missing permissions or invalid group data provided. | `'Permissions are required...'` |\n" +
                          "| **500 Err** | Infrastructure failure or Transaction deadlock. | `'Error creating group: {msg}'` |\n\n" +
                          "> **Note:** The created group can be immediately assigned to employees in the User Management module.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerRequestExample(typeof(CreateGroupDTO), typeof(CreateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(GroupCreateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GroupCreateErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupByIdErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO groupDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid group data provided.");

                var result = await _unitOfWork.GroupService.CreateGroupAsync(groupDTO);

                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return StatusCode(201, result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating group: {ex.Message}");
            }
        }

        #endregion

        #region Update Group
        [HttpPut]
        [Route("~/Group/Update/{id}")]
        [Authorize(Policy =
            $"Permission={Permissions.Edit};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "PutUpdatePermissionGroup",
            Summary = "Modify group name or redefine permission matrix",
            Description = "### 🔄 Dynamic Security Re-configuration\n\n" +
                          "Updates an existing permission group. This endpoint implements a **Full Permission Override** strategy to ensure absolute security consistency.\n\n" +
                          "#### **⚙️ Update Mechanics:**\n" +
                          "* **Purge & Populate**: If the `Permissions` list is provided, the system **deletes all existing module links** for this group and re-inserts the new ones from scratch.\n" +
                          "* **Conditional Name Update**: The group `Name` is only updated if a non-empty string is provided.\n\n" +
                          "#### **🛡️ Transactional Safety:**\n" +
                          "The entire process (Deletion of old and Insertion of new) is wrapped in a **Database Transaction**. If any step fails, the group's original permissions are fully restored via rollback.\n\n" +
                          "#### **⚠️ Response Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **200 OK** | Group and its matrix successfully synchronized. | `'Group {name} updated successfully'` |\n" +
                          "| **400 Bad** | Provided ID not found or invalid data format. | `'Group with id {id} not found'` |\n" +
                          "| **500 Err** | Failure during the 'Purge' or 'Re-insert' DB operations. | `'Error updating group: {msg}'` |\n\n" +
                          "> **Warning:** Updating a group's permissions immediately affects all employees assigned to this group. They may need to refresh their session to reflect new access levels.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerRequestExample(typeof(UpdateGroupDTO), typeof(UpdateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GroupUpdateSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GroupUpdateErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupByIdErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDTO groupDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid group data provided.");

                var result = await _unitOfWork.GroupService.UpdateGroupAsync(id, groupDTO);

                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating group: {ex.Message}");
            }
        }
        #endregion

        #region Delete Group
        [HttpDelete]
        [Route("~/Group/Delete/{id}")]
        [Authorize(Policy =
            $"Permission={Permissions.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "DeletePermissionGroup",
            Summary = "Permanently remove a permission group",
            Description = "### ⚠️ Irreversible Security Purge\n\n" +
                          "Deletes a permission group and all its associated module mappings. This endpoint implements a **Strict Dependency Check** to protect system stability.\n\n" +
                          "#### **🚫 Deletion Constraints:**\n" +
                          "* **User Association Lock**: If any employee is currently assigned to this group, the deletion will be **Blocked** with a 400 error. You must reassign those users first.\n" +
                          "* **Cascade Cleanup**: If the group is eligible for deletion, the system automatically purges all linked records in the `GroupMedules` table before removing the group itself.\n\n" +
                          "#### **⚠️ Response Matrix (Plain Text):**\n" +
                          "| Status | Scenario | Format |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **204 OK** | Group and its mappings successfully purged. | `'Group {name} deleted successfully'` |\n" +
                          "| **400 Bad** | Group has active users or ID does not exist. | `'Group {name} cannot be deleted...'` |\n" +
                          "| **500 Err** | Database constraint violation or Infrastructure failure. | `'Error deleting group: {msg}'` |\n\n" +
                          "> **Best Practice:** Always verify that the group is empty (no assigned employees) via the Employees module before attempting a delete.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerResponseExample(StatusCodes.Status204NoContent, typeof(GroupDeleteSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(GroupDeleteDependencyErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(GroupByIdErrorExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var result = await _unitOfWork.GroupService.DeleteGroupAsync(id);

                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return StatusCode(204, result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting group: {ex.Message}");
            }
        }
        #endregion
    }
}