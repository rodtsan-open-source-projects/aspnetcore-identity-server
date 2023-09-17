using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;

namespace AspNetCore.Identity.Core.Commands;

public class UpdatePasswordCommand : ICommand<CommandHandlerResponse>
{
	public UpdatePasswordCommand(Guid userId, string oldPassword, string newPassword)
	{
		UserId = userId;
		OldPassword = oldPassword;
		NewPassword = newPassword;
	}

	public Guid UserId { get; set; }
	public string OldPassword { get; set; }
	public string NewPassword { get; set; }
}

public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
{
	public UpdatePasswordValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.OldPassword).NotEmpty();
		RuleFor(x => x.NewPassword).NotEmpty();
	}
}
