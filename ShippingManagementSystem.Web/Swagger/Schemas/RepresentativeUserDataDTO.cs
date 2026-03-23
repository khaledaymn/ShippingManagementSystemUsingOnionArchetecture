using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;

namespace ShippingManagementSystem.Web.Swagger.Schemas
{
    public class RepresentativeUserDataDTO : SpecificUserDataDTO
    {
        /// <summary>
        /// The representative's commission format (e.g., "$50" for fixed or "10%" for percentage).
        /// </summary>
        /// <example>$50</example>
        public string CompanyPercentage { get; set; }

        /// <summary>
        /// List of governorate names assigned to this representative for delivery.
        /// </summary>
        /// <example>["Cairo", "Alexandria"]</example>
        public IEnumerable<string> Governorates { get; set; } = new List<string>();
    }
}
