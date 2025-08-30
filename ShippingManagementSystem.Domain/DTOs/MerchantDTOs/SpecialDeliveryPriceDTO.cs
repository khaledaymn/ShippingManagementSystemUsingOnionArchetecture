namespace ShippingManagementSystem.Domain.DTOs
{
    public class SpecialDeliveryPriceDTO
    {
        public int cityId { get; set; }
        public string? CityName { get; set; }
        public double SpecialPrice { get; set; }
    }
}
