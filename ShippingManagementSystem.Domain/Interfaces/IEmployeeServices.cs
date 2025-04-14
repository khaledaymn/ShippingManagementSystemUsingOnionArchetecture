using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IEmployeeServices
    {
        Task<(bool IsSuccess, string Message)> AddEmployeeAsync(AddEmployeeDTO dto);
        Task<PaginationResponse<EmployeeDTO>> GetAllEmployeesAsync(EmployeeParams param);
        Task<EmployeeDTO?> GetEmployeeByIdAsync(string id);
        Task<(bool IsSuccess, string Message)> UpdateEmployeeAsync(UpdateEmployeeDTO dto);
        Task<(bool IsSuccess, string Message)> DeleteEmployeeAsync(string id);
    }
}
