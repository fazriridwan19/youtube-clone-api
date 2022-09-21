using youtube_clone_api.Models;

namespace youtube_clone_api.Services.DislikeService
{
    public interface IDislikeService
    {
        Task<ServiceResponse<Dislike>> DislikeVideoById(int videoId);
        Task<ServiceResponse<Dislike>> CancelDislikeVideoById(int videoId);
    }
}
