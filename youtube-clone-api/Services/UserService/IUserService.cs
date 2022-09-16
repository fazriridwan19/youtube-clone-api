using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<UserResponseDto>?> GetUser(int id);
        Task<ServiceResponse<List<UserResponseDto>>> GetUsers();
        Task<ServiceResponse<User>> UpdateUser(int id, UpdateRequestDto updateRequest);
        Task<ServiceResponse<User>> DeleteUser(int id);
        Task<ServiceResponse<Subscriber>> SubscribeUserById(int userId);
        Task<ServiceResponse<Subscriber>> UnsubscribeUserById(int userId);
    }
}
