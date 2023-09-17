using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;


namespace AspNetCore.Identity.Core.Commands
{
	public class UpdateProfileCommand : ICommand<Profile>
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string ProfilePhotoUrl { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
