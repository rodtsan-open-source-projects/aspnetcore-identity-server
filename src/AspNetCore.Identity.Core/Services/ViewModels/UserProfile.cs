namespace AspNetCore.Identity.Core.Services.ViewModels
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid CompanyId { get; set; } = Guid.Empty;
        public string? CompanyName { get; set; } = string.Empty;
        public string? ProfilePhotoUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? PhotoThumbUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Role { get; set; }
    }
}
