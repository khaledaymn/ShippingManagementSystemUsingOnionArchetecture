using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using ShippingManagementSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class CityServices : ICityServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public CityServices(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public async Task<PaginationResponse<CityDTO>> GetAllCitiesAsync(CityParams param)
        {
            try
            {
                var spec = new CitySpecification(param);
                var cities = await _unitOfWork.Repository<City>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<City>().CountAsync(spec);
                
                var cityDTOs = cities.Select(c => new CityDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    ChargePrice = c.ChargePrice,
                    PickUpPrice = c.PickUpPrice,
                    GovernorateName = c.Governorate?.Name,
                    IsDeleted = c.IsDeleted
                }).ToList();
                
                return new PaginationResponse<CityDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    cityDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving cities", ex);
            }
        }
        public async Task<CityDTO?> GetCityByIdAsync(int id)
        {
            try
            {
                var spec = new CitySpecification(id);
                var city = await _unitOfWork.Repository<City>().GetBySpecAsync(spec);
                
                if (city == null)
                    return null;
                
                return new CityDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    ChargePrice = city.ChargePrice,
                    PickUpPrice = city.PickUpPrice,
                    GovernorateName = city.Governorate?.Name,
                    IsDeleted = city.IsDeleted
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving city with id {id}", ex);
            }
        }
        public async Task<(bool IsSuccess, string Message)> CreateCityAsync(CreateCityDTO cityDTO)
        {
            try
            {
                // Check if governorate exists
                var governorate = await _unitOfWork.Repository<Governorate>().GetById(cityDTO.GovernorateId);
                if (governorate == null)
                    return (false, $"Governorate with id {cityDTO.GovernorateId} not found");

                var city = new City
                {
                    Name = cityDTO.Name,
                    ChargePrice = cityDTO.ChargePrice,
                    PickUpPrice = cityDTO.PickUpPrice,
                    GovernorateId = cityDTO.GovernorateId,
                    IsDeleted = false
                };
                
                var add = _context.Cities.Add(city);
                await _unitOfWork.Save();
                
                return (true, $"City '{city.Name}' created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating city: {ex.Message}");
            }
        }
        public async Task<(bool IsSuccess, EditCityDTO)> EditCityAsync(EditCityDTO cityDTO)
        {
            try
            {
                var city = await _unitOfWork.Repository<City>().GetById(cityDTO.Id);

                if (city == null)
                    return (false, null);

                // Only update properties that are provided
                if (!string.IsNullOrEmpty(cityDTO.Name))
                {
                    city.Name = cityDTO.Name;
                }

                if (cityDTO.ChargePrice!=null&&cityDTO.ChargePrice > 0)
                {
                    city.ChargePrice = cityDTO.ChargePrice??0;
                }

                if (cityDTO.PickUpPrice!=null&&cityDTO.PickUpPrice > 0)
                {
                    city.PickUpPrice = cityDTO.PickUpPrice??0;
                }

                if (cityDTO.GovernorateId!=null &&cityDTO.GovernorateId > 0 && cityDTO.GovernorateId != city.GovernorateId)
                {
                    var governorate = await _unitOfWork.Repository<Governorate>().GetById(cityDTO.GovernorateId);
                    if (governorate == null)
                        return (false, null);

                    city.GovernorateId = cityDTO.GovernorateId ?? 0;
                }

                _unitOfWork.Repository<City>().Update(city);
                await _unitOfWork.Save();

                return (true, cityDTO);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return (false, null);
            }
        }
        public async Task<(bool IsSuccess, string Message)> DeleteCityAsync(int id)
        {
            try
            {
                var city = await _unitOfWork.Repository<City>().GetById(id);
                
                if (city == null)
                    return (false, $"City with id {id} not found");
                
                // Soft delete
                city.IsDeleted = !city.IsDeleted;
                _unitOfWork.Repository<City>().Update(city);
                
                // Hard delete if needed
                // _unitOfWork.Repository<City>().Delete(city);
                
                await _unitOfWork.Save();
                
                return (true, $"City '{city.Name}' deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting city: {ex.Message}");
            }
        }
    }
} 