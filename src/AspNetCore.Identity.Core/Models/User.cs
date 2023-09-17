using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Core.Models;

public class User : IdentityUser<Guid>
{
	public Profile Profile { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime? RefreshTokenExpiryTime { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? LastEditedDate { get; set; }
	public bool Disabled { get; set; }
	public bool Deleted { get; set; }
	public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

}

public class UserRole : IdentityUserRole<Guid>
{
	public required User User { get; set; }
	public required Role Role { get; set; }
}
public class UserLogin : IdentityUserLogin<Guid> { }
public class UserClaim : IdentityUserClaim<Guid> { }
public class UserToken : IdentityUserToken<Guid> { }
