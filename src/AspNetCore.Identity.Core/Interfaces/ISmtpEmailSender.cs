using System.Net.Mail;

namespace AspNetCore.Identity.Core.Interfaces;

public interface ISmtpEmailSender
{
	Task SendAsync(string email, string displayName, string subject, string body);
}
