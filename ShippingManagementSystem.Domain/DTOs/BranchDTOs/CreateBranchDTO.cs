using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.BranchDTOs
{
    public class CreateBranchDTO
    {
        /// <summary> The descriptive name of the branch. </summary>
        /// <example>October City Branch</example>
        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary> The physical location or address of the new branch. </summary>
        /// <example>Plot 45, Beside the Central Bank</example>
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        /// <summary> The City ID that this branch will serve. </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "City ID is required")]
        public int CityId { get; set; }
    }
} 