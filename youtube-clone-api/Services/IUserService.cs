using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<User>?> GetUser(int id);
        Task<ServiceResponse<List<User>>> GetUsers();
        Task<ServiceResponse<User>> UpdateUser(int id, UpdateRequestDto updateRequest);
        Task<ServiceResponse<User>> DeleteUser(int id);
    }
}
