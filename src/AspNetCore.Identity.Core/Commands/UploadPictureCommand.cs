using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Models;

namespace AspNetCore.Identity.Core.Commands;

public class UploadPictureCommand : ICommand<Profile>
{
    public Guid Id { get; set; }
    public string Path { get; set; } = string.Empty;
}