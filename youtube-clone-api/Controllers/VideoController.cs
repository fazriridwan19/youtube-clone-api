using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Services.VideoService;

namespace youtube_clone_api.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Video>>> CreateVideo([FromBody] VideoRequestDto request)
        {
            var response = await _videoService.CreateVideo(request);
            if (response.Status ==  404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Video>>> GetVideo(int id)
        {
            var response = await _videoService.GetVideoById(id);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Video>>> GetVideos()
        {
            return Ok(await _videoService.GetVideos());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Video>>> UpdateVideo(int id, [FromBody] UpdateVideoRequestDto request)
        {
            var response = await _videoService.UpdateVideoById(id, request);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            if (response.Status == 401)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Video>>> DeleteVideo(int id)
        {
            var response = await _videoService.DeleteVideo(id);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            if (response.Status == 401)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
    }
}
