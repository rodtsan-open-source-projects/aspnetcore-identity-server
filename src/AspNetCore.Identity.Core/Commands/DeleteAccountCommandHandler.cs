using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Core.Commands;

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand, CommandHandlerResponse>
{
	private readonly UserManager<User> _userManager;
	private readonly IBusinessDbContext _dbContext;
	public DeleteAccountCommandHandler(
	  UserManager<User> userManager,
	  IBusinessDbContext dbContent)
	{
		_userManager = userManager;
		_dbContext = dbContent;
	}

	public async Task<CommandHandlerResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Include(q => q.Profile)
				   .Include(q => q.UserRoles)
						.ThenInclude(q => q.Role)
				   .SingleOrDefaultAsync(q => q.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new ValidationException("User not found");

		IdentityResult result = new();
		if (user.UserRoles.Any() && !user.UserRoles.Any(q => q.Role.Name!.Contains("administrators")))
		{
			user.Deleted = true;
			user.Profile.Deleted = true;
			result = await _userManager.UpdateAsync(user);

			if (result.Errors.Any())
				throw new ValidationException(result.Errors.First().Description);
		}

		return CommandHandlerResponse.Ok(user.Id);
	}
}
