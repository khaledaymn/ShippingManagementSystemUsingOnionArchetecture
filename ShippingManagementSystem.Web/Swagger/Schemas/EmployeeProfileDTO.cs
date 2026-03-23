using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;

namespace ShippingManagementSystem.Web.Swagger.Schemas
{
    public class EmployeeProfileDTO : SpecificUserDataDTO
    {
        /// <summary>Group-based permissions categorized by module.</summary>
        public Dictionary<string, List<string>> Permissions { get; set; } = new();
    }
}
