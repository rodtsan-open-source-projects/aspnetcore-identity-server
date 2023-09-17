namespace AspNetCore.Identity.Core.Models;

public class Profile : EntityBase
{
	public Guid ProfileId { get; set; }
	public User User { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public string? ProfilePhotoUrl { get; set; }
	public string? GithubUrl { get; set; }
	public string? LinkedInUrl { get; set; }
	public string? TwitterUrl { get; set; }
	public required string Email { get; set; }
	public string? Phone { get; set; }
	public string? PhotoThumbUrl { get; set; }
	public DateTime? BirthDate { get; set; }
	public override string ToString() => $"{FirstName} {LastName}";
}
