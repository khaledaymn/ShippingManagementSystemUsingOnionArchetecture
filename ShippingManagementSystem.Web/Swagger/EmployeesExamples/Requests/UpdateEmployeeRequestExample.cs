using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Requests
{
    public class UpdateEmployeeRequestExample : IExamplesProvider<UpdateEmployeeDTO>
    {
        public UpdateEmployeeDTO GetExamples() => new UpdateEmployeeDTO
        {
            Id = "f47ac10b-58cc-4372-a567-0e02b2c3d479",
            Name = "Ahmed Samy (Updated)",
            PhoneNumber = "01000000000",
            GroupId = 2, 
            BranchIds = new List<int> { 1, 3 }
        };
    }
}
