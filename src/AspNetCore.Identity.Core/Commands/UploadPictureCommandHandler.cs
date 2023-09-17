using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Core.Commands
{
	public class UploadPictureCommandHandler : ICommandHandler<UploadPictureCommand, Profile>
    {
        private readonly IBusinessDbContext _db;
        public UploadPictureCommandHandler(IBusinessDbContext db)
        {
            _db = db;
        }
        public async Task<Profile> Handle(UploadPictureCommand request, CancellationToken cancellationToken)
        {
            var profile = await _db.Profiles.SingleOrDefaultAsync(q => q.ProfileId == request.Id, cancellationToken) ?? throw new ValidationException("User Profile not found");


			profile!.ProfilePhotoUrl = request.Path;
			profile.PhotoThumbUrl = request.Path;
			profile.LastEditedById = request.Id;
			profile.LastEditedDate = DateTime.UtcNow;
			_db.Profiles.Update(profile);

			await _db.SaveChangesAsync(cancellationToken);

            return profile;
        }
    }
}
