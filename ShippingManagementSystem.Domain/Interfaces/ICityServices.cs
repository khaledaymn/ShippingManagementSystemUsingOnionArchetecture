using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface ICityServices
    {
        Task<PaginationResponse<CityDTO>> GetAllCitiesAsync(CityParams param);
        Task<CityDTO?> GetCityByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateCityAsync(CreateCityDTO cityDTO);
        //Task<(bool IsSuccess, string Message)> UpdateCityAsync(int id, CityDTO cityDTO);
        Task<(bool IsSuccess, string Message)> DeleteCityAsync(int id);
        Task<(bool IsSuccess, EditCityDTO)> EditCityAsync(EditCityDTO cityDTO);
    }
} 