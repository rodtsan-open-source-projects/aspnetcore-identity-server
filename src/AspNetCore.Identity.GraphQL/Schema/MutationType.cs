using AspNetCore.Identity.Core.Commands;
using AspNetCore.Identity.Core.Models;
using AspNetCore.Identity.Core.Responses;
using Humanizer;
using MediatR;
using Path = System.IO.Path;

namespace AspNetCore.Identity.GraphQL.Schema
{
	public class Mutation
	{
		private const string PROFILE_IMAGES_URL = "/images/profiles/{0}";
		private const string PROFILE_IMAGES_PATH = @"wwwroot\images\profiles";
		public async Task<CommandHandlerResponse> SignUp([Service] ISender sender, SignUpCommand input, CancellationToken cancellationToken)
		{
			return await sender.Send(input, cancellationToken);
		}

		public async Task<TokenResponse> SignIn([Service] ISender sender, SignInCommand input) => await sender.Send(input);

		public async Task<CommandHandlerResponse> DeleteAccount([Service] ISender sender, DeleteAccountCommand input) => await sender.Send(input);

		public async Task<TokenResponse> RefreshToken([Service] ISender sender, RefreshTokenCommand input) => await sender.Send(input);
	
		public async Task<CommandHandlerResponse> Revoke([Service] ISender sender, RevokeCommand input) => await sender.Send(input);

		public async Task<Profile> UpdateProfile([Service] ISender sender, UpdateProfilePicture input, CancellationToken cancellationToken)
		{
			var command = new UpdateProfileCommand
			{
				Id = input.Id,
				FirstName = input.FirstName,
				LastName = input.LastName,
				Phone = input.Phone,
			};
			if (input.File != null)
			{
				var imagePath = Path.Combine(PROFILE_IMAGES_PATH, $"{input.Id}.png");
				var urlPath = PROFILE_IMAGES_URL.FormatWith($"{input.Id}.png");
				using var stream = File.Create(imagePath);
				await input.File.CopyToAsync(stream, cancellationToken);
				command.ProfilePhotoUrl = urlPath;
			}
			return await sender.Send(command, cancellationToken);
		}

	}

	public class UpdateProfilePicture : UpdateProfileCommand
	{
		public IFile? File { get; set; }
	}

	public class UpdateProfileType : InputObjectType<UpdateProfilePicture>
	{
		protected override void Configure(
			IInputObjectTypeDescriptor<UpdateProfilePicture> descriptor)
		{
			descriptor.Field(f => f.Id).Type<UuidType>();
			descriptor.Field(f => f.FirstName).Type<StringType>();
			descriptor.Field(f => f.LastName).Type<StringType>();
			descriptor.Field(f => f.Phone).Type<StringType>();
			descriptor.Field(f => f.ProfilePhotoUrl).Ignore(true);
			descriptor.Field(f => f.File).Type<UploadType>();
		}
	}

	public class MutationType : ObjectType<Mutation>
	{
		protected override void Configure(
			IObjectTypeDescriptor<Mutation> descriptor)
		{
			descriptor.Field(f => f.SignUp(default!, default!, default!));
			descriptor.Field(f => f.SignIn(default!, default!));
			descriptor.Field(f => f.RefreshToken(default!, default!));
			descriptor.Field(f => f.Revoke(default!, default!));
			descriptor.Field(f => f.UpdateProfile(default!, default!, default!))
				.Argument("input", a => a.Type<UpdateProfileType>());
			
			
		}
	}

}
