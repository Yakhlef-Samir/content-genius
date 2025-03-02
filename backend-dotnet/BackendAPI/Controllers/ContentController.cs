using System;
using System.Threading.Tasks;
using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IUserService _userService;

        public ContentController(IContentService contentService, IUserService userService)
        {
            _contentService = contentService ?? throw new ArgumentNullException(nameof(contentService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] ContentGenerationRequest request)
        {
            try
            {
                var result = await _contentService.GenerateContentAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("credits/{userId}")]
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

        [HttpPost("credits/add")]
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
