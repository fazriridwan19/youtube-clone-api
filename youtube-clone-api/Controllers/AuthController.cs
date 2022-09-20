using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Services.AuthService;

namespace youtube_clone_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.Login(request);
            if (response.Status == 404) return NotFound(response);
            if (response.Status == 400) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<User>>> Register([FromBody] RegisterRequestDto request)
        {
            return Ok(await _authService.Register(request));
        }
    }
}
