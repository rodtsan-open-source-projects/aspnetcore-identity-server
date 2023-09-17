using AspNetCore.Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Identity.Infrastructure.Data.EntityTypeConfigurations;

internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(q => new { q.UserId, q.RoleId });
		builder.HasOne(q => q.Role)
			.WithMany(q => q.UserRoles)
			.HasForeignKey(q => q.RoleId)
			.IsRequired();

		builder.HasOne(q => q.User)
			.WithMany(q => q.UserRoles)
			.HasForeignKey(q => q.UserId)
			.IsRequired();

	}
}
