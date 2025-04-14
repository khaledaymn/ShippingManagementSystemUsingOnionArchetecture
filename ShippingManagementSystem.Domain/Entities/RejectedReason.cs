namespace ShippingManagementSystem.Domain.Entities
{
    public class RejectedReason
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<RejectedReasonOrder>? Orders { get; }=new List<RejectedReasonOrder>();
    }
}
