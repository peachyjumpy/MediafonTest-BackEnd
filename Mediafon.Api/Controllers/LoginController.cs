using Mediafon.Api.Data;
using Mediafon.Api.DTOs;
using Mediafon.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mediafon.Api.Services;

namespace Mediafon.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly MediafonDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtTokenService _jwtService;

        public LoginController(MediafonDbContext context, JwtTokenService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password.");
            }
            // Generate a simple token (for demonstration purposes only)
            var token = _jwtService.GenerateToken(user);

            return Ok(new LoginResponseDto { Token = token });

        }

    }
}
