using Microsoft.EntityFrameworkCore;
using Shipping_Project.DTOs;
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
        public MerchantSpecialPrice()
        {

        }
        public MerchantSpecialPrice(string Merchantid, SpecialDeliveryPriceForMerchant sp)
        {
            this.MerchantId = Merchantid;
            this.CityId = sp.cityId;
            this.SpecialPrice = sp.SpecialPreice;
        }
        public double SpecialPrice { get; set; }
        [ForeignKey("Merchant")]
        public string MerchantId { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual Merchant? Merchant { get; set; }
        public virtual City? City { get; set; }
    }
}
