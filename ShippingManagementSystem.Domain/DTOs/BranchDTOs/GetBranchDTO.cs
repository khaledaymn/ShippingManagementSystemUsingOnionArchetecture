namespace ShippingManagementSystem.Domain.DTOs.BranchDTOs
{
    public class GetBranchDTO
    {
        /// <summary> Unique branch identifier. </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary> Name of the branch. </summary>
        /// <example>Main Branch - Cairo</example>
        public string Name { get; set; } = string.Empty;

        /// <summary> Formatted branch creation date. </summary>
        /// <example>2023-01-15</example>
        public string CreationDate { get; set; }

        /// <summary> Soft-delete status. </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }

        /// <summary> Detailed physical location description. </summary>
        /// <example>Downtown Square, Building 5</example>
        public string Location { get; set; } = string.Empty;

        /// <summary> Name of the city where the branch is located. </summary>
        /// <example>Cairo</example>
        public string CityName { get; set; }
    }
}
