using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.DTOs.AuthenticationDTOs
{
    public class AuthenticationResponseDTO
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public Dictionary<string, List<string>> Permissions { get; set; }
        [JsonIgnore]
        public bool IsAuthenticated { get; set; }
    }
}
