#region Usings 
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDtos;
using ShippingManagementSystem.Application.Exptions;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerhantSpeialPricesSpeifications;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.UserBranches;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingManagementSystem.Domain.DTOs.MerchantDTOs;

#endregion

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

            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return (false, "Email already Exists");
            var random = new Random().Next(00, 99);

            var newUser = new ApplicationUser
            {
                Name = dto.Name,
                UserName = _userManager.FindByNameAsync(dto.Name.Replace(" ", "")).Result is not null ? dto.Name.Replace(" ", "") + random : dto.Name.Replace(" ", ""),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsDeleted = false,
                HiringDate = DateTime.Now,
                Address = dto.Address,
            };

            using var transaction = await _unit.BeginTransactionAsync();

            try
            {
                var result = await _userManager.CreateAsync(newUser, dto.Password);
                if (!result.Succeeded)
                {
                    await _unit.RollbackAsync();
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return (false, $"Create failed: {errors}");
                }

                await _userManager.AddToRoleAsync(newUser, Roles.Merchant);

                var merchant = new Merchant
                {
                    UserID = newUser.Id,
                    StoreName = dto.StoreName,
                    RejectedOrederPercentage = dto.RejectedOrderPrecentage,
                    SpecialPickUp = dto.SpecialPickUp
                };

                await _unit.Repository<Merchant>().Add(merchant);

                if (dto.BranchesIds?.Count > 0)
                {
                    var userBranches = dto.BranchesIds
                        .Select(branchId => new UserBranches(newUser.Id, branchId))
                        .ToList();

                    await _unit.Repository<UserBranches>().AddRange(userBranches);
                }

                if (dto.SpecialDeliveryPrices != null && dto.SpecialDeliveryPrices.Any())
                {
                    foreach (var specialPrice in dto.SpecialDeliveryPrices)
                    {
                        var merchantSpecialPrice = new MerchantSpecialPrice
                        {
                            MerchantId = newUser.Id,
                            CityId = specialPrice.cityId,
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
        #endregion

        #region Get All Merchants

        public async Task<PaginationResponse<MerchantDTO>> GetAllMerchantsAsync(MerchantParams param)
        {
            var spec = new MerchantSpecification(param);

            var merchants = await _unit.Repository<Merchant>().GetAllBySpecAsync(spec);

            var totalCount = await _unit.Repository<Merchant>().CountAsync(spec);
            var SpecialDeliveryPrices = merchants.Select(merchant => new {
                ListPrices = merchant.MerchantSpecialPrices?.Select(msp => new
                {
                    cityId = msp.CityId,
                    SpecialPreice = msp.SpecialPrice,
                    CityName = msp.City.Name,

                }).ToList(),
            });

            var result = merchants.Select(merchant => new MerchantDTO
            {
                Id = merchant.User.Id,
                Address = merchant.User.Address,
                Email = merchant.User.Email,
                SpecialPickUp = merchant.SpecialPickUp,
                StartWorkDate = merchant.User.HiringDate,
                PhoneNumber = merchant.User.PhoneNumber,
                StoreName = merchant.StoreName,
                Branches = merchant.User.UserBranches?
                    .Select(ub => new GetBranchDTO
                    {
                        Id = ub.BranchId,
                        Name = ub.Branch?.Name ?? string.Empty,
                        CreationDate = ub.Branch?.CreationDate.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                        IsDeleted = ub.Branch?.IsDeleted ?? false,
                        CityName = ub.Branch?.City?.Name,
                        Location = ub.Branch?.Location
                    })
                    .ToList() ?? new List<GetBranchDTO>(),
                Name = merchant.User.Name,
                SpecialDeliveryPrices = merchant.MerchantSpecialPrices?.Select(msp => new SpecialDeliveryPriceDTO
                {
                    cityId = msp.CityId,
                    SpecialPrice = msp.SpecialPrice,
                    CityName = msp.City.Name,
                    
                }).ToList(),
                IsDeleted = merchant.User.IsDeleted,
                RejectedOrderPrecentage = merchant.RejectedOrederPercentage
            }).ToList();


            return new PaginationResponse<MerchantDTO>(
                pageSize: param.PageSize,
                pageIndex: param.PageIndex,
                totalCount: totalCount,
                data: result.AsReadOnly()
            )
                ;
        }
        #endregion

        #region Get Merchant By Id
        public async Task<MerchantDTO?> GetMerchantByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            MerchantSpecification spec = new MerchantSpecification(id);
            var Merchant = await _unit.Repository<Merchant>().GetBySpecAsync(spec);
            if (Merchant != null)
            {
                var specialPrices = await _unit.Repository<MerchantSpecialPrice>().GetAll();
                
                var user = await this._userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return null;
                }

                var userBranchesids = await _unit.Repository<UserBranches>().GetAllBySpecAsync(new UserBranchSpeification(user.Id));

                var userbranches = userBranchesids.Select(b => b.Branch).ToList();
                if (userbranches == null)
                {
                    return null;
                }

                var MerchantDto = new MerchantDTO
                {
                    Id = user?.Id,
                    Address = user?.Address,
                    Email = user?.Email,
                    SpecialPickUp = Merchant.SpecialPickUp,
                    StartWorkDate = user?.HiringDate,
                    PhoneNumber = user?.PhoneNumber,
                    StoreName = Merchant.StoreName,
                    Branches = user?.UserBranches?
                    .Select(ub => new GetBranchDTO
                    {
                        Id = ub.BranchId,
                        Name = ub.Branch?.Name ?? string.Empty,
                        CreationDate = ub.Branch?.CreationDate.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                        IsDeleted = ub.Branch?.IsDeleted ?? false,
                        CityName = ub.Branch?.City?.Name,
                        Location = ub.Branch?.Location
                    })
                    .ToList() ?? new List<GetBranchDTO>(),
                    Name = user.Name,
                    SpecialDeliveryPrices = specialPrices?.Where(sp => sp.MerchantId == id).Select(msp => new SpecialDeliveryPriceDTO
                    {
                        cityId = msp.CityId,
                        CityName = msp?.City.Name,
                        SpecialPrice = msp.SpecialPrice,

                    }).ToList(),
                    IsDeleted = user.IsDeleted,
                    RejectedOrderPrecentage = Merchant.RejectedOrederPercentage,
                };

                return MerchantDto;
            }

            return null;
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

            using var transaction = await _unit.BeginTransactionAsync();

            try
            {
                var user = merchant.User;

                if (!string.IsNullOrEmpty(dto.Name))
                    user.Name = dto.Name;

                if (!string.IsNullOrEmpty(dto.Email))
                    user.Email = dto.Email;

                if (!string.IsNullOrEmpty(dto.PhoneNumber))
                    user.PhoneNumber = dto.PhoneNumber;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    return (false, $"Update failed: {errors}");
                }
                

                if (!string.IsNullOrEmpty(dto.StoreName))
                    merchant.StoreName = dto.StoreName;


                if (dto.RejectedOrderPrecentage != null)
                    merchant.RejectedOrederPercentage = dto.RejectedOrderPrecentage ?? 0;


                if (dto.SpecialPickUp.HasValue)
                    merchant.SpecialPickUp = dto.SpecialPickUp;

                 _unit.Repository<Merchant>().Update(merchant);


                if (dto.BranchesId != null && dto.BranchesId.Any())
                {
                    var existingBranches = _unit.Repository<UserBranches>()
                        .GetAll().Result
                        .Where(ub => ub.UserId == dto.Id)
                        .ToList();

                    await _unit.Repository<UserBranches>().DeleteRange(existingBranches);

                    foreach (var branchId in dto.BranchesId)
                    {
                        var userBranch = new UserBranches
                        {
                            BranchId = branchId,
                            UserId = dto.Id
                        };
                        await _unit.Repository<UserBranches>().Add(userBranch);
                    }
                }


                var existingMerchantSpecialPrices = await _unit.Repository<MerchantSpecialPrice>().GetAllBySpecAsync(new MerchantSpecialSpecifications(dto.Id));
                if (existingMerchantSpecialPrices.Count > 0)
                    await _unit.Repository<MerchantSpecialPrice>().DeleteRange(existingMerchantSpecialPrices);
                if (dto.SpecialDeliveryPrices != null && dto.SpecialDeliveryPrices.Any() )
                {
                    var newMerchantSpecialForCities = dto.SpecialDeliveryPrices
                        .Select(specialPrice => new MerchantSpecialPrice
                        {
                            MerchantId = dto.Id,
                            CityId = specialPrice.cityId,
                            SpecialPrice = specialPrice.SpecialPrice
                        })
                        .ToList();
                    await _unit.Repository<MerchantSpecialPrice>().AddRange(newMerchantSpecialForCities);
                }

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


            try
            {

                var user = merchant.User;
                user.IsDeleted = !user.IsDeleted;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    await _unit.RollbackAsync();
                    var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    return (false, $"Delete failed: {errors}");
                }

                await _unit.Save();

                return (true, "Merchant deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }






        #endregion
    }
}