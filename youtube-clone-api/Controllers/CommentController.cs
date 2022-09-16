using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using youtube_clone_api.Models;
using youtube_clone_api.Services.CommentService;

namespace youtube_clone_api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("{videoId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Comment>>> CreateComment(int videoId, [FromBody] string commentText)
        {
            var response = await _commentService.CreateComment(videoId, commentText);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Comment>>>> GetComments()
        {
            var response = await _commentService.GetComments();
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Comment>>> GetComment(int id)
        {
            var response = await _commentService.GetCommentById(id);
            if (response.Status == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Comment>>> UpdateComment(int id, [FromBody] string commentText)
        {
            var response = await _commentService.UpdateComment(id, commentText);
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
        public async Task<ActionResult<ServiceResponse<Comment>>> DeleteComment(int id)
        {
            var response = await _commentService.DeleteComment(id);
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
