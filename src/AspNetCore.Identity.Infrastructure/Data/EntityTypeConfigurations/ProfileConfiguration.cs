using AspNetCore.Identity.Core.Extensions;
using AspNetCore.Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Identity.Infrastructure.Data.EntityTypeConfigurations;

internal class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
	public void Configure(EntityTypeBuilder<Profile> builder)
	{
		builder.HasKey(q => q.ProfileId);

		builder.HasOne(q => q.User)
			.WithOne(q => q.Profile);

		builder.Property(q => q.FirstName).HasMaxLength(50)
		.IsRequired();

		builder.Property(q => q.LastName).HasMaxLength(50)
		.IsRequired();

		builder.Property(q => q.ProfilePhotoUrl).HasMaxLength(160);


		builder.HasBaseEntity();
	}
}
