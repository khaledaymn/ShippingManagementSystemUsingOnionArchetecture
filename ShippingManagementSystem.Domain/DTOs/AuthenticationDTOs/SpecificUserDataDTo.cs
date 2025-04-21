using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs
{
    public class SpecificUserDataDTo
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string UserName { get; set; }

        public string? Address { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Enter a valid international phone number.")]

        public string? PhoneNumber { get; set; }
    }

}
