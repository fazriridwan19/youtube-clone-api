using youtube_clone_api.Models;

namespace youtube_clone_api.Services.LikeService
{
    public interface ILikeService
    {
        Task<ServiceResponse<Like>> LikeVideoById(int videoId);
        Task<ServiceResponse<Like>> CancelLikeVideoById(int videoId);
    }
}
