namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification
{
    public class MerchantParams
    {
        /// <summary> Search term (matches against Name, StoreName, or Email). </summary>
        /// <example>Ayman</example>
        public string? Search { get; set; }

        /// <summary> Filter by activation status (IsActive/IsDeleted). </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }

        /// <summary> Sorting criteria (e.g., 'name_asc', 'date_desc'). </summary>
        /// <example>name_asc</example>
        public string? Sort { get; set; }

        /// <summary> Current page number. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of results per page (Max 10). </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 10;
    }
}