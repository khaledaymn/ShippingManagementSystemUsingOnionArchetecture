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
        [Required(ErrorMessage = "Order status is required")]
        public string OrderState { get; set; } = string.Empty;
        [Required(ErrorMessage = "ShippigRepresentativeId is required")]
        public string ShippigRepresentativeId { get; set; }
    }
}
