namespace BackendAPI.Models
{
    public class ContentGenerationRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public int MaxTokens { get; set; } = 150;
        public string ContentType { get; set; } = "text";
    }
}
