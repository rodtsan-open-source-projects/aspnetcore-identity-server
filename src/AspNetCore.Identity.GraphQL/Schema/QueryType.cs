using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Services;
using AspNetCore.Identity.Core.Services.ViewModels;
using HotChocolate.Authorization;

namespace AspNetCore.Identity.GraphQL.Schema
{
	[Authorize]
	public class Query
	{
		public async Task<UserProfile?> GetUserProfileById([Service] IQueryService queryService, Guid id) => await queryService.GetUserProfileById(id);
		
		public async Task<IEnumerable<Role>?> GetRoles([Service] IQueryService queryService) => await queryService.GetRoles();

		public async Task<Pagination<UserProfile>> GetUserProfiles([Service] IQueryService queryService, Pagination input) => await queryService.GetUserProfiles(input);

	}

	public class QueryType : ObjectType<Query>
	{
		protected override void Configure(
			IObjectTypeDescriptor<Query> descriptor)
		{
			descriptor.Field(f => f.GetUserProfileById(default!, default!));
			descriptor.Field(f => f.GetRoles(default!));
			descriptor.Field(f => f.GetUserProfiles(default!, default!));
		}
	}
}
