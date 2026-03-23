using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class AssignOrderToDelivaryDTO
    {
        /// <summary> Target state (Usually 'DeliveredToTheRepresentative'). </summary>
        /// <example>DeliveredToTheRepresentative</example>
        [Required]
        public string OrderState { get; set; } = string.Empty;

        /// <summary> User ID of the Shipping Representative. </summary>
        /// <example>usr_rep_55</example>
        [Required]
        public string ShippingRepresentativeId { get; set; }
    }
}
