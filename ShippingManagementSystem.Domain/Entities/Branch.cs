using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public string Location { get; set; }
        public virtual List<UserBranches>? UserBranches { get; } = new List<UserBranches>();
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City? City { get; set; }
        //public virtual List<Order>? BranchOrder { get; } = new List<Order>();
    }
}
