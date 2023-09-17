using AspNetCore.Identity.Core.Extensions;
using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace AspNetCore.Identity.Infrastructure.Data
{
	public sealed class BusinessDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IBusinessDbContext
	{
		// private readonly IDomainEventDispatcher? _dispatcher;
		public BusinessDbContext(DbContextOptions<BusinessDbContext> options /*, IDomainEventDispatcher? dispatcher */) : base(options)
		{
			// _dispatcher = dispatcher;
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{

			builder.StringMaxLengthConvention(80);
			// builder.PluralizeTableConvention();

			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		public override DatabaseFacade Database => base.Database;

		public void RollBack()
		{
			foreach (var entry in ChangeTracker.Entries())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.State = EntityState.Detached;
						break;
				}
			}
		}
		public override int SaveChanges()
		{
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return base.SaveChangesAsync(cancellationToken);
		}

		public override DbSet<TEntity> Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public override DbSet<Role> Roles => Set<Role>();
		public override DbSet<User> Users => Set<User>();
		public DbSet<Profile> Profiles => Set<Profile>();

	}
}

