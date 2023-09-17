using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Services.ViewModels;

namespace AspNetCore.Identity.Core.Services
{
	public interface IQueryService
	{
		Task<IEnumerable<Role>> GetRoles();
		Task<IEnumerable<Role>> GetRoles(bool includeDeletedRoles);
		Task<UserProfile?> GetUserProfileById(Guid id);
		Task<Pagination<UserProfile>> GetUserProfiles(Pagination page);
	}
}