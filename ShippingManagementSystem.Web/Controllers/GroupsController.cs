using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.GroupSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("~/Group/GetAll")]
        //[Authorize(Policy = Permissions.View)]
        public async Task<IActionResult> GetAllGroups([FromQuery] GroupParams param)
        {
            var result = await _unitOfWork.GroupService.GetAllGroupsAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/Group/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _unitOfWork.GroupService.GetGroupByIdAsync(id);
            if (group == null)
                return NotFound($"Group with ID {id} not found");
            
            return Ok(group);
        }

        [HttpPost]
        [Route("~/Group/Create")]
        //[Authorize(Policy = Permissions.Create)]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO groupDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.GroupService.CreateGroupAsync(groupDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/Group/Update/{id}")]
        //[Authorize(Policy = Permissions.Edit)]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDTO groupDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.GroupService.UpdateGroupAsync(id, groupDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/Group/Delete/{id}")]
        //[Authorize(Policy = Permissions.Delete)]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var result = await _unitOfWork.GroupService.DeleteGroupAsync(id);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }
    }
} 