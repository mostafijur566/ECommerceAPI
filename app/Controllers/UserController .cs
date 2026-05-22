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
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // POST api/user/register-admin
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")] // only existing admin can create another
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto)
        {
            if (await _userRepo.EmailExistsAsync(dto.Email))
                return BadRequest(ApiResponse<string>.FailResponse("Email already exists."));

            var user = await _userRepo.RegisterAsync(dto, role: "Admin");
            return Ok(ApiResponse<UserResponseDto>.SuccessResponse(user, "Admin registered successfully."));
        }

        // POST api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _userRepo.EmailExistsAsync(dto.Email))
                return BadRequest(new { message = "Email already exists." });

            var user = await _userRepo.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // POST api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _userRepo.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result);
        }

        // GET api/user/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        // DELETE api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userRepo.DeleteAsync(id);
            if (!result) return NotFound(new { message = "User not found." });

            return NoContent();
        }
    }
}