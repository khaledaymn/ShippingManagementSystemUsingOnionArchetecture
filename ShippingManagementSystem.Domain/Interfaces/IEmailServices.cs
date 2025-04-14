
namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IEmailServices
    {
        Task<string> SendEmailAsync(string Name, string Email,string Token);
    }
}
