using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class SendResetPasswordCommand : ICommand<CommandHandlerResponse>
{
	public string Email { get; set; } = string.Empty;
}

public sealed class SendResetPasswordValidator : AbstractValidator<SendResetPasswordCommand>
{
	public SendResetPasswordValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
	}
}
