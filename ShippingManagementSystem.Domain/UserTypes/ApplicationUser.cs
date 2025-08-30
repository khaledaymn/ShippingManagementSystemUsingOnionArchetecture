using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Domain.Entities;


namespace ShippingManagementSystem.Domain.UserTypes
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime HiringDate { get; set; }
        public virtual List<Merchant>? Merchant { get; } = new List<Merchant>();
        public virtual List<Employee>? Employee { get; } = new List<Employee>();
        public virtual ShippigRepresentative? Representive { get; set; }
        public virtual List<UserBranches>? UserBranches { get; } = new List<UserBranches>();
    }
}
