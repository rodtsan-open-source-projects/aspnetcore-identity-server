using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class SignUpCommand : ICommand<CommandHandlerResponse>
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public sealed class SignUpValidator : AbstractValidator<SignUpCommand>
{
	public SignUpValidator()
	{
		RuleFor(x => x.FirstName).NotEmpty();
		RuleFor(x => x.LastName).NotEmpty();
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Password).NotEmpty();
	}
}