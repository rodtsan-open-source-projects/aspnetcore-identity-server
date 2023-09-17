using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Responses;
using AspNetCore.Identity.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace AspNetCore.Identity.Core.Commands
{
	public class SignInCommandHandler : ICommandHandler<SignInCommand, TokenResponse>
    {
        private readonly UserManager _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenProvider _tokenProvider;
        public SignInCommandHandler(
          UserManager userManager,
          SignInManager<User> signInManager,
          ITokenProvider tokenProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
        }

        public async Task<TokenResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName) ?? throw new ValidationException("Invalid username or password");
    
            if (user!.Deleted)
         		throw new ValidationException("This account was deleted");
		
            var result = await _signInManager.CheckPasswordSignInAsync(user!, request.Password, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
					throw new ValidationException("This account is locked");
                if (result.IsNotAllowed)
					throw new ValidationException("This account is not allowed");

				throw new ValidationException("Invalid Username or Password");
            }

            var claims = await _userManager.GetClaimsByIdAsync(user.Id, cancellationToken);

            TokenResponse token = new()
            {
                AccessToken = _tokenProvider.GenerateJwtToken(claims),
                RefreshToken = _tokenProvider.GenerateRefreshToken(),
            };

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Errors.Any())
				throw new ValidationException(updateResult.Errors.First().Description);

			return token;
        }

    }
}
