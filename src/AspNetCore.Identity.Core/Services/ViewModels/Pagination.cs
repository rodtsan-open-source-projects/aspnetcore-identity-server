namespace AspNetCore.Identity.Core.Services.ViewModels;

public class Pagination<T> : Pagination
{
    public virtual List<T>? Records { get; set; }
}

public class Pagination
{
    public virtual string Keywords { get; set; } = string.Empty;
    public virtual string OrderBy { get; set; } = string.Empty;
    public virtual int PageNumber { get; set; }
    public virtual int RecordCount { get; set; }
    public virtual int PageCount { get; set; }
    public virtual int PageSize { get; set; } = 10;
}

