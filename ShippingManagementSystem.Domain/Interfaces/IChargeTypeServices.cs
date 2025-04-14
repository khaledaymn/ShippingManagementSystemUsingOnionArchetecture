using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IChargeTypeServices
    {
        Task<PaginationResponse<ChargeTypeDTO>> GetAllChargeTypesAsync(ChargeTypeParams param);
        Task<ChargeTypeDTO?> GetChargeTypeByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateChargeTypeAsync(CreateChargeTypeDTO chargeTypeDTO);
        Task<(bool IsSuccess, string Message)> UpdateChargeTypeAsync(int id, ChargeTypeDTO chargeTypeDTO);
        Task<(bool IsSuccess, string Message)> DeleteChargeTypeAsync(int id);
    }
} 