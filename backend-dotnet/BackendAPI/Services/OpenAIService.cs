using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BackendAPI.Models;

namespace BackendAPI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string OPENAI_API_URL = "https://api.openai.com/v1/completions";

        public OpenAIService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
        }

        public async Task<string> GenerateContentAsync(ContentGenerationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Prompt))
                throw new ArgumentException("Prompt cannot be empty", nameof(request));

            var openAIRequest = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a professional content creator." },
                    new { role = "user", content = request.Prompt }
                },
                max_tokens = request.MaxTokens,
                temperature = 0.7
            };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(openAIRequest),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await _httpClient.PostAsync(OPENAI_API_URL, requestContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);

                return result?.Choices?[0]?.Message?.Content 
                    ?? throw new InvalidOperationException("Failed to get valid response from OpenAI");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to communicate with OpenAI API", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to parse OpenAI response", ex);
            }
        }
    }

    internal class OpenAIResponse
    {
        public Choice[] Choices { get; set; }
    }

    internal class Choice
    {
        public Message Message { get; set; }
    }

    internal class Message
    {
        public string Content { get; set; }
    }
}
