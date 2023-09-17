using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Core.Responses;

namespace AspNetCore.Identity.Core.Commands
{
    public class RevokeCommand : ICommand<CommandHandlerResponse>
    {
        public string UserName { get; set; } = string.Empty;
    }
}
