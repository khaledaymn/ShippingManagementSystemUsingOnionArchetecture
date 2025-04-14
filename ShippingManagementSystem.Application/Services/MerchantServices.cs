using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class MerchantServices : IMerchantServices
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;

        public MerchantServices(IUnitOfWork unit, UserManager<ApplicationUser> userManager)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        #region Add Merchant
        public async Task<(bool IsSuccess, string Message)> AddMerchantAsync(AddMerchantDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email))
                return (false, "Invalid merchant data.");

            var newUser = new ApplicationUser
            {
                Name = dto.Name,
                UserName = dto.Name.Replace(" ", ""),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsDeleted = false,
                HiringDate = DateTime.Now,
            };

            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(newUser, dto.Password);
                    if (!result.Succeeded)
                    {
                        await _unit.RollbackAsync();
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        return (false, $"Create failed: {errors}");
                    }

                    var merchant = new Merchant
                    {
                        UserID = newUser.Id,
                        StoreName = dto.StoreName,
                        RejectedOrederPercentage = dto.RejectedOrderPercentage,
                        SpecialPickUp = dto.SpecialPickUp
                    };
                    await _unit.Repository<Merchant>().Add(merchant);

                    if (dto.CityIds != null && dto.CityIds.Any())
                    {
                        foreach (var cityId in dto.CityIds)
                        {
                            var merchantCity = new MerchantCity
                            {
                                MerchantId = newUser.Id,
                                CityId = cityId
                            };
                            await _unit.Repository<MerchantCity>().Add(merchantCity);
                        }
                    }

                    // Add special prices for cities if provided
                    if (dto.SpecialPrices != null && dto.SpecialPrices.Any())
                    {
                        foreach (var specialPrice in dto.SpecialPrices)
                        {
                            var merchantSpecialPrice = new MerchantSpecialPrice
                            {
                                MerchantId = newUser.Id,
                                CityId = specialPrice.CityId,
                                SpecialPrice = specialPrice.SpecialPrice
                            };
                            await _unit.Repository<MerchantSpecialPrice>().Add(merchantSpecialPrice);
                        }
                    }

                    await _unit.Save();
                    await transaction.CommitAsync();

                    return (true, "Merchant created successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }
        #endregion


        #region Get All Merchants
        //public async Task<PaginationResponse<MerchantDTO>> GetAllMerchantsAsync(MerchantParams param)
        //{
        //    var spec = new MerchantSpecification(param);
        //    var merchants = await _unit.Repository<Merchant>().GetAllBySpecAsync(spec);

        //    return merchants.Select(m => new MerchantDTO
        //    {
        //        Id = m.UserID,
        //        Name = m.User.Name,
        //        Email = m.User.Email,
        //        PhoneNumber = m.User.PhoneNumber,
        //        StoreName = m.StoreName,
        //        RejectedOrderPercentage = m.RejectedOrederPercentage,
        //        SpecialPickUp = m.SpecialPickUp,
        //        HiringDate = m.User.HiringDate,
        //        MerchantCities = m.MerchantCities?.Select(mc => mc.CityId.ToString()).ToList() ?? new List<string>(),
        //        SpecialPrices = m.MerchantSpecialPrices?.Select(msp => new MerchantSpecialPriceDTO
        //        {
        //            CityId = msp.CityId,
        //            SpecialPrice = msp.SpecialPrice
        //        }).ToList() ?? new List<MerchantSpecialPriceDTO>()
        //    }).ToList();
        //}

        public async Task<PaginationResponse<MerchantDTO>> GetAllMerchantsAsync(MerchantParams param)
        {
            var spec = new MerchantSpecification(param);

            var merchants = await _unit.Repository<Merchant>().GetAllBySpecAsync(spec);

            var totalCount = await _unit.Repository<Merchant>().CountAsync(spec);

            var result = merchants.Select(m => new MerchantDTO
            {
                Id = m.UserID,
                Name = m.User?.Name ?? string.Empty,
                Email = m.User?.Email ?? string.Empty,
                PhoneNumber = m.User?.PhoneNumber ?? string.Empty,
                StoreName = m.StoreName ?? string.Empty,
                RejectedOrderPercentage = m.RejectedOrederPercentage,
                SpecialPickUp = m.SpecialPickUp,
                HiringDate = m.User?.HiringDate ?? DateTime.UtcNow,
                MerchantCities = m.MerchantCities?.Select(mc => mc.CityId.ToString()).ToList() ?? new List<string>(),
                SpecialPrices = m.MerchantSpecialPrices?.Select(msp => new MerchantSpecialPriceDTO
                {
                    CityId = msp.CityId,
                    SpecialPrice = msp.SpecialPrice
                }).ToList() ?? new List<MerchantSpecialPriceDTO>()
            }).ToList();

            return new PaginationResponse<MerchantDTO>(
                pageSize: param.PageSize,
                pageIndex: param.PageIndex,
                totalCount: totalCount,
                data: result.AsReadOnly()
            );
        }
        #endregion


        #region Get Merchant By Id
        public async Task<MerchantDTO?> GetMerchantByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var spec = new MerchantSpecification(id);
            var merchant = await _unit.Repository<Merchant>().GetBySpecAsync(spec);

            if (merchant == null)
                return null;

            return new MerchantDTO
            {
                Id = merchant.UserID,
                Name = merchant.User.Name,
                Email = merchant.User.Email,
                PhoneNumber = merchant.User.PhoneNumber,
                StoreName = merchant.StoreName,
                RejectedOrderPercentage = merchant.RejectedOrederPercentage,
                SpecialPickUp = merchant.SpecialPickUp,
                HiringDate = merchant.User.HiringDate,
                MerchantCities = merchant.MerchantCities?.Select(mc => mc.CityId.ToString()).ToList() ?? new List<string>(),
                SpecialPrices = merchant.MerchantSpecialPrices?.Select(msp => new MerchantSpecialPriceDTO
                {
                    CityId = msp.CityId,
                    SpecialPrice = msp.SpecialPrice
                }).ToList() ?? new List<MerchantSpecialPriceDTO>()
            };
        }
        #endregion


        #region Update Merchant
        public async Task<(bool IsSuccess, string Message)> UpdateMerchantAsync(UpdateMerchantDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Id))
                return (false, "Invalid merchant data.");

            var spec = new MerchantSpecification(dto.Id);
            var merchant = await _unit.Repository<Merchant>().GetBySpecAsync(spec);

            if (merchant == null)
                return (false, "Merchant not found.");

            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    // Update ApplicationUser properties - only update properties that are not null
                    var user = merchant.User;
                    bool userUpdated = false;

                    if (!string.IsNullOrEmpty(dto.Name))
                    {
                        user.Name = dto.Name;
                        userUpdated = true;
                    }

                    if (!string.IsNullOrEmpty(dto.Email))
                    {
                        user.Email = dto.Email;
                        userUpdated = true;
                    }

                    if (!string.IsNullOrEmpty(dto.PhoneNumber))
                    {
                        user.PhoneNumber = dto.PhoneNumber;
                        userUpdated = true;
                    }

                    // Only update user if any properties changed
                    if (userUpdated)
                    {
                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                            return (false, $"Update failed: {errors}");
                        }
                    }

                    // Update Merchant properties - only update properties that are not null
                    bool merchantUpdated = false;

                    if (!string.IsNullOrEmpty(dto.StoreName))
                    {
                        merchant.StoreName = dto.StoreName;
                        merchantUpdated = true;
                    }

                    // RejectedOrderPercentage is now nullable, so we check if it has a value
                    if (dto.RejectedOrderPercentage.HasValue)
                    {
                        merchant.RejectedOrederPercentage = dto.RejectedOrderPercentage.Value;
                        merchantUpdated = true;
                    }

                    // SpecialPickUp is nullable, so we can directly assign it
                    if (dto.SpecialPickUp.HasValue)
                    {
                        merchant.SpecialPickUp = dto.SpecialPickUp;
                        merchantUpdated = true;
                    }

                    // Only update merchant if any properties changed
                    if (merchantUpdated)
                    {
                        _unit.Repository<Merchant>().Update(merchant);
                    }

                    // Update merchant cities if provided
                    if (dto.CityIds != null && dto.CityIds.Any())  // Only process if CityIds is not null and has items
                    {
                        // Get existing merchant cities
                        var existingCities = merchant.MerchantCities?.ToList() ?? new List<MerchantCity>();
                        
                        // Only add new cities that don't exist
                        // We don't remove existing cities that are not in the DTO
                        var existingCityIds = existingCities.Select(c => c.CityId).ToList();
                        foreach (var cityId in dto.CityIds)
                        {
                            if (!existingCityIds.Contains(cityId))
                            {
                                var merchantCity = new MerchantCity
                                {
                                    MerchantId = dto.Id,
                                    CityId = cityId
                                };
                                await _unit.Repository<MerchantCity>().Add(merchantCity);
                            }
                        }
                    }
                    // Note: We don't remove any existing cities that aren't in the DTO
                    // This allows for partial updates of cities
                    
                    // Update merchant special prices if provided
                    if (dto.SpecialPrices != null && dto.SpecialPrices.Any())  // Only process if SpecialPrices is not null and has items
                    {
                        // Get existing merchant special prices
                        var existingSpecialPrices = merchant.MerchantSpecialPrices?.ToList() ?? new List<MerchantSpecialPrice>();
                        
                        // Only update the special prices that are provided in the DTO
                        // We don't remove existing special prices that are not in the DTO
                        foreach (var specialPrice in dto.SpecialPrices)
                        {
                            // Only process special prices with valid data
                            if (specialPrice.CityId > 0 && specialPrice.SpecialPrice >= 0)
                            {
                                var existingSpecialPrice = existingSpecialPrices.FirstOrDefault(sp => sp.CityId == specialPrice.CityId);
                                
                                if (existingSpecialPrice == null)
                                {
                                    // Add new special price
                                    var merchantSpecialPrice = new MerchantSpecialPrice
                                    {
                                        MerchantId = dto.Id,
                                        CityId = specialPrice.CityId,
                                        SpecialPrice = specialPrice.SpecialPrice
                                    };
                                    await _unit.Repository<MerchantSpecialPrice>().Add(merchantSpecialPrice);
                                }
                                else
                                {
                                    // Update existing special price
                                    existingSpecialPrice.SpecialPrice = specialPrice.SpecialPrice;
                                    _unit.Repository<MerchantSpecialPrice>().Update(existingSpecialPrice);
                                }
                            }
                        }
                    }
                    // Note: We don't remove any existing special prices that aren't in the DTO
                    // This allows for partial updates of special prices
                    // Note: We don't remove any existing special prices that aren't in the DTO
                    // This allows for partial updates of special prices

                    await _unit.Save();
                    await transaction.CommitAsync();

                    return (true, "Merchant updated successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }
        #endregion


        #region Delete Merchant
        public async Task<(bool IsSuccess, string Message)> DeleteMerchantAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return (false, "Invalid merchant ID.");

            var spec = new MerchantSpecification(id);
            var merchant = await _unit.Repository<Merchant>().GetBySpecAsync(spec);

            if (merchant == null)
                return (false, "Merchant not found.");

            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    // Soft delete the user
                    var user = merchant.User;
                    user.IsDeleted = true;
                    var updateResult = await _userManager.UpdateAsync(user);

                    if (!updateResult.Succeeded)
                    {
                        await _unit.RollbackAsync();
                        var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                        return (false, $"Delete failed: {errors}");
                    }

                    await _unit.Save();
                    await transaction.CommitAsync();

                    return (true, "Merchant deleted successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }
        #endregion
    }
}