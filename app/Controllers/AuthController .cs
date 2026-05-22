using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.User;
using app.Helper;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        // POST api/auth/register-admin
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")] // only existing admin can create another
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto)
        {
            if (await _authRepo.EmailExistsAsync(dto.Email))
                return BadRequest(ApiResponse<string>.FailResponse("Email already exists."));

            var user = await _authRepo.RegisterAsync(dto, role: "Admin");
            return Ok(ApiResponse<UserResponseDto>.SuccessResponse(user, "Admin registered successfully."));
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _authRepo.EmailExistsAsync(dto.Email))
                return BadRequest(new { message = "Email already exists." });

            var user = await _authRepo.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authRepo.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result);
        }

        // GET api/auth/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _authRepo.GetByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        // DELETE api/auth/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _authRepo.DeleteAsync(id);
            if (!result) return NotFound(new { message = "User not found." });

            return NoContent();
        }
    }
}