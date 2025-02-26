namespace BackendAPI.Models
{
    public class ContentGenerationResponse
    {
        public string Content { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string ContentType { get; set; } = "text";
        public int TokensUsed { get; set; }
    }
}
