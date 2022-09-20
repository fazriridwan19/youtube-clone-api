using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Services.LikeService;
using youtube_clone_api.Services.VideoService;

namespace youtube_clone_api.Controllers
{
    [EnableCors()]
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly ILikeService _likeService;

        public VideoController(IVideoService videoService, ILikeService likeService)
        {
            _videoService = videoService;
            _likeService = likeService;
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

        [HttpPost("/like/{videoId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Like>>> LikeVideo(int videoId)
        {
            var response = await _likeService.LikeVideoById(videoId);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            if (response.Status == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("/unlike/{videoId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Like>>> CancelLikeVideo(int videoId)
        {
            var response = await _likeService.CancelLikeVideoById(videoId);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
