using System;
using System.Net.Http;
using System.Threading.Tasks;
using BackendAPI.Models;
using BackendAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BackendAPI.IntegrationTests
{
    public class OpenAIServiceTests : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAIService _openAIService;
        private readonly string _apiKey;

        public OpenAIServiceTests()
        {
            // Dans un environnement réel, ceci viendrait de la configuration
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") 
                ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set");
            
            _httpClient = new HttpClient();
            _openAIService = new OpenAIService(_httpClient, _apiKey);
        }

        [Fact]
        public async Task GenerateContentAsync_WithValidPrompt_ReturnsContent()
        {
            // Arrange
            var request = new ContentGenerationRequest
            {
                Prompt = "Write a short greeting",
                MaxTokens = 50,
                ContentType = "text"
            };

            // Act
            var result = await _openAIService.GenerateContentAsync(request);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GenerateContentAsync_WithLongPrompt_ReturnsLongerContent()
        {
            // Arrange
            var request = new ContentGenerationRequest
            {
                Prompt = "Write a paragraph about artificial intelligence",
                MaxTokens = 200,
                ContentType = "text"
            };

            // Act
            var result = await _openAIService.GenerateContentAsync(request);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Length.Should().BeGreaterThan(100);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
