using ShippingManagementSystem.Application.UserTypes.Enums;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.UserTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.UserTypes
{
    public class ShippigRepresentative
    {
        public DiscountType DiscountType { get; set; }
        public double CompanyPersentage { get; set; }
        [ForeignKey("User"),Key]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual List<ShippingRepGovernorate>? ShippingRepGovernorates { get; set; }= new List<ShippingRepGovernorate>();
        public virtual List<Order>? ShippigRepresentativeOrders { get; } = new List<Order>();
    }

}
