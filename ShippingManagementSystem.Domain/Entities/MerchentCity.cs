using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.UserTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.Entities
{
    [PrimaryKey(nameof(MerchantId),nameof(CityId))]
    public class MerchantCity
    {
        [ForeignKey("Merchant")]
        public string MerchantId { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual Merchant? Merchant { get; set; }
        public virtual City? City { get; set; }
    }
}
