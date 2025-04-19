
using ShippingManagementSystem.Domain.Entities;

namespace Shipping_Project.DTOs.BranchesDtos
{
    public class branchDtoForMerchant
    {
        public branchDtoForMerchant()
        {
            
        }
        public branchDtoForMerchant(Branch bran)
        {
            this.BrancheID = bran.Id;
            this.Name = bran.Name;
        }
        public int BrancheID { get; set; }
        public string? Name { get; set; }
    }
}
