using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class EditCityDTO
    {
        public int Id { get; set; } 
        public string ? Name { get; set; } = string.Empty;
        public double? ChargePrice { get; set; }
        public double? PickUpPrice { get; set; }
        public int ? GovernorateId { get; set; }
    }
}
