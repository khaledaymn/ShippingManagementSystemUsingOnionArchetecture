using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.BranchDTOs
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty; 
        public int? CityId { get; set; }
    }
}
