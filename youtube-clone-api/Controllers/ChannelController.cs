using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Services;

namespace youtube_clone_api.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _channelService;

        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Channel>>> CreateChannel(CreateChannelRequestDto request)
        {
            var response = await _channelService.CreateChannel(request);
            if (response.Message == "User not Found")
            {
                return NotFound(response);
            }
            if (response.Message == "Already have Channel")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
