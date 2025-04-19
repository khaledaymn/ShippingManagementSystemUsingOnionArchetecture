using Microsoft.AspNetCore.Identity;
using Shipping_Project.DTOs.BranchesDtos;
using Shipping_Project.DTOs;
using Shipping_Project.DTOs.MerchantDtos;
using ShippingManagementSystem.Application.Exptions;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.UserBranches;
using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerhantSpeialPricesSpeifications;

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
        public async Task<(bool IsSuccess, string Message)> AddMerchantAsync(MerchantDtoForAdding dto)
        {
                if (dto == null || string.IsNullOrEmpty(dto.Email))
                    return (false, "Invalid merchant data.");
            if (dto.Email.CheckEmailExists(_userManager).Result)
                return (false, "Email already Exists");


        var newUser = new ApplicationUser
                {
                    Name = dto.Name,
                    UserName = dto.Name.Replace(" ", ""),
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    IsDeleted = false,
                    HiringDate = DateTime.Now,
                    Address = dto.Address,

             
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
                                    SpecialPrice = specialPrice.SpecialPreice
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

        public async Task<PaginationResponse<MerchantDTO>> GetAllMerchantsAsync(MerchantParams param)
        {



            var spec = new MerchantSpecification(param);

            var merchants = await _unit.Repository<Merchant>().GetAllBySpecAsync(spec);

            var totalCount = await _unit.Repository<Merchant>().CountAsync(spec);
            var result = merchants.Select(m => new MerchantDTO(m)).ToList();

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
        public async Task< MerchecntForEditingAndGetting?> GetMerchantByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            MerchantSpecification spec = new MerchantSpecification(id);
            var Merchant = await _unit.Repository<Merchant>().GetBySpecAsync(spec);
            if (Merchant != null)
            {
                var user = await this._userManager.FindByIdAsync(id);
                if (user == null)
                {
                return null;

                }
                
                var userBranchesids = await _unit.Repository<UserBranches>().GetAllBySpecAsync(new UserBranchSpeification( user.Id));

                var userbranches = userBranchesids.Select(b => b.Branch).ToList();
                if (userbranches == null)
                {
                    return null;
                }
                List<branchDtoForMerchant> BranchForResp = userbranches.Select(b => new branchDtoForMerchant(b)).ToList();



             

                var MerchantDto = new MerchecntForEditingAndGetting
                {
                    Id = user.Id,
                    Address = user.Address,
                    UserName = user.UserName,
                    Email = user.Email,
                    SpecialPickUp = Merchant.SpecialPickUp,
                    StartWorkDDate = user.HiringDate,
                    PhoneNumber = user.PhoneNumber,
                    StoreName = Merchant.StoreName,
                    Branches = BranchForResp,
                    Name = user.Name,
                    SpecialDeliveryPrices = Merchant.MerchantSpecialPrices?.Select(msp => new SpecialDeliveryPriceForMerchant
                    {
                        cityId = msp.CityId,
                        SpecialPreice = msp.SpecialPrice,
                        CityName=msp.City.Name

                    }).ToList(),
                    IsDeleted = user.IsDeleted,
                };

                return MerchantDto;
            }

            return null;
            

           
        }
        #endregion


        #region Update Merchant
        public async Task<(bool IsSuccess, string Message)> UpdateMerchantAsync(MerchecntForEditingAndGetting dto)
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

                 
                    if (userUpdated)
                    {
                        user.IsDeleted = dto.IsDeleted;
                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                            return (false, $"Update failed: {errors}");
                        }
                    }

                   
                    bool merchantUpdated = false;

                    if (!string.IsNullOrEmpty(dto.StoreName))
                    {
                        merchant.StoreName = dto.StoreName;
                        merchantUpdated = true;
                    }

                   
                    if (dto.RejectedOrderPrecentage != null)
                    {
                        merchant.RejectedOrederPercentage = dto.RejectedOrderPrecentage;
                        merchantUpdated = true;
                    }

                   
                    if (dto.SpecialPickUp.HasValue)
                    {
                        merchant.SpecialPickUp = dto.SpecialPickUp;
                        merchantUpdated = true;
                    }

                    
                    if (merchantUpdated)
                    {
                        _unit.Repository<Merchant>().Update(merchant);
                    }

                   
                    var existingUserBranches = await _unit.Repository<UserBranches>().GetAllBySpecAsync(new UserBranchSpeification(dto.Id));
                    _unit.Repository<UserBranches>().DeleteRange(existingUserBranches);
                    var newUserBranches = dto.Branches.Select(b => new UserBranches(dto.Id, b.BrancheID)).ToList();
                    await _unit.Repository<UserBranches>().AddRange(newUserBranches);


                    var existingMerchantSpecialPrices = await _unit.Repository<MerchantSpecialPrice>().GetAllBySpecAsync(new MerchantSpecialSpecifications(dto.Id));
                    if (existingMerchantSpecialPrices.Count > 0)
                        _unit.Repository<MerchantSpecialPrice>().DeleteRange(existingMerchantSpecialPrices);
                    if (dto.SpecialDeliveryPrices.Count > 0)
                    {
                        var newMerchantSpecialForCities = dto.SpecialDeliveryPrices.Select(s => new MerchantSpecialPrice(dto.Id, s)).ToList();
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