using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<User>> Register(RegisterRequestDto request);
        Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto request);
    }
}
