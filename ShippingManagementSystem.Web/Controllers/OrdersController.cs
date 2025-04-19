using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.OrderSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("~/Orders/GetAll")]
        //[Authorize(Policy = Orders.View)]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderParams param)
        {
            var result = await _unitOfWork.OrderService.GetAllOrdersAsync(param);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/Orders/GetById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _unitOfWork.OrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found");
            
            return Ok(order);
        }
        
        [HttpGet]
        [Route("~/Orders/GetByStatus/{status}")]
        //[Authorize(Policy = Orders.View)]
        public async Task<IActionResult> GetOrdersByStatus(string status)
        {
            var orders = await _unitOfWork.OrderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }
        
        [HttpGet]
        [Route("~/Orders/GetProductsByOrderId/{orderId}")]
        //[Authorize(Policy = Orders.View)]
        public async Task<IActionResult> GetProductsByOrderId(int orderId)
        {
            var products = await _unitOfWork.OrderService.GetAllProductsByOrderIdAsync(orderId);
            return Ok(products);
        }

        [HttpPost]
        [Route("~/Orders/CreateOrder")]
        //[Authorize(Policy = Orders.Create)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.OrderService.CreateOrderAsync(orderDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("~/Orders/UpdateOrder/{id}")]
        //[Authorize(Policy = Orders.Edit)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO statusDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.OrderService.UpdateOrderStatusAsync(id, statusDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("~/Orders/DeleteOrder/{id}")]
        //[Authorize(Policy = Orders.Delete)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _unitOfWork.OrderService.DeleteOrderAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }
    }
} 