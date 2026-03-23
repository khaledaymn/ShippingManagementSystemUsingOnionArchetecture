using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Application.DTOs.AuthenticationDTOs
{
    public class ChangePasswordDTO
    {
        /// <summary>
        /// The current password of the user to verify identity before change.
        /// </summary>
        /// <example>OldP@ssword123</example>
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }

        /// <summary>
        /// The new secure password to replace the current one.
        /// </summary>
        /// <example>NewP@ssw0rd2025!</example>
        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }

        /// <summary>
        /// The unique ID (GUID) of the user performing the password change.
        /// </summary>
        /// <example>usr_a1b2c3d4e5</example>
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }
    }
}
