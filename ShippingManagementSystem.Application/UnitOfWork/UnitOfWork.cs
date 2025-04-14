using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShippingManagementSystem.Application.Services.IdentityServices;
using ShippingManagementSystem.Application.Settings;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Interfaces.IGenaricRepository;
using ShippingManagementSystem.Infrastructure.Data;
using ShippingManagementSystem.Infrastructure.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShippingManagementSystem.Application.Services.ExternalServices.EmailServices;
using ShippingManagementSystem.Domain.UserTypes;
using ShippingManagementSystem.Domain.Services;
using ShippingManagementSystem.Application.Services;

namespace ShippingManagementSystem.Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        #region Private Properties

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JWT> _jwt;
        private readonly IOptions<EmailConfiguration> _emailSettings;

        #region Services

        private readonly ConcurrentDictionary<string, object> _repositories;
        private IAuthenticationServices _authenticationService;
        private IEmailServices _emailService;
        private IEmployeeServices _employeeService;
        private IMerchantServices _merchantService;
        private IShippingRepresentativeServices _shippingRepresentativeService;
        private IBranchServices _branchService;
        private IChargeTypeServices _chargeTypeService;
        private ICityServices _cityService;
        private IGovernorateServices _governorateService;
        private IGroupServices _groupService;
        private IMeduleServices _meduleService;
        private IRejectedReasonServices _rejectedReasonService;
        private IOrderServices _orderService;
        private IStandardServices _standersServices;

        #endregion

        #endregion


        #region Public Properties

        #region Authentication Service
        public IAuthenticationServices AuthenticationService
        {
            get
            {
                if (_authenticationService != null)
                    return _authenticationService;
                return new AuthenticationServices(_userManager,_jwt,this);
            }
        }

        #endregion

        #region Email Service
        public IEmailServices EmailService
        {
            get
            {
                if (_emailService != null)
                    return _emailService;
                return new EmailService(_emailSettings);
            }
        }

        #endregion

        #region Employee Service

        public IEmployeeServices EmployeeService
        {
            get
            {
                if (_employeeService != null)
                    return _employeeService;
                return new EmployeeServices(this,_userManager,_context);
            }
        }

        #endregion

        #region Merchant Service

        public IMerchantServices MerchantService
        {
            get
            {
                if (_merchantService != null)
                    return _merchantService;
                return new MerchantServices(this, _userManager);
            }
        }

        #endregion

        #region Shipping Representative Service

        public IShippingRepresentativeServices ShippingRepresentativeServices
        {
            get
            {
                if (_shippingRepresentativeService != null)
                    return _shippingRepresentativeService;
                return new ShippingRepresentativeServices(this, _userManager);
            }
        }

        #endregion

        #region Branch Service
        public IBranchServices BranchService
        {
            get
            {
                if (_branchService != null)
                    return _branchService;
                return new BranchServices(this);
            }
        }

        #endregion

        #region Charge Type Service
        public IChargeTypeServices ChargeTypeService
        {
            get
            {
                if (_chargeTypeService != null)
                    return _chargeTypeService;
                return new ChargeTypeServices(this);
            }
        }

        #endregion

        #region City Service

        public ICityServices CityService
        {
            get
            {
                if (_cityService != null)
                    return _cityService;
                return new CityServices(this,_context);
            }
        }

        #endregion

        #region Governorate Service

        public IGovernorateServices GovernorateService
        {
            get
            {
                if (_governorateService != null)
                    return _governorateService;
                return new GovernorateServices(this);
            }
        }

        #region Group Service

        public IGroupServices GroupService
        {
            get
            {
                if (_groupService != null)
                    return _groupService;
                return new GroupServices(this, _userManager,_context);
            }
        }

        #endregion

        #region Medule Service

        public IMeduleServices MeduleService
        {
            get
            {
                if (_meduleService != null)
                    return _meduleService;
                return new MeduleServices(this, _context);
            }
        }

        #endregion

        #region Rejected Reason Service
        public IRejectedReasonServices RejectedReasonService
        {
            get
            {
                if (_rejectedReasonService != null)
                    return _rejectedReasonService;
                return new RejectedReasonServices(this);
            }
        }
        #endregion

        #endregion

        #region Order Service

        public IOrderServices OrderService
        {
            get
            {
                if (_orderService != null)
                    return _orderService;
                return new OrderServices(this);
            }
        }

        #endregion

        #region Standard Service

        public IStandardServices standardServices
        {
            get
            {
                if (_standersServices != null)
                    return _standersServices;
                return new StandardServices(this);
            }
        }

        #endregion

        #endregion


        #region Constructor
        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, IOptions<EmailConfiguration> emailSettings)
        {
            _context = context;
            _userManager = userManager;
            _jwt = jwt;
            _repositories = new();
            _emailSettings = emailSettings;
        }

        #endregion


        #region Repository

        public IGenaricRepo<T> Repository<T>() where T : class
        {
            try
            {
                var repository = (IGenaricRepo<T>)_repositories.GetOrAdd(typeof(T).Name, _ => new GenaricRepo<T>(_context));

                return repository;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


        #region Begin Transaction

        private IDbContextTransaction _transaction;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            try
            {
                if (_context == null)
                    throw new InvalidOperationException("Database context is not initialized.");

                if (_context.Database == null)
                    throw new InvalidOperationException("Database connection is not initialized.");
               
                if (_transaction != null)
                    return _transaction;

                _transaction = await _context.Database.BeginTransactionAsync();
                return _transaction;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while starting a transaction.", ex);
            }
        }

        #endregion


        #region Commit Transaction

        public async Task CommitAsync()
        {
            try
            {
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while committing the transaction.", ex);
            }
        }

        #endregion


        #region Rollback Transaction

        public async Task RollbackAsync()
        {
            try
            {
                await _context.Database.RollbackTransactionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while rolling back the transaction.", ex);
            }
        }

        #endregion


        #region Save Changes
        public async Task<int> Save()
        {
            try
            {
                int result = await _context.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Concurrency error occurred while saving changes.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while saving changes.", ex);
            }
        }


        #endregion


        #region Dispose
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        #endregion
    }
}
