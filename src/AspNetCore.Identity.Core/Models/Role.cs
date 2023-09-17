using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Core.Models;

public class Role : IdentityRole<Guid>
{
	public Role() { }
	public Role(string name) : base(name) { }
	public string Description { get; set; } = string.Empty;
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public Guid? CreatedById { get; set; }
	public User? CreatedBy { get; set; }
	public DateTime? LastEditedDate { get; set; }
	public Guid? LastEditedById { get; set; }
	public User? LastEditedBy { get; set; }
	public bool Deleted { get; set; }
	public bool Disabled { get; set; }
	public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

}

public class RoleClaim : IdentityRoleClaim<Guid> { }
