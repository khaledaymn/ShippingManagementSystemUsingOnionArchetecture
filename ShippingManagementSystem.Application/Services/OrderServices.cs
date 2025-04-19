using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Enums;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.OrderSpecification;
using ShippingManagementSystem.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingManagementSystem.Domain.UserTypes;

namespace ShippingManagementSystem.Application.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Get All Orders

        public async Task<PaginationResponse<OrderDTO>> GetAllOrdersAsync(OrderParams param)
        {
            try
            {
                var spec = new OrderSpecification(param);
                var orders = await _unitOfWork.Repository<Order>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<Order>().CountAsync(spec);
                
                var orderDTOs = orders.Select(MapOrderToDTO).ToList();
                
                return new PaginationResponse<OrderDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    orderDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving orders", ex);
            }
        }

        #endregion


        #region Get Order By Id
        public async Task<OrderDTO?> GetOrderByIdAsync(int id)
        {
            try
            {
                var spec = new OrderSpecification(id);
                var order = await _unitOfWork.Repository<Order>().GetBySpecAsync(spec);
                
                if (order == null)
                    return null;
                
                return MapOrderToDTO(order);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order with id {id}", ex);
            }
        }

        #endregion


        #region Get Orders By Status
        public async Task<List<OrderDTO>> GetOrdersByStatusAsync(string status)
        {
            try
            {
                var spec = new OrderSpecification(status);
                var orders = await _unitOfWork.Repository<Order>().GetAllBySpecAsync(spec);
                
                return orders.Select(MapOrderToDTO).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving orders with status {status}", ex);
            }
        }

        #endregion


        #region Get Products By Order Id

        public async Task<List<ProductDTO>> GetAllProductsByOrderIdAsync(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetById(orderId);
                if (order == null)
                    return new List<ProductDTO>();
                
                // Get all products for the order
                var products = await _unitOfWork.Repository<Product>()
                    .Find(p => p.OrderId == orderId);
                
                return products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    Quantity = p.Quantity,
                    OrderId = p.OrderId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving products for order {orderId}", ex);
            }
        }

        #endregion


        #region Create Order
        public async Task<(bool IsSuccess, string Message)> CreateOrderAsync(CreateOrderDTO orderDTO)
        {
            try
            {
                // Validate relationships
                var city = await _unitOfWork.Repository<City>().GetById(orderDTO.CityId);
                if (city == null)
                    return (false, $"City with id {orderDTO.CityId} not found");
                
                var chargeType = await _unitOfWork.Repository<ChargeType>().GetById(orderDTO.ChargeTypeId);
                if (chargeType == null)
                    return (false, $"Charge type with id {orderDTO.ChargeTypeId} not found");
                
                var branch = await _unitOfWork.Repository<Branch>().GetById(orderDTO.BranchId);
                if (branch == null)
                    return (false, $"Branch with id {orderDTO.BranchId} not found");
                
                var merchant = await _unitOfWork.Repository<Merchant>().GetById(orderDTO.MerchantId);
                if (merchant == null)
                    return (false, $"Merchant with id {orderDTO.MerchantId} not found");
                
                // Calculate total weight from products
                int totalWeight = orderDTO.Products.Sum(p => p.Weight * p.Quantity);
                
                // Calculate charge price based on the algorithm provided
                double chargePrice = await CalculateChargePrice(orderDTO, totalWeight);
                
                // Create order
                var order = new Order
                {
                    CreationDate = DateTime.UtcNow,
                    CustomerName = orderDTO.CustomerName,
                    CustomerPhone1 = orderDTO.CustomerPhone1,
                    CustomerPhone2 = orderDTO.CustomerPhone2,
                    VillageAndStreet = orderDTO.VillageAndStreet,
                    Notes = orderDTO.Notes,
                    OrderPrice = orderDTO.OrderPrice,
                    ChargePrice = chargePrice,
                    OrderState = OrderState.New,
                    OrderType = Enum.Parse<OrderType>(orderDTO.OrderType),
                    PaymentType = Enum.Parse<PaymentType>(orderDTO.PaymentType),
                    TotalWeight = totalWeight,
                    ShippingToVillage = orderDTO.ShippingToVillage,
                    CityId = orderDTO.CityId,
                    ChargeTypeId = orderDTO.ChargeTypeId,
                    BranchId = orderDTO.BranchId,
                    MerchantId = orderDTO.MerchantId,
                    AmountReceived = CalculateAmountReceived(orderDTO.PaymentType, orderDTO.OrderPrice)
                };
                
                _unitOfWork.Repository<Order>().Add(order);
                await _unitOfWork.Save();
                
                // Add products
                foreach (var productDTO in orderDTO.Products)
                {
                    var product = new Product
                    {
                        Name = productDTO.Name,
                        Weight = productDTO.Weight,
                        Quantity = productDTO.Quantity,
                        OrderId = order.Id
                    };
                    
                    _unitOfWork.Repository<Product>().Add(product);
                }
                
                await _unitOfWork.Save();
                
                return (true, $"Order created successfully with ID: {order.Id}");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating order: {ex.Message}");
            }
        }

        #endregion


        #region Assign Order To Delivary
        public async Task<(bool IsSuccess, string Message)> AssignOrderToDelivaryAsync(int id, AssignOrderToDelivaryDTO statusDTO)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetById(id);
                
                if (order == null)
                    return (false, $"Order with id {id} not found");

                if (order.OrderState != OrderState.New)
                    return (false, $"Order already assigned to delivary.");
                
                // Update shipping representative if provided
                if (!string.IsNullOrEmpty(statusDTO.ShippigRepresentativeId))
                {
                    var rep = await _unitOfWork.Repository<ShippigRepresentative>().GetById(statusDTO.ShippigRepresentativeId);
                    if (rep == null)
                        return (false, $"Shipping representative with id {statusDTO.ShippigRepresentativeId} not found");
                    
                    order.ShippigRepresentativeId = statusDTO.ShippigRepresentativeId;
                    order.OrderState = OrderState.Pendding;
                }
                else
                    return (false, $"Shipping representative ID is required");
                                
                
                _unitOfWork.Repository<Order>().Update(order);
                await _unitOfWork.Save();
                
                return (true, $"Order status updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating order status: {ex.Message}");
            }
        }

        #endregion


        #region Update Order Status
        public async Task<(bool IsSuccess, string Message)> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO statusDTO)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetById(id);
                
                if (order == null)
                    return (false, $"Order with id {id} not found");
                
                // Update order status
                if (!string.IsNullOrEmpty(statusDTO.OrderState))
                {
                    if (Enum.TryParse<OrderState>(statusDTO.OrderState, out var orderState))
                    {
                        order.OrderState = orderState;
                    }
                    else
                    {
                        return (false, $"Invalid order status: {statusDTO.OrderState}");
                    }
                }
               

                // Update notes if provided
                if (!string.IsNullOrEmpty(statusDTO.Notes))
                    order.Notes = statusDTO.Notes;
                
                _unitOfWork.Repository<Order>().Update(order);
                await _unitOfWork.Save();
                
                return (true, $"Order status updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating order status: {ex.Message}");
            }
        }

        #endregion


        #region Delete Order
        public async Task<(bool IsSuccess, string Message)> DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetById(id);
                
                if (order == null)
                    return (false, $"Order with id {id} not found");
                
                // Soft delete
                order.IsDeleted = !order.IsDeleted;
                _unitOfWork.Repository<Order>().Update(order);
                
                await _unitOfWork.Save();
                
                return (true, $"Order deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting order: {ex.Message}");
            }
        }

        #endregion


        #region Helper Methods

        private OrderDTO MapOrderToDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                CreationDate = order.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                CustomerName = order.CustomerName,
                CustomerPhone1 = order.CustomerPhone1,
                CustomerPhone2 = order.CustomerPhone2,
                VillageAndStreet = order.VillageAndStreet,
                Notes = order.Notes,
                ChargePrice = order.ChargePrice,
                OrderState = order.OrderState.ToString(),
                OrderType = order.OrderType.ToString(),
                PaymentType = order.PaymentType.ToString(),
                OrderPrice = order.OrderPrice,
                TotalWeight = order.TotalWeight,
                IsDeleted = order.IsDeleted,
                ShippingToVillage = order.ShippingToVillage,
                AmountReceived = order.AmountReceived,
                CityId = order.CityId,
                CityName = order.City?.Name,
                ChargeTypeId = order.ChargeTypeId,
                ChargeTypeName = order.ChargeType?.Name,
                BranchId = order.BranchId,
                BranchName = order.Branches?.Name,
                MerchantId = order.MerchantId,
                MerchantName = order.Merchant?.StoreName,
                ShippigRepresentativeId = order.ShippigRepresentativeId,
                ShippigRepresentativeName = order.ShippigRepresentative?.User.Name,
                Products = order.Products?.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    Quantity = p.Quantity,
                    OrderId = p.OrderId
                }).ToList()
            };
        }
        
        private async Task<double> CalculateChargePrice(CreateOrderDTO order, int totalWeight)
        {
            double chargePrice = 0;

            try
            {
                // Get city info
                var city = await _unitOfWork.Repository<City>().GetById(order.CityId);
                if (city == null)
                    throw new Exception($"City with id {order.CityId} not found");
                
                // Get merchant info
                var merchant = await _unitOfWork.Repository<Merchant>().GetById(order.MerchantId);
                if (merchant == null)
                    throw new Exception($"Merchant with id {order.MerchantId} not found");
                
                // Get charge type info
                var chargeType = await _unitOfWork.Repository<ChargeType>().GetById(order.ChargeTypeId);
                if (chargeType == null)
                    throw new Exception($"Charge type with id {order.ChargeTypeId} not found");


                var specialPrice = merchant.MerchantSpecialPrices.FirstOrDefault(x => x.CityId == order.CityId);
                
                if (specialPrice != null)
                    chargePrice += specialPrice.SpecialPrice;
                else
                    chargePrice += city.ChargePrice;

                
                // Add village price if shipping to village
                if (order.ShippingToVillage)
                {
                    // You might need to get this from a settings/standard entity
                    chargePrice += _unitOfWork.Repository<Standard>().GetById(1).Result.VillagePrice; // Default village price, adjust as needed
                }
                
                // Add charge for extra weight
                // You might need to get standard weight and KG price from a settings entity
                int standardWeight = _unitOfWork.Repository<Standard>().GetById(1).Result.StandardWeight; // Default standard weight, adjust as needed
                double kgPrice = _unitOfWork.Repository<Standard>().GetById(1).Result.KGprice; // Default price per KG, adjust as needed
                
                if (totalWeight > standardWeight)
                {
                    var extraWeight = totalWeight - standardWeight;
                    var extraPrice = extraWeight * kgPrice;
                    chargePrice += extraPrice;
                }

                // Add pickup price if needed
                if (order.OrderType == OrderType.PickupFromTheMerchant.ToString())
                {
                    // Use merchant's special pickup price if available
                    if (merchant.SpecialPickUp.HasValue)
                    {
                        chargePrice += merchant.SpecialPickUp.Value;
                    }
                    else
                    {
                        chargePrice += city.PickUpPrice;
                    }
                }

                // Add charge type extra price
                chargePrice += chargeType.ExtraPrice;
                
                return chargePrice;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in calculating charge price", ex);
            }
        }
        
        private double CalculateAmountReceived(string paymentType, double orderPrice)
        {
            switch (paymentType)
            {
                case "CashOnDelivery":
                    return orderPrice;
                case "ExchangeOrder":
                    return 0;
                default:
                    return 0;
            }
        }

        #endregion

    }
} 