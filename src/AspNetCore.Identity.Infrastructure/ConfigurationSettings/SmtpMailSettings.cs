namespace AspNetCore.Identity.Infrastructure.ConfigurationSettings
{
	public class SmtpMailSettings
	{
		public string SmtpHost { get; set; } = string.Empty;
		public int SmtpPort { get; set;}
		public string SmtpUserName { get; set; } = string.Empty;
		public string SmtpPassword { get; set; } = string.Empty;
		public MailFrom From { get; set; } = new();
		public string BccEmails { get; set; } = string.Empty;

		public class MailFrom
		{
			public string Name { get; set; } = string.Empty;
			public string Email { get; set; } = string.Empty;
		}
	}
}
