using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Entities
{
    [PrimaryKey(nameof(MerchantId), nameof(CityId))]
    public class MerchantSpecialPrice
    {
        public double SpecialPrice { get; set; }
        [ForeignKey("Merchant")]
        public string MerchantId { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual Merchant? Merchant { get; set; }
        public virtual City? City { get; set; }
    }
}
