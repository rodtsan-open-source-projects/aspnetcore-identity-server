namespace AspNetCore.Identity.Core.Models;

public class Profile : EntityBase
{
	public Guid ProfileId { get; set; }
	public User User { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string? ProfilePhotoUrl { get; set; }
	public string? GithubUrl { get; set; }
	public string? LinkedInUrl { get; set; }
	public string? TwitterUrl { get; set; }
	public string Email { get; set; } = string.Empty;
	public string? Phone { get; set; }
	public string? PhotoThumbUrl { get; set; }
	public DateTime? BirthDate { get; set; }
	public override string ToString() => $"{FirstName} {LastName}";
}
