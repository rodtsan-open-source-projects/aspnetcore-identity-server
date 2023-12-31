﻿using AspNetCore.Identity.Core.Extensions;
using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Infrastructure.ConfigSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCore.Identity.Infrastructure;

public class SmtpEmailSender : ISmtpEmailSender
{
	private readonly ILogger<SmtpEmailSender> _logger;
	private readonly SmtpMailSettings _smtpSettings;
	public SmtpEmailSender(ILogger<SmtpEmailSender> logger, IOptions<SmtpMailSettings> smtpSettingsOptions)
	{
		_logger = logger;
		_smtpSettings = smtpSettingsOptions.Value;
	}

	public async Task SendAsync(string email, string displayName, string subject, string body)
	{
		using var client = new SmtpClient(_smtpSettings.SmtpHost, _smtpSettings.SmtpPort);
		client.Credentials = new NetworkCredential(_smtpSettings.SmtpUserName, _smtpSettings.SmtpPassword);
		var from = _smtpSettings.From;
		var message = new MailMessage
		{
			From = new MailAddress(from.Email, from.DisplayName),
			Subject = subject,
			IsBodyHtml = true,
			Body = body
		};
		foreach (var bcc in _smtpSettings.Bcc)
			message.Bcc.Add(new MailAddress(bcc.Email, bcc.DisplayName));

		message.To.Add(new MailAddress(email, displayName));
		await client.SendMailAsync(message);

		_logger.LogWarning("Sending email to {to} from {from} with subject {subject}.", email, from.Email, subject);
	}
}