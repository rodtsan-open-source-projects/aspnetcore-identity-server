using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Services;
using AspNetCore.Identity.Infrastructure.ConfigSettings;
using AspNetCore.Identity.Infrastructure.Data;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AspNetCore.Identity.Infrastructure
{
	public static class DefaultInfrastructureServices
	{
		const string JWT_SETTINGS_CONFIG = "JwtSettings";
		const string SMTP_MAIL_SETTINGS_CONFIG = "SmtpMailSettings";
		const string CONNECTION_STRING_NAME = "DefaultConnection";
		const string MIGRATION_ASSEMBLY_NAME = "AspNetCore.Identity.Infrastructure";
		public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<IBusinessDbContext, BusinessDbContext>(options =>
			{
				options.UseSqlServer(config.GetConnectionString(CONNECTION_STRING_NAME), optionsAction =>
				{
					optionsAction.MigrationsAssembly(MIGRATION_ASSEMBLY_NAME);
					optionsAction.EnableRetryOnFailure();
				});
			});
			services.AddIdentity<User, Role>()
			.AddEntityFrameworkStores<BusinessDbContext>()
			.AddRoleManager<RoleManager<Role>>()
			.AddUserManager<UserManager>()
			.AddSignInManager<SignInManager<User>>()
			.AddDefaultTokenProviders();

			services.Configure<JwtSettings>(
			config.GetSection(JWT_SETTINGS_CONFIG));
			services.Configure<SmtpMailSettings>(
			config.GetSection(SMTP_MAIL_SETTINGS_CONFIG));

			var jwtSettings = new JwtSettings();
			config.GetSection(JWT_SETTINGS_CONFIG).Bind(jwtSettings);


			var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey));
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer("Bearer", options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateAudience = false,
					ValidateIssuer = false,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = signingKey,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(jwtSettings.ExpiresIn) //the default for this setting is 5 minutes
				};
				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
						{
							context.Response.Headers.Add("Token-Expired", "true");
						}
						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
				{
					policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
					policy.RequireClaim(JwtRegisteredClaimNames.Name);
					policy.RequireClaim(JwtClaimTypes.Role);
				});
			});

			services.AddTransient<ITokenProvider, TokenProvider>();
			services.AddTransient<ISmtpEmailSender, SmtpEmailSender>();
			services.AddTransient<IEmailTemplateOptions, EmailTemplateOptions>();
		}
	}
}
