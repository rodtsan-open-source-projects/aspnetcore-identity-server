using AspNetCore.Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Identity.Infrastructure.Data.Configurations;

internal class DistributedCacheConfiguration : IEntityTypeConfiguration<DistributedCache>
{
	public void Configure(EntityTypeBuilder<DistributedCache> builder)
	{
		builder.HasKey(q => q.Id);

		builder.Property(q => q.Id)
			.HasColumnType("nvarchar(449)")
			.ValueGeneratedNever()
			.IsRequired();

		builder.Property(q => q.Value)
			.HasColumnType("varbinary(max)")
			.IsRequired();

		builder.Property(q => q.ExpiresAtTime)
			.HasColumnType("datetimeoffset(7)")
			.IsRequired();

		builder.Property(q => q.SlidingExpirationInSeconds)
			.HasColumnType("bigint");

		builder.Property(q => q.AbsoluteExpiration)
			.HasColumnType("datetimeoffset(7)");


	}
}
