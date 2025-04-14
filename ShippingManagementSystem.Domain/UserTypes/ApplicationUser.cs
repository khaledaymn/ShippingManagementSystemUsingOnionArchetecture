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
        public virtual Merchant? Merchant { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual ShippigRepresentative? Representive { get; set; }
        public virtual List<UserBranches>? UserBranches { get; } = new List<UserBranches>();
    }
}
