namespace AspNetCore.Identity.Core.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn => 5 * 60 * 1000;
        public string TokenType => "Bearer";
    }
}
