using System.Security.Claims;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services
{
    public class ChannelService : IChannelService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ChannelService(DataContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public async Task<ServiceResponse<Channel>> CreateChannel(CreateChannelRequestDto request)
        {
            var serviceResponse = new ServiceResponse<Channel>();
            string userEmail = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == userEmail);
            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "User not Found";
                return serviceResponse;
            }
            if (await _context.Channels.FindAsync(user.Id) != null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Already have Channel";
                return serviceResponse;
            }
            var channel = new Channel
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                User = user,
                UserId = user.Id
            };
            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            serviceResponse.Data = channel;
            serviceResponse.Message = "Channel Successfully Added";
            return serviceResponse;
        }
    }
}
