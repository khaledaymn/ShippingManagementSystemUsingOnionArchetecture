using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Success
{
    public class GetEmployeeProfileSuccessExample : IExamplesProvider<EmployeeProfileDTO>
    {
        public EmployeeProfileDTO GetExamples() => new EmployeeProfileDTO
        {
            Id = "usr_emp_01",
            Name = "Ahmed Employee",
            UserName = "ahmed_staff",
            Email = "ahmed@company.com",
            Role = "Internal Staff",
            HireDate = "2023-10-01",
            PhoneNumber = "01011122233",
            Address = "Company Headquarters, Cairo",
            Permissions = new Dictionary<string, List<string>>
            {
                { "Orders", new List<string> { "View", "Edit", "Delete" } },
                { "Branches", new List<string> { "View" } }
            }
        };
    }
    public class GetMerchantProfileExample : IExamplesProvider<MerchantProfileDTO>
    {
        public MerchantProfileDTO GetExamples() => new MerchantProfileDTO
        {
            Id = "usr_mer_05",
            Name = "Khaled Merchant",
            StoreName = "Ayman Electronics",
            Email = "khaled@store.com",
            Role = "Merchant",
            RejectedOrederPercentage = 2.5,
            SpecialPickUp = 15,
            HireDate = "2024-01-20",
            PhoneNumber = "01144556677",
            Address = "Nasr City, Cairo"
        };
    }
    public class GetRepresentativeProfileExample : IExamplesProvider<RepresentativeUserDataDTO>
    {
        public RepresentativeUserDataDTO GetExamples() => new RepresentativeUserDataDTO
        {
            Id = "usr_rep_10",
            Name = "Maged Delivery",
            Role = "Shipping Representative",
            CompanyPercentage = "10%",
            Governorates = new List<string> { "Cairo", "Giza", "Alexandria" },
            Email = "maged@logistics.com",
            HireDate = "2024-03-15",
            PhoneNumber = "01288990011",
            Address = "Representative Hub, Giza"
        };
    }
}
