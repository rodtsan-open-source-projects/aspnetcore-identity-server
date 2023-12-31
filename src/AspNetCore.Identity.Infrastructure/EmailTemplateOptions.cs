﻿using AspNetCore.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web;

namespace AspNetCore.Identity.Infrastructure
{
	public class EmailTemplateOptions : IEmailTemplateOptions
	{
		private readonly ISmtpEmailSender _emailSender;
		private readonly HttpContext? _httpContext;
		private readonly string _baseUrl;
		public EmailTemplateOptions(ISmtpEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
		{
			_emailSender = emailSender;
			_httpContext = httpContextAccessor.HttpContext;
			_baseUrl = _httpContext?.Request.BaseUrl() ?? "";
		}

		public async Task SendEmailConfirmationAsync(string email, string displayName, string userId, string token)
		{
			string encodedToken = HttpUtility.UrlEncode(token);
			string link = $"{_baseUrl}/ConfirmPassword?userid={userId}token={encodedToken}";
			var contentString = await GetContentStringAsync("PasswordReset.html");
			var content = contentString.Replace("{{LINK}}", link);
			await _emailSender.SendAsync(email, displayName, "Password Reset", content);
		}

		public async Task SendPasswordResetAsync(string email, string displayName, string userId, string token)
		{
			string encodedToken = HttpUtility.UrlEncode(token);
			string link = $"{_baseUrl}/PasswordReset?token={encodedToken}";
			var contentString = await GetContentStringAsync("PasswordReset.html");
			var content = contentString.Replace("{{LINK}}", link);
			await _emailSender.SendAsync(email, displayName, "Password Reset", content);
		}

		private async Task<string> GetContentStringAsync(string emailTemplatePath)
		{
			using var client = new HttpClient();
			client.BaseAddress = new Uri(_baseUrl);
			var response = await client.GetAsync($"/EmailTemplates/{emailTemplatePath}");
			return await response.Content.ReadAsStringAsync();
		}


	}
}
