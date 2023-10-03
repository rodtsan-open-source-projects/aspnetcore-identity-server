using Autofac;
using Autofac.Extensions.DependencyInjection;
using AspNetCore.Identity.Core;
using AspNetCore.Identity.Core.Behaviors;
using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Services;
using AspNetCore.Identity.GraphQL.Schema;
using AspNetCore.Identity.Infrastructure;
using AspNetCore.Identity.Infrastructure.ConfigSettings;
using AspNetCore.Identity.Infrastructure.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Serilog;
using System.Reflection;
using Path = System.IO.Path;

namespace AspNetCore.Identity.GraphQL
{
	public class Program
	{
		const string SQL_CACHE_SETTINGS_CONFIG = "SqlCacheSettings";
		const string MEDIATR_ASSEMBLY_NAME = "AspNetCore.Identity.Core";

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddCors(options => options.AddDefaultPolicy(
				builder =>
				{
					builder.WithOrigins(new string[]
					{
						"http://localhost:3000"
					})
					.AllowAnyHeader()
					.AllowAnyMethod();
				}));

			builder.Services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			builder.Services.AddInfrastructureServices(builder.Configuration);
			builder.Services.AddAutofac(cfg =>
			{
				cfg.RegisterModule<DefaultCoreModule>();
			});

			builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load(MEDIATR_ASSEMBLY_NAME)));
			builder.Services.AddValidatorsFromAssembly(Assembly.Load(MEDIATR_ASSEMBLY_NAME));
			
			// Reserve for redis cache
			//builder.Services.AddStackExchangeRedisCache(options =>
			//{
			//	options.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
			//	options.InstanceName = "SampleInstance";
			//});

			var sqlCacheSettings = new SqlCacheSettings();
			builder.Configuration.GetSection(SQL_CACHE_SETTINGS_CONFIG).Bind(sqlCacheSettings);

			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddDistributedSqlServerCache(options =>
			{
				options.ConnectionString = sqlCacheSettings.ConnectionString;
				options.SchemaName = sqlCacheSettings.SchemaName;
				options.TableName = sqlCacheSettings.TableName;
			});

			builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
			builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			builder.Services.AddTransient<ExceptionHandlingMiddleware>();
		    builder.Services.AddTransient<IEmailTemplateOptions, EmailTemplateOptions>();
			builder.Services.AddTransient<ISmtpEmailSender, SmtpEmailSender>();
			builder.Services.AddTransient<IQueryService, QueryService>();

			builder.Services
				.AddGraphQLServer()
				.AddAuthorization()
				.AddQueryType<QueryType>()
				.AddMutationType<MutationType>()
				.AddType<UploadType>()
				//.AddMutationConventions()
				.ModifyOptions(x => x.RemoveUnreachableTypes = true)
				.ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment()); 

			var app = builder.Build();

			app.UseMiddleware<ExceptionHandlingMiddleware>();
			app.UseCors();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
				OnPrepareResponse = ctx =>
				{
					ctx.Context.Response.Headers.Append(
						 "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
				}
			});

			app.MapGraphQL();

			app.Run();
		}
	}

	
}