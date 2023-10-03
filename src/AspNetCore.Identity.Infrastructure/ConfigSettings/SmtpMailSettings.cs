namespace AspNetCore.Identity.Infrastructure.ConfigSettings;

public class MailFrom
{
	public string DisplayName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
}
public class SmtpMailSettings
{
	public string SmtpHost { get; set; } = string.Empty;
	public int SmtpPort { get; set;}
	public string SmtpUserName { get; set; } = string.Empty;
	public string SmtpPassword { get; set; } = string.Empty;
	public MailFrom From { get; set; } = new();
	public List<MailFrom> Bcc { get; set; } = new List<MailFrom>();

}
