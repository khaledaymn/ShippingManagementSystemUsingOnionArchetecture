using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.MeduleDTOs
{
    public class Permission
    {
        public int Id { get; set; }
        public List<int> Values { get; set; }
    }
}
