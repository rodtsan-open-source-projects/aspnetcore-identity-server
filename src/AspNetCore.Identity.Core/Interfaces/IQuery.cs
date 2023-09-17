using MediatR;

namespace AspNetCore.Identity.Core.Interfaces;
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}