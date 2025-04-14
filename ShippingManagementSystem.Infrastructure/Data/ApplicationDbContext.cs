using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<UserBranches> UserBranches { get; set; }
        public DbSet<ChargeType> ChargeTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Governorate> governorates { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<MerchantCity> MerchantCities { get; set; }
        public DbSet<MerchantSpecialPrice> MerchantSpecialPrices { get; set; }
        public DbSet<RejectedReason> RejectedReasons { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<ShippigRepresentative> ShippigRepresentatives { get; set; }
        public DbSet<ShippingRepGovernorate> ShippingRepGovernorates { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Medule> Medules { get; set; }
        public DbSet<GroupMedule> GroupMedules { get; set; }
        public DbSet<Product> Products { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
