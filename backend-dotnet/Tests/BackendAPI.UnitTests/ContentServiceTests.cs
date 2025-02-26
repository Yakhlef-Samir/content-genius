using Xunit;
using Moq;
using FluentAssertions;
using BackendAPI.Services;
using BackendAPI.Models;

namespace BackendAPI.UnitTests
{
    public class ContentServiceTests
    {
        private readonly Mock<IOpenAIService> _mockOpenAIService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly ContentService _contentService;

        public ContentServiceTests()
        {
            _mockOpenAIService = new Mock<IOpenAIService>();
            _mockUserService = new Mock<IUserService>();
            _contentService = new ContentService(_mockOpenAIService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GenerateContentAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new ContentGenerationRequest
            {
                Prompt = "Write a blog post about AI",
                MaxTokens = 150,
                ContentType = "text"
            };

            var expectedResponse = new ContentGenerationResponse
            {
                Content = "This is a generated blog post about AI",
                Success = true,
                ContentType = "text",
                TokensUsed = 10
            };

            _mockOpenAIService
                .Setup(x => x.GenerateAsync(request.Prompt, request.MaxTokens))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _contentService.GenerateContentAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Content.Should().Be(expectedResponse.Content);
            result.TokensUsed.Should().Be(expectedResponse.TokensUsed);
        }

        [Fact]
        public async Task GenerateContentAsync_EmptyPrompt_ThrowsArgumentException()
        {
            // Arrange
            var request = new ContentGenerationRequest
            {
                Prompt = "",
                MaxTokens = 150
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _contentService.GenerateContentAsync(request)
            );
        }

        [Fact]
        public async Task CanGenerateContent_UserHasCredits_ReturnsTrue()
        {
            // Arrange
            var userId = "user123";
            _mockUserService
                .Setup(x => x.GetRemainingCredits(userId))
                .ReturnsAsync(5);

            // Act
            var result = await _contentService.CanGenerateContent(userId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanGenerateContent_UserNoCredits_ReturnsFalse()
        {
            // Arrange
            var userId = "user123";
            _mockUserService
                .Setup(x => x.GetRemainingCredits(userId))
                .ReturnsAsync(0);

            // Act
            var result = await _contentService.CanGenerateContent(userId);

            // Assert
            result.Should().BeFalse();
        }
    }
}
