using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;

namespace AspNetCore.Identity.Core.Commands;

public class DeleteAccountCommand : ICommand<CommandHandlerResponse>
{
	public Guid UserId { get; set; }

}
