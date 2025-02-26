using BackendAPI.Models;

namespace BackendAPI.Services
{
    public class ContentService : IContentService
    {
        private readonly IOpenAIService _openAIService;
        private readonly IUserService _userService;

        public ContentService(IOpenAIService openAIService, IUserService userService)
        {
            _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<bool> CanGenerateContent(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }

            var remainingCredits = await _userService.GetRemainingCredits(userId);
            return remainingCredits > 0;
        }

        public async Task<ContentGenerationResponse> GenerateContentAsync(ContentGenerationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                throw new ArgumentException("Prompt cannot be empty", nameof(request));
            }

            if (request.MaxTokens <= 0)
            {
                throw new ArgumentException("MaxTokens must be greater than 0", nameof(request));
            }

            try
            {
                var response = await _openAIService.GenerateAsync(request.Prompt, request.MaxTokens);
                return response;
            }
            catch (Exception ex)
            {
                return new ContentGenerationResponse
                {
                    Success = false,
                    Error = ex.Message,
                    ContentType = request.ContentType
                };
            }
        }

        public async Task<bool> SaveGeneratedContent(string userId, ContentGenerationResponse content)
        {
            // Cette méthode sera implémentée plus tard avec la persistance des données
            return true;
        }
    }
}
