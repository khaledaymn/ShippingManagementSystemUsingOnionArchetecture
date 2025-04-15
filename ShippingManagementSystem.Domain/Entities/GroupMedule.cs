using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.Entities
{
    [PrimaryKey(nameof(MeduleId),nameof(GroupId),nameof(Permission))]
    public class GroupMedule
    {
        public Permission Permission { get; set; }
        [ForeignKey("Medule")]
        public int MeduleId { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public virtual Medule? Medule { get; set; }
        public virtual Group? Group { get; set; }
    }
}
