using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using ShippingManagementSystem.Domain.Specifications.OrderSpecification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IOrderServices
    {
        Task<PaginationResponse<OrderDTO>> GetAllOrdersAsync(OrderParams param);
        Task<OrderDTO?> GetOrderByIdAsync(int id);
        Task<List<OrderDTO>> GetOrdersByStatusAsync(string status);
        Task<List<ProductDTO>> GetAllProductsByOrderIdAsync(int orderId);
        Task<(bool IsSuccess, string Message)> CreateOrderAsync(CreateOrderDTO orderDTO);
        Task<(bool IsSuccess, string Message)> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO statusDTO);
        Task<(bool IsSuccess, string Message)> DeleteOrderAsync(int id);
        Task<(bool IsSuccess, string Message)> AssignOrderToDelivaryAsync(int id, AssignOrderToDelivaryDTO statusDTO);
    }
} 