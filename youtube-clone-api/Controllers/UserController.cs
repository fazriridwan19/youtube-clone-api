using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Services;

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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<User>>> GetUser(int id)
        {
            var response = await _userService.GetUser(id);
            if (response?.Message == "Data not found")
            {
                return NotFound(response);
            }
            if (response?.Message == "Not Authorize")
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
    }
}
