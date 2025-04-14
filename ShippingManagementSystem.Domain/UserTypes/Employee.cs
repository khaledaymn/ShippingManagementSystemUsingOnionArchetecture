using ShippingManagementSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.UserTypes
{
    public class Employee
    {
        [ForeignKey("User"),Key]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("GroupId")]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
