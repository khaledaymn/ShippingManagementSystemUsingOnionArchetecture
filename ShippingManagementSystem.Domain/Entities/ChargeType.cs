namespace ShippingManagementSystem.Domain.Entities
{
    public class ChargeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ExtraPrice { get; set; }
        public int NumOfDay { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
