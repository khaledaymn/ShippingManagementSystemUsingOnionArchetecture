using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.Entities
{
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

    }
}
