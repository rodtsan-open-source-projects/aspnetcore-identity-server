using AspNetCore.Identity.Core.Enums;
using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class SignInCommand : ICommand<TokenResponse>
{
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;

}

public sealed class SignInValidator : AbstractValidator<SignInCommand>
{
	public SignInValidator()
	{
		RuleFor(x => x.UserName).NotEmpty().EmailAddress();
		RuleFor(x => x.Password).NotEmpty();
	}
}
