using AspNetCore.Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AspNetCore.Identity.Core.Interfaces
{
	public interface IBusinessDbContext
	{
		DatabaseFacade Database { get; }
		DbSet<Profile> Profiles { get; }
		DbSet<Role> Roles { get; }
		DbSet<User> Users { get; }

		void RollBack();
		int SaveChanges();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		DbSet<TEntity> Set<TEntity>() where TEntity : class;
	}
}