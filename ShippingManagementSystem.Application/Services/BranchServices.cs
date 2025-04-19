using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class BranchServices : IBranchServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get All Branches
        public async Task<PaginationResponse<GetBranchDTO>> GetAllBranchesAsync(BranchParams param)
        {
            try
            {
                var spec = new BranchSpecification(param);
                var branches = await _unitOfWork.Repository<Branch>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<Branch>().CountAsync(spec);
                
                var branchDTOs = branches.Select(b => new GetBranchDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    CreationDate = b.CreationDate.ToString("yyyy-MM-dd"),
                    IsDeleted = b.IsDeleted,
                    Location = b.Location,
                    CityName = b.City?.Name
                }).ToList();
                
                return new PaginationResponse<GetBranchDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    branchDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving branches", ex);
            }
        }

        #endregion


        #region Get Branch By Id
        public async Task<GetBranchDTO?> GetBranchByIdAsync(int id)
        {
            try
            {
                var spec = new BranchSpecification(id);
                var branch = await _unitOfWork.Repository<Branch>().GetBySpecAsync(spec);
                
                if (branch == null)
                    return null;
                
                return new GetBranchDTO
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    CreationDate = branch.CreationDate.ToString("yyyy-MM-dd"),
                    IsDeleted = branch.IsDeleted,
                    Location = branch.Location,
                    CityName = branch.City?.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving branch with id {id}", ex);
            }
        }

        #endregion


        #region Create Branch
        public async Task<(bool IsSuccess, string Message)> CreateBranchAsync(CreateBranchDTO branchDTO)
        {
            try
            {
                var branch = new Branch
                {
                    Name = branchDTO.Name,
                    Location = branchDTO.Location,
                    CityId = branchDTO.CityId,
                    CreationDate = DateTime.Now,
                    IsDeleted = false
                };
                
                _unitOfWork.Repository<Branch>().Add(branch);
                await _unitOfWork.Save();
                
                return (true, $"Branch '{branch.Name}' created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating branch: {ex.Message}");
            }
        }

        #endregion


        #region Update Branch
        public async Task<(bool IsSuccess, string Message)> UpdateBranchAsync(int id, BranchDTO branchDTO)
        {
            try
            {
                var branch = await _unitOfWork.Repository<Branch>().GetById(id);
                
                if (branch == null)
                    return (false, $"Branch with id {id} not found");
                
                // Only update properties that are provided (not null/empty)
                if (!string.IsNullOrEmpty(branchDTO.Name))
                    branch.Name = branchDTO.Name;
                
                if (!string.IsNullOrEmpty(branchDTO.Location))
                    branch.Location = branchDTO.Location;
                
                if (branchDTO.CityId is not null && branchDTO.CityId > 0)
                    branch.CityId = branchDTO.CityId ?? 0;
                
                _unitOfWork.Repository<Branch>().Update(branch);
                await _unitOfWork.Save();
                
                return (true, $"Branch '{branch.Name}' updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating branch: {ex.Message}");
            }
        }

        #endregion


        #region Change Status of Branch
        public async Task<(bool IsSuccess, string Message)> DeleteBranchAsync(int id)
        {
            try
            {
                var branch = await _unitOfWork.Repository<Branch>().GetById(id);
                
                if (branch == null)
                    return (false, $"Branch with id {id} not found");
                
                // Soft delete
                branch.IsDeleted = !branch.IsDeleted;
                _unitOfWork.Repository<Branch>().Update(branch);
                
                // Hard delete if needed
                // _unitOfWork.Repository<Branch>().Delete(branch);
                
                await _unitOfWork.Save();
                
                return (true, $"Branch '{branch.Name}' deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting branch: {ex.Message}");
            }
        }

        #endregion
    }
} 