using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.DTOs.AuthenticationDTOs
{
    public class LoginDTO
    {
        /// <summary>
        /// The registered email address of the user.
        /// </summary>
        /// <example>admin@shipping.com</example>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        /// <summary>
        /// User's secure password.
        /// </summary>
        /// <example>P@ssw0rd123!</example>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
