using Microsoft.EntityFrameworkCore.Storage;
using ShippingManagementSystem.Application.Services;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Interfaces.IGenaricRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBranchServices BranchService { get; }
        IChargeTypeServices ChargeTypeService { get; }
        ICityServices CityService { get; }
        IGovernorateServices GovernorateService { get; }
        IGroupServices GroupService { get; }
        IMeduleServices MeduleService { get; }
        IRejectedReasonServices RejectedReasonService { get; }
        IShippingRepresentativeServices ShippingRepresentativeServices { get; }
        IEmployeeServices EmployeeService { get; }
        IMerchantServices MerchantService { get; }
        IAuthenticationServices AuthenticationService { get; }
        IOrderServices OrderService { get; }
        IEmailServices EmailService { get; }
        IStandardServices standardServices { get; }
        IGenaricRepo<T> Repository<T>() where T : class;
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> Save();
    }
}
