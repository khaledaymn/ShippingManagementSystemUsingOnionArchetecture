using ShippingManagementSystem.Domain.UserTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public virtual List<Employee>? User { get; set; }
        public virtual List<GroupMedule>? GroupMedules { get; set; }
    }
}
