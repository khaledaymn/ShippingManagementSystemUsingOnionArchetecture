using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;

namespace ShippingManagementSystem.Web.Swagger.Schemas
{
    public class MerchantProfileDTO : SpecificUserDataDTO
    {
        /// <summary> Registered trade name of the store. </summary>
        /// <example>Ayman Logistics Store</example>
        public string StoreName { get; set; }

        /// <summary> Historical percentage of rejected shipments. </summary>
        /// <example>1.5</example>
        public double RejectedOrederPercentage { get; set; }

        /// <summary> Custom pickup fee assigned to this merchant. </summary>
        /// <example>25</example>
        public int? SpecialPickUp { get; set; }
    }
}
