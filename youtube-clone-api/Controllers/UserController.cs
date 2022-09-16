using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Services.UserService;

namespace youtube_clone_api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserResponseDto>>>> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<UserResponseDto>>> GetUser(int id)
        {
            var response = await _userService.GetUser(id);
            if (response?.Status == 404)
            {
                return NotFound(response);
            }
            if (response?.Status == 401)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<User>>> UpdateUser(int id, [FromBody] UpdateRequestDto updateRequest)
        {
            var response = await _userService.UpdateUser(id, updateRequest);
            if (response?.Message == "User not Found")
            {
                return NotFound(response);
            }
            if (response?.Message == "Not Authorize")
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ServiceResponse<User>>> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response?.Message == "User not Found")
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("subscribe/{userId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Subscriber>>> SubscribeUser(int userId)
        {
            var response = await _userService.SubscribeUserById(userId);
            if (response?.Status == 404)
            {
                return NotFound(response);
            }
            if (response?.Status == 401)
            {
                return Unauthorized(response);
            }
            if (response?.Status == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("unsubscribe/{userId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<Subscriber>>> UnsubscribeUser(int userId)
        {
            var response = await _userService.UnsubscribeUserById(userId);
            if (response?.Status == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
