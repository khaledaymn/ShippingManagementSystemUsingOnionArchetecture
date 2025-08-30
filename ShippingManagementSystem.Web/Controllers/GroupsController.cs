using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.Specifications.GroupSpecification;
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