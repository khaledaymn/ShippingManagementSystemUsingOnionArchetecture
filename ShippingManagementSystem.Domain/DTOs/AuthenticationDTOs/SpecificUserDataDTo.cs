namespace ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs
{
    public class SpecificUserDataDTO
    {
        /// <summary>The unique identifier of the user.</summary>
        /// <example>usr_abcd1234</example>
        public string Id { get; set; }

        /// <summary>The full legal name of the user.</summary>
        /// <example>Khaled Ayman</example>
        public string? Name { get; set; }

        /// <summary>Unique login username.</summary>
        /// <example>khaled_aym</example>
        public string? UserName { get; set; }

        /// <summary>Residential or business address.</summary>
        /// <example>123 Nile St, Maadi, Cairo</example>
        public string? Address { get; set; }

        /// <summary>Official email address.</summary>
        /// <example>khaled@example.com</example>
        public string? Email { get; set; }

        /// <summary>The date the user joined the company (formatted).</summary>
        /// <example>2024-05-20</example>
        public string? HireDate { get; set; }

        /// <summary>Primary contact phone number.</summary>
        /// <example>01012345678</example>
        public string? PhoneNumber { get; set; }

        /// <summary>Assigned security role.</summary>
        /// <example>Employee</example>
        public string? Role { get; set; }
    }
}
