using MediatR;

namespace AspNetCore.Identity.Core.Interfaces;
public interface ICommand<out TResponse> : IRequest<TResponse>
{
	
}
