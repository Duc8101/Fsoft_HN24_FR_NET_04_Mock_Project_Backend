using Common.DTOs.UserDTO;
using Phone_Shop.Common.DTOs.UserDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Users
{
    public interface IUserService
    {
        ResponseBase Login(LoginDTO DTO);
        ResponseBase Register(RegisterDTO DTO);
        Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO);
        ResponseBase Detail(int userId);
        ResponseBase UpdateProfile(ProfileDTO DTO, int userId);
        ResponseBase ChangePassword(ChangePasswordDTO DTO, int userId);
        ResponseBase GetUserByToken(string token);
    }
}
