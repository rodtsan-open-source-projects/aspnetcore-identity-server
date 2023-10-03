using AspNetCore.Identity.Core.Enums;
using AspNetCore.Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AspNetCore.Identity.Infrastructure.Data.Configurations;
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasOne(q => q.Profile)
				.WithOne(q => q.User)
				.HasForeignKey<Profile>(q => q.ProfileId)
				.OnDelete(DeleteBehavior.Cascade);

		builder.Property(q => q.RefreshToken)
				.HasMaxLength(120);

		builder.Property(q => q.LastEditedDate)
			.HasDefaultValue(DateTime.UtcNow)
			.ValueGeneratedOnUpdate();

		builder.Property(q => q.CreatedDate)
			.HasDefaultValue(DateTime.UtcNow)
			.ValueGeneratedOnAdd();

		builder.Property(q => q.Deleted)
		 .HasDefaultValue(false)
			 .ValueGeneratedOnAdd();

		builder.Property(q => q.Disabled)
			.HasDefaultValue(false)
				.ValueGeneratedOnAdd();

		builder.HasMany(q => q.UserRoles);

	}
}
