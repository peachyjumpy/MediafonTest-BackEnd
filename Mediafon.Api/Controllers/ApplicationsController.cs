using Mediafon.Api.Data;
using Mediafon.Api.DTOs;
using Mediafon.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mediafon.Api.Controllers
{
    [ApiController]
    [Route("api/applications")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly MediafonDbContext _context;

        public ApplicationsController(MediafonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var applications = await _context.ApplicationRequests
                .Where(a => a.UserId == int.Parse(userId))
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new ApplicationResponseDto
                {
                    Id = a.Id,
                    Type = a.Type,
                    Message = a.Message,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(applications);
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var application = new ApplicationRequest
            {
                UserId = userId,
                Type = dto.Type,
                Message = dto.Message,
                Status = "submitted",
                CreatedAt = DateTime.UtcNow
            };

            _context.ApplicationRequests.Add(application);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
