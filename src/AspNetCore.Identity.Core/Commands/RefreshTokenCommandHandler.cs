using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using AspNetCore.Identity.Core.Services;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
	private readonly ITokenProvider _tokenProvider;
	private readonly UserManager _userManager;
	public RefreshTokenCommandHandler(UserManager userManager, ITokenProvider tokenProvider)
	{
		_userManager = userManager;
		_tokenProvider = tokenProvider;
	}
	public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var claimsIdentity = await _tokenProvider.GetPrincipalFromExpiredToken(request.AccessToken);

		if (!claimsIdentity.Claims.Any())
			throw new ValidationException("Error in GetPrincipalFromExpiredToken");

		var user = await _userManager.FindByNameAsync(claimsIdentity.Name ?? string.Empty) ?? throw new ValidationException("User not found");


		if (user!.RefreshTokenExpiryTime >= DateTime.UtcNow)
			throw new ValidationException("Refresh Token has been expired");

		var claims = await _userManager.GetClaimsByIdAsync(user!.Id, cancellationToken);

		TokenResponse newToken = new()
		{
			AccessToken = _tokenProvider.GenerateJwtToken(claims),
			RefreshToken = _tokenProvider.GenerateRefreshToken()
		};

		user.RefreshToken = newToken.RefreshToken;

		var updateResult = await _userManager.UpdateAsync(user);

		if (updateResult.Errors.Any())
			throw new ValidationException(updateResult.Errors.First().Description);

		return newToken;
	}
}
