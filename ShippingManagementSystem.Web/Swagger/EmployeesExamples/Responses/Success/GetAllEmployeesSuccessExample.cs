using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Success
{
    public class GetAllEmployeesSuccessExample : IExamplesProvider<PaginationResponse<EmployeeDTO>>
    {
        public PaginationResponse<EmployeeDTO> GetExamples()
        {
            var data = new List<EmployeeDTO>
            {
                new EmployeeDTO
                {
                    Id = "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                    Name = "Ahmed Ali",
                    Email = "ahmed.ali@company.com",
                    PhoneNumber = "01012345678",
                    CreationDate = "2024-01-10",
                    Permission = "Admin",
                    Address = "Maadi, Cairo",
                    IsDeleted = false,
                    Branches = new List<GetBranchDTO>
                    {
                        new GetBranchDTO { Id = 1, Name = "Main Hub", CityName = "Cairo", IsDeleted = false },
                        new GetBranchDTO { Id = 2, Name = "Giza Station", CityName = "Giza", IsDeleted = false }
                    }
                }
            };
            return new PaginationResponse<EmployeeDTO>(10, 1, 1, data);
        }
    }
}
