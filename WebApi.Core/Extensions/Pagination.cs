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
    private const int defaultPageSize = 20;
    private const int maxPageSize = 50;

    public int Total { get; private set; }
    public int Limit { get; private set; }
    public int Page { get; private set; }
    public IQueryable<T> Objects { get; private set; }

    internal PaginatedResult(int pageNumber, int pageSize = defaultPageSize)
    {
        Limit = pageSize;
        Page = pageNumber;

        if (Limit is < 0 or > maxPageSize)
        {
            Limit = defaultPageSize;
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
        if (skip + Limit > Total)
        {
            skip = Total - Limit;
            Page = Total / Limit - 1;
        }

        return queryable.Skip(skip).Take(Limit);
    }
}