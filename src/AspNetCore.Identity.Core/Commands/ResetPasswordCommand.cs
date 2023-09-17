using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class ResetPasswordCommand : ICommand<CommandHandlerResponse>
{
	public Guid UserId { get; set; }
	public string Token { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
	public ResetPasswordValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Token).NotEmpty();
		RuleFor(x => x.Password).NotEmpty();
	}
}
