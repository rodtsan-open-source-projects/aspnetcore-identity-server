using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using AspNetCore.Identity.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AspNetCore.Identity.Core.Commands
{
	public class RevokeCommandHandler : ICommandHandler<RevokeCommand, CommandHandlerResponse>
	{
		private readonly UserManager _userManager;
		private readonly ITokenProvider _tokenProvider;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public RevokeCommandHandler(IHttpContextAccessor httpContextAccessor, UserManager userManager, ITokenProvider tokenProvider)
		{
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			_tokenProvider = tokenProvider;
		}

		public async Task<CommandHandlerResponse> Handle(RevokeCommand request, CancellationToken cancellationToken)
		{

			var userNameFormToken = await GetUserNameFromToken();

			var userName = userNameFormToken ?? request.UserName ?? string.Empty;

			var user = await _userManager.FindByNameAsync(userName) ?? throw new ValidationException("User not found");

			user!.RefreshToken = null;
			user!.RefreshTokenExpiryTime = null;

			var updateResult = await _userManager.UpdateAsync(user);

			if (updateResult.Errors.Any())
				throw new ValidationException(updateResult.Errors.First().Description);

			return CommandHandlerResponse.Ok(user!.Id);
		}

		private async Task<string?> GetUserNameFromToken()
		{
			var httpContext = _httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(IHttpContextAccessor));
			if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values))
			{
				if (values.Any())
				{
					var token = values[0]?["Bearer ".Length..] ?? "";
					var identity = await _tokenProvider.GetPrincipalFromExpiredToken(token);
					return identity?.Name;
				}
				return null;
			}

			return null;
		}
	}
}
