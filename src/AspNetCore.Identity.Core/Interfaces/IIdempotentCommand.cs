namespace AspNetCore.Identity.Core.Interfaces;

public interface IIdempotentCommand<out TResponse> : ICommand<TResponse>
{
	Guid RequestId { get; set; }
}