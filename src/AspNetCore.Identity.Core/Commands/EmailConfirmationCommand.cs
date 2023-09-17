using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class EmailConfirmationCommand : ICommand<CommandHandlerResponse>
{
	public Guid UserId { get; }
	public string Token { get; }
	public EmailConfirmationCommand(Guid userId, string token)
	{
		UserId = userId;
		Token = token;
	}

}

public sealed class EmailConfirmationValidator : AbstractValidator<EmailConfirmationCommand>
{
	public EmailConfirmationValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Token).NotEmpty();
	}
}
