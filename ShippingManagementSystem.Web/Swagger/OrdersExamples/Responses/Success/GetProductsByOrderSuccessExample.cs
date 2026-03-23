using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success
{
    public class GetProductsByOrderSuccessExample : IExamplesProvider<List<ProductDTO>>
    {
        public List<ProductDTO> GetExamples()
        {
            return new List<ProductDTO>
            {
                new ProductDTO
                {
                    Id = 45,
                    Name = "Gaming Laptop",
                    Weight = 2500,
                    Quantity = 1,
                    OrderId = 1025
                },
                new ProductDTO
                {
                    Id = 46,
                    Name = "Cooling Pad",
                    Weight = 500,
                    Quantity = 1,
                    OrderId = 1025
                }
            };
        }
    }
}
