using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.Entities
{
    public class City 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ChargePrice { get; set; }
        public double PickUpPrice { get; set; }
        public bool IsDeleted { get; set; }=false;
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }
        public virtual List<MerchantCity>? MerchantCities { get; } = new List<MerchantCity>();
        public virtual List<Branch>? Branches { get; }=new List<Branch>();
        //public virtual List<Order>? Orders { get; }=new List<Order>();
    }
}
