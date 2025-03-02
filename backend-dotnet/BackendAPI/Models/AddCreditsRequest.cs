namespace BackendAPI.Models
{
    public class AddCreditsRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
} 