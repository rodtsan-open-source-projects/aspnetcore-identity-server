namespace AspNetCore.Identity.Infrastructure.ConfigSettings;

public class SqlCacheSettings
{
	public string? ConnectionString { get; set; }
	public string? SchemaName { get; set; }
	public string? TableName { get; set; }
}
