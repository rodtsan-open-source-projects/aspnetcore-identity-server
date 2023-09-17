using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Core.Commands
{
    public class SendResetPasswordCommandHandler : ICommandHandler<SendResetPasswordCommand, CommandHandlerResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailTemplate _emailTemplate;
        public SendResetPasswordCommandHandler(UserManager<User> userManager, IEmailTemplate emailTemplate)
        {
            _userManager = userManager;
           _emailTemplate = emailTemplate;
        }

        public async Task<CommandHandlerResponse> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.Users
                .Include(q => q.Profile)
                .SingleOrDefaultAsync(q => q.Email!.ToLower() == request.Email.ToLower(), cancellationToken: cancellationToken) ?? throw new ValidationException("User cannot not be found");

        
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailTemplate.SendPasswordResetAsync(user.Email!, user.Profile.ToString(), user.Id.ToString(), token);
			
			return CommandHandlerResponse.Ok();
        }
    }
}
