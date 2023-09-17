using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AspNetCore.Identity.Core.Models;


namespace AspNetCore.Identity.Core.Extensions
{
    public static class ModelBuilderExtensions
    {
		public static void HasBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : EntityBase
        {
            builder.HasOne(q => q.CreatedBy)
                .WithMany()
                .HasForeignKey(q => q.CreatedById)
				.OnDelete(DeleteBehavior.NoAction);

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
        }

        public static void StringMaxLengthConvention(this ModelBuilder builder, int len)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetMaxLength(100);
                    }
                }
            }
        }

		public static void PluralizeTableConvention(this ModelBuilder builder)
		{
			foreach (var entity in builder.Model.GetEntityTypes())
			{
                entity.SetTableName(Pluralizer.Pluralize(entity.GetTableName()));
			}
		}
	}
}
