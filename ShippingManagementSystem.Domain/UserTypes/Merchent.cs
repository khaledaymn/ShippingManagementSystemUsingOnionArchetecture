using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingManagementSystem.Domain.UserTypes
{
   
    public class Merchant
    {
        public Merchant()
        {

        }

        //public Merchant( string id,MerchantDtoForAdding Merchanttoadd)
        //{
        //    this.UserID = id;
        //    this.StoreName = Merchanttoadd.StoreName;
        //    this.RejectedOrederPercentage = Merchanttoadd.RejectedOrderPrecentage;
        //    this.SpecialPickUp = Merchanttoadd.SpecialPickUp;
        //}

        [Key]
        public string UserID { get; set; }
        public string StoreName { get; set; }
        public double RejectedOrederPercentage { get; set; }
        public int? SpecialPickUp { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        public virtual List<MerchantCity>? MerchantCities { get; set; } = new List<MerchantCity>();
        public virtual List<MerchantSpecialPrice>? MerchantSpecialPrices { get; set; } = new List<MerchantSpecialPrice>();
        
        //public virtual List<Order> MerchantOrders { get; } = new List<Order>();
    }
}
