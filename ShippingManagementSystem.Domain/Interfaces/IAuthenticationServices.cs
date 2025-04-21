using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<AuthenticationResponseDTO> Login(LoginDTO data);
        Task<string> ForgetPassword(ForgetPasswordDTO dto);
        Task<string> ResetPassword(ResetPasswordDTO dto);
        Task<string> ChangePassword(ChangePasswordDTO dto);
        Task<SpecificUserDataDTo> GetSpecificUser(string id);
        Task<(bool IsSuccess, string Message)> UpdateUserData(SpecificUserDataDTo dto);

    }
}
