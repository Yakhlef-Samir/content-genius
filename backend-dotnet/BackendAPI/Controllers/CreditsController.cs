using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CreditsController : ControllerBase
    {
        private readonly IUserService _userService;

        public CreditsController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCredits(string userId)
        {
            try
            {
                var credits = await _userService.GetCreditsBalanceAsync(userId);
                return Ok(new { credits });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCredits([FromBody] AddCreditsRequest request)
        {
            try
            {
                await _userService.AddCreditsAsync(request.UserId, request.Amount);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
} 