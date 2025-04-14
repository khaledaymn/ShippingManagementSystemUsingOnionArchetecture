using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.MeduleSpecification;
using ShippingManagementSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class MeduleServices : IMeduleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public MeduleServices(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<PaginationResponse<MeduleDTO>> GetAllMedulesAsync(MeduleParams param)
        {
            try
            {
                var spec = new MeduleSpecification(param);
                var medules = await _unitOfWork.Repository<Medule>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<Medule>().CountAsync(spec);
                
                var meduleDTOs = medules.Select(m => new MeduleDTO
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList();
                
                return new PaginationResponse<MeduleDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    meduleDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving modules", ex);
            }
        }

        public async Task<MeduleDTO?> GetMeduleByIdAsync(int id)
        {
            try
            {
                var spec = new MeduleSpecification(id);
                var medule = await _unitOfWork.Repository<Medule>().GetBySpecAsync(spec);
                
                if (medule == null)
                    return null;
                
                return new MeduleDTO
                {
                    Id = medule.Id,
                    Name = medule.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving module with id {id}", ex);
            }
        }

    }
} 