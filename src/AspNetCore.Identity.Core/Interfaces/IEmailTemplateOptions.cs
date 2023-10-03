namespace AspNetCore.Identity.Core.Interfaces;

public interface IEmailTemplateOptions
{
	Task SendEmailConfirmationAsync(string email, string displayName, string userId, string token);
	Task SendPasswordResetAsync(string email, string displayName, string userId, string token);
}