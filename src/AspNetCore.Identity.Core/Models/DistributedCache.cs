namespace AspNetCore.Identity.Core.Models;

public class DistributedCache
{
	public required string Id { get; set; }
	public required byte[] Value { get; set; }
	public required DateTimeOffset ExpiresAtTime { get; set; }
	public long SlidingExpirationInSeconds { get; set; }
	public DateTimeOffset AbsoluteExpiration { get; set; }
}
