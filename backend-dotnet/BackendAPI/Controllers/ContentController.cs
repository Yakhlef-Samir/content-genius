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
        public async Task<ActionResult<ContentGenerationResponse>> GenerateContent([FromBody] ContentGenerationRequest request)
        {
            try
            {
                var userId = User.Identity.Name;

                if (!await _userService.HasAvailableCreditsAsync(userId))
                {
                    return BadRequest(new { error = "Insufficient credits" });
                }

                var response = await _contentService.GenerateContentAsync(request);
                
                // Déduire les crédits seulement si la génération a réussi
                if (response.Success)
                {
                    await _userService.DeductCreditsAsync(userId);
                }

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log l'erreur ici
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpGet("credits")]
        public async Task<ActionResult<int>> GetCreditsBalance()
        {
            try
            {
                var userId = User.Identity.Name;
                var balance = await _userService.GetCreditsBalanceAsync(userId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                // Log l'erreur ici
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpPost("credits/add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCredits(string userId, int amount)
        {
            try
            {
                await _userService.AddCreditsAsync(userId, amount);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log l'erreur ici
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }
    }
}
