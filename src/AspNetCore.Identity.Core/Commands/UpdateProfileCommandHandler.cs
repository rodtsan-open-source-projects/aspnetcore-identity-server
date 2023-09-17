using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace AspNetCore.Identity.Core.Commands
{
    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, Profile>
    {
        private readonly IBusinessDbContext _dbContext;
        public UpdateProfileCommandHandler(IBusinessDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Profile> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _dbContext.Profiles.SingleOrDefaultAsync(q => q.ProfileId == request.Id, cancellationToken) ?? throw new ValidationException("Profile not found");

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            if (request.ProfilePhotoUrl.Contains("/images/profiles"))
                profile.ProfilePhotoUrl = request.ProfilePhotoUrl;
            profile.LastEditedDate = DateTime.UtcNow;
            profile.Phone = request.Phone;
            profile.LastEditedById = request.Id;

            _dbContext.Profiles.Update(profile);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return profile;
        }
    }
}
