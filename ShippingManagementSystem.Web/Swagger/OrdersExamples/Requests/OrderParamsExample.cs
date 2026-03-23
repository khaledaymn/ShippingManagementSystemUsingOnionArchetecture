using ShippingManagementSystem.Domain.Specifications.OrderSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Requests
{
    public class OrderParamsExample : IExamplesProvider<OrderParams>
    {
        public OrderParams GetExamples() => new OrderParams
        {
            Search = "Khaled",
            OrderState = "Pendding",
            CityId = 5,
            PageIndex = 1,
            PageSize = 10,
            Sort = "date_desc"
        };
    }
}
