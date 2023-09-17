using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands
{
    public class RefreshTokenCommand : ICommand<TokenResponse>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

	public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
	{
		public RefreshTokenValidator() {
			RuleFor(x => x.AccessToken).NotEmpty();
			RuleFor(x => x.RefreshToken).NotEmpty();
		}
	}
}
