namespace WebApi.Core.Extensions;

public static class Extensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source,
        int pageSize, int pageNumber)
    {
        return new PaginatedResult<T>(pageNumber, pageSize).Paginate(source);
    }
}

public class PaginatedResult<T>
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 50;

    private int Total { get; set; }
    private int Limit { get; set; }
    private int Page { get; set; }

    internal PaginatedResult(int pageNumber, int pageSize = DefaultPageSize)
    {
        Limit = pageSize;
        Page = pageNumber;

        if (Limit is < 0 or > MaxPageSize)
        {
            Limit = DefaultPageSize;
        }
        if (pageNumber < 0)
        {
            Page = 0;
        }
    }

    internal IQueryable<T> Paginate(IQueryable<T> queryable)
    {
        Total = queryable.Count();

        if (Limit > Total)
        {
            Limit = Total;
            Page = 0;
        }

        var skip = Page * Limit;
        
        if (skip + Limit <= Total) return queryable.Skip(skip).Take(Limit);
        
        skip = Total - Limit;
        Page = Total / Limit - 1;

        return queryable.Skip(skip).Take(Limit);
    }
}