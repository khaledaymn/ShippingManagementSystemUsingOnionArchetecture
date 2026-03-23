using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Success
{
    public class GetEmployeeByIdSuccessExample : IExamplesProvider<EmployeeDTO>
    {
        public EmployeeDTO GetExamples() => new EmployeeDTO
        {
            Id = "f47ac10b-58cc-4372-a567-0e02b2c3d479",
            Name = "Ahmed Samy",
            Email = "ahmed.samy@shipping.com",
            PhoneNumber = "01012345678",
            CreationDate = "2024-03-15",
            Permission = "General Supervisor",
            Address = "Zamalek, Cairo",
            IsDeleted = false,
            Branches = new List<GetBranchDTO>
            {
                new GetBranchDTO { Id = 1, Name = "Cairo Hub", IsDeleted = false },
                new GetBranchDTO { Id = 5, Name = "Alex Hub", IsDeleted = false }
            }
        };
    }
}
