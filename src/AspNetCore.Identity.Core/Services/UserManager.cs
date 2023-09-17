using AspNetCore.Identity.Core.Extensions;
using AspNetCore.Identity.Core.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AspNetCore.Identity.Core.Services;

public class UserManager : UserManager<User>
{
	public UserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
	{
	
	}

	public virtual async Task<IList<Claim>> GetClaimsByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var user = await Users.Include(q => q.Profile)
			.Include(q => q.Profile)
			.Include(q => q.UserRoles)
				.ThenInclude(q => q.Role)
			.SingleOrDefaultAsync(user => user.Id == id, cancellationToken) ?? throw new ArgumentNullException("User not found");

		IList<Claim> claims = new List<Claim>()
		{
			new Claim (JwtClaimTypes.Id, user.Id.ToString()),
			new Claim (ClaimTypes.Name, user.UserName!),
			new Claim (JwtClaimTypes.Email, user.Email!),
			new Claim (JwtClaimTypes.GivenName, user.Profile.FirstName),
			new Claim (JwtClaimTypes.FamilyName, user.Profile.LastName),
			// new Claim (ExtendedJwtClaimsTypes.UserType, user.UserTypeId.ToString()),
		};

		if (user.UserRoles.Any())
		{
			foreach (var userRole in user.UserRoles)
			{
				// Role has null warnings here even though it should not happen. see .NET 7 <Nullable>enable</Nullable>
				claims.Add(new Claim(JwtClaimTypes.Role, userRole.Role!.Name ?? "user"));
			}
		}
		return claims;
	}
}
