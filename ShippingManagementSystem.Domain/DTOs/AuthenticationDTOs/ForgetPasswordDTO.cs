using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Application.DTOs.AuthenticationDTOs
{
    public class ForgetPasswordDTO
    {
        /// <summary>
        /// The registered email address where the password reset link will be sent.
        /// </summary>
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "This Field Required")]
        [EmailAddress(ErrorMessage = "Invalid Mail")]
        public string Email { get; set; }
    }
}
