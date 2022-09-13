using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services
{
    public interface IChannelService
    {
        Task<ServiceResponse<Channel>> CreateChannel(CreateChannelRequestDto request);
    }
}
