using System.Text.Json.Serialization;

namespace ShippingManagementSystem.Application.DTOs.AuthenticationDTOs
{
    public class AuthenticationResponseDTO
    {
        /// <summary>
        /// Unique identifier (GUID) of the authenticated user.
        /// </summary>
        /// <example>u9a1-b2c3-d4e5-f6g7</example>
        public string Id { get; set; }

        /// <summary>
        /// Status message regarding the authentication result.
        /// </summary>
        /// <example>Login successfully</example>
        public string Message { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        /// <example>Khaled Ayman</example>
        public string Name { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        /// <example>khaled@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// JWT Bearer Token for authorizing subsequent requests.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; }

        /// <summary>
        /// Assigned primary role in the system.
        /// </summary>
        /// <example>Admin</example>
        public string Role { get; set; }

        /// <summary>
        /// Dictionary of specific permissions mapped by module for Employee roles.
        /// </summary>
        public Dictionary<string, List<string>> Permissions { get; set; }

        [JsonIgnore]
        public bool IsAuthenticated { get; set; }
    }
}
