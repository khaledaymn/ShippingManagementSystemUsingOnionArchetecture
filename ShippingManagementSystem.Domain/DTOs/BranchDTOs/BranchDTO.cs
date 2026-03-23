using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.BranchDTOs
{
    public class BranchDTO
    {
        /// <summary> Unique identifier for the branch. </summary>
        /// <example>5</example>
        public int Id { get; set; }

        /// <summary> The official name of the branch. </summary>
        /// <example>Main Cairo Hub</example>
        public string? Name { get; set; } = string.Empty;

        /// <summary> Detailed physical address or GPS coordinates of the branch. </summary>
        /// <example>123 Logistics St, Industrial Zone</example>
        public string? Location { get; set; } = string.Empty;

        /// <summary> The ID of the city where this branch is located. </summary>
        /// <example>10</example>
        public int? CityId { get; set; }
    }
}
