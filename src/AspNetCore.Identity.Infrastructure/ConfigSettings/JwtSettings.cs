namespace AspNetCore.Identity.Infrastructure.ConfigSettings;

public class JwtSettings
{
    public string SigningKey { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiresIn { get; set; } = 90; /* Number of days */
}
