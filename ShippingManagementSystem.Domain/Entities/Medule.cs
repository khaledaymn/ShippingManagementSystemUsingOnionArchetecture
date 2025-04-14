namespace ShippingManagementSystem.Domain.Entities
{
    public class Medule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<GroupMedule>? GroupMedules { get; set; }
    }
}
