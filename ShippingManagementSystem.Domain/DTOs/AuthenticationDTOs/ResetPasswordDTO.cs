using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Application.DTOs.AuthenticationDTOs
{
    public class ResetPasswordDTO
    {
        /// <summary>
        /// The unique security token received via email for password recovery.
        /// </summary>
        /// <example>CfDJ8M+s/R5...</example>
        [Required(ErrorMessage = "Token is Required")]
        public string Token { get; set; }

        /// <summary>
        /// The email address associated with the account being recovered.
        /// </summary>
        /// <example>khaled.ayman@example.com</example>
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email not in correct formate")]
        public string Email { get; set; }

        /// <summary>
        /// The new secure password to be set for the account.
        /// </summary>
        /// <example>NewP@ssw0rd2025</example>
        [Required(ErrorMessage = "Passwird is Required")]
        public string Password { get; set; }
    }
}
