using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Responses;
using AspNetCore.Identity.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Core.Commands;
public class SignUpCommandHandler : ICommandHandler<SignUpCommand, CommandHandlerResponse>
{
    private const string DEFAULT_ROLE = "user"; // 
    private readonly UserManager _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IEmailTemplateOptions _emailTemplate;
    public SignUpCommandHandler(UserManager userManager,
        RoleManager<Role> roleManager,
		IEmailTemplateOptions emailTemplate)
    {
        _userManager = userManager;
        _roleManager = roleManager;
		_emailTemplate = emailTemplate;
    }

    public async Task<CommandHandlerResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            UserName = request.Email.ToLower(),
            Email = request.Email.ToLower(),
            Profile = new Profile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email.ToLower(),
            },
        };

        user.Profile.CreatedBy = user;

        var result = await _userManager.CreateAsync(user, request.Password);

		if (result.Errors.Any())
			throw new ValidationException(result.Errors.First().Description);

		bool roleExists = await _roleManager.RoleExistsAsync(DEFAULT_ROLE);

        if (!roleExists)
        {
            var createRoleResult = await _roleManager.CreateAsync(new Role(DEFAULT_ROLE));
            if (createRoleResult.Errors.Any())
				throw new ValidationException(createRoleResult.Errors.First().Description);
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, DEFAULT_ROLE);
        if (addToRoleResult.Errors.Any())
			throw new ValidationException(addToRoleResult.Errors.First().Description);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        await _emailTemplate.SendEmailConfirmationAsync(user.Email, user.Profile.ToString(), user.Id.ToString(), token);

        return CommandHandlerResponse.Ok();
    }

}
