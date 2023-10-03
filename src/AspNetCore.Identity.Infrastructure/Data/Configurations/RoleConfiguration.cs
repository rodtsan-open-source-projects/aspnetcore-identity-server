using AspNetCore.Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Identity.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.HasOne(q => q.CreatedBy)
			.WithMany()
			.HasForeignKey(q => q.CreatedById);

		builder.HasOne(q => q.LastEditedBy)
			.WithMany()
			.HasForeignKey(q => q.LastEditedById);

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
