using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Requests
{
    public class AddEmployeeRequestExample : IExamplesProvider<AddEmployeeDTO>
    {
        public AddEmployeeDTO GetExamples() => new AddEmployeeDTO
        {
            Name = "Maged Mohamed",
            Email = "maged.m@shipping.com",
            PhoneNumber = "01122334455",
            Address = "Dokki, Giza",
            Password = "SecurePassword123!",
            GroupId = 3, 
            BranchIds = new List<int> { 1, 5 } 
        };
    }
}
