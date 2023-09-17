using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Core.Commands
{
	public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, CommandHandlerResponse>
	{
		private readonly UserManager<User> _userManager;
		public ResetPasswordCommandHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<CommandHandlerResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
		{

			var user = await _userManager.FindByIdAsync(request.UserId.ToString()) ?? throw new ValidationException("User not found");

			var result = await _userManager.ResetPasswordAsync(user!, request.Token, request.Password);

			if (result.Errors.Any())
				throw new ValidationException(result.Errors.First().Description);
		

			return CommandHandlerResponse.Ok();
		}
	}
}
