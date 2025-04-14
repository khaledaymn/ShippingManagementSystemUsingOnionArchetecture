using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using ShippingManagementSystem.Domain.Specifications.MeduleSpecification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IMeduleServices
    {
        Task<PaginationResponse<MeduleDTO>> GetAllMedulesAsync(MeduleParams param);
        Task<MeduleDTO?> GetMeduleByIdAsync(int id);
    }
} 