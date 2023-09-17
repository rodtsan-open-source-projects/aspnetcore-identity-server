using AspNetCore.Identity.Core.Extensions;
using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Dynamic.Core;

namespace AspNetCore.Identity.Core.Services;

public class QueryService : IQueryService
{
	private readonly IBusinessDbContext _dbContext;
	private readonly IDistributedCache _cache;
	public QueryService(IBusinessDbContext dbContext, IDistributedCache cache)
	{
		_dbContext = dbContext;
		_cache = cache;
	}

	public async Task<IEnumerable<Role>> GetRoles() => await GetRoles(false);
	public async Task<IEnumerable<Role>> GetRoles(bool includeDeletedRoles)
	{
		var CACHE_KEY = $"Roles_{includeDeletedRoles}";

		if (_cache.TryGetValue(CACHE_KEY, out List<Role>? cachedRoles) && cachedRoles != null)
		{
			return cachedRoles;
		}
		var roles = await _dbContext.Roles.ToListAsync();

		var cacheEntryOptions = new DistributedCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromSeconds(3600))
				.SetAbsoluteExpiration(TimeSpan.FromSeconds(18000));

		await _cache.SetAsync(CACHE_KEY, roles, cacheEntryOptions);

		return roles;
	}
	public async Task<Pagination<UserProfile>> GetUserProfiles(Pagination page)
	{
		var keywords = page.Keywords.ToLower();
		var pageSize = Math.Max(page.PageSize, 5);
		var pageNumber = Math.Max(1, page.PageNumber);
		var orderBy = page.OrderBy.ToLower();

		var CACHE_KEY = $"UserProfiles_{keywords.Replace(" ", "_")}_{orderBy}_{pageNumber}_{pageSize}";

		if (_cache.TryGetValue(CACHE_KEY, out Pagination<UserProfile>? cachedUserProfiles))
		{
			return cachedUserProfiles ?? new();
		}

		var query = _dbContext.Users
				.Include(q => q.Profile)
				.Include(q => q.UserRoles)
					.ThenInclude(q => q.Role)
				.Where(q => q.Deleted == false)
				.Select(q => new
				{
					q.Id,
					q.Profile.Email,
					q.Profile.FirstName,
					q.Profile.LastName,
					q.Profile.PhotoThumbUrl,
					q.Profile.GithubUrl,
					q.Profile.TwitterUrl,
					q.Profile.LinkedInUrl,
					q.Profile.Phone,
					q.Profile.BirthDate,
					Role = q.UserRoles.Select(q => q.Role.Name).FirstOrDefault(),
				}).AsQueryable();

		if (!string.IsNullOrEmpty(keywords))
		{
			query = query.Where(q => q.Email!.Contains(keywords) ||
								q.FirstName.Contains(keywords) ||
								q.LastName.Contains(keywords));
		}

		if (!string.IsNullOrEmpty(orderBy))
		{
			query = query.OrderBy(orderBy);
		}

		var recordCount = query.Count();
		var pageCount = (int)Math.Ceiling((double)recordCount / pageSize);
		var skipPageNumber = pageNumber - 1;

		var records = await query
			.Skip(skipPageNumber * pageSize)
			.Take(pageSize)
			.Select(q => new UserProfile
			{
				Id = q.Id,
				Email = q.Email,
				FirstName = q.FirstName,
				LastName = q.LastName,
				ProfilePhotoUrl = q.PhotoThumbUrl,
				GithubUrl = q.GithubUrl,
				TwitterUrl = q.TwitterUrl,
				LinkedInUrl = q.LinkedInUrl,
				Phone = q.Phone,
				BirthDate = q.BirthDate,
				Role = q.Role,
			}).ToListAsync();

		var pageResult = new Pagination<UserProfile>
		{
			PageNumber = pageNumber,
			PageSize = pageSize,
			PageCount = pageCount,
			RecordCount = recordCount,
			Keywords = keywords,
			OrderBy = orderBy,
			Records = records
		};

		var cacheEntryOptions = new DistributedCacheEntryOptions()
			.SetSlidingExpiration(TimeSpan.FromSeconds(20))
			.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

		await _cache.SetAsync(CACHE_KEY, pageResult, cacheEntryOptions);

		return pageResult;
	}
	public async Task<UserProfile?> GetUserProfileById(Guid id)
	{
		var CACHE_KEY = $"{nameof(UserProfile)}_{id}";

		if (_cache.TryGetValue(CACHE_KEY, out UserProfile? cachedUserProfile) && cachedUserProfile != null)
		{
			return cachedUserProfile;
		}

		return await _dbContext.Users
			.Include(q => q.Profile)
			.Include(q => q.UserRoles)
				.ThenInclude(q => q.Role)
			.Where(q => q.Id == id)
			.Select(q => new UserProfile
			{
				Id = q.Id,
				Email = q.Profile.Email,
				FirstName = q.Profile.FirstName,
				LastName = q.Profile.LastName,
				ProfilePhotoUrl = q.Profile.PhotoThumbUrl,
				GithubUrl = q.Profile.GithubUrl,
				TwitterUrl = q.Profile.TwitterUrl,
				LinkedInUrl = q.Profile.LinkedInUrl,
				Phone = q.Profile.Phone,
				BirthDate = q.Profile.BirthDate,
				Role = q.UserRoles.Select(q => q.Role.Name).FirstOrDefault(string.Empty)
			})
			.SingleOrDefaultAsync();
	}
}
