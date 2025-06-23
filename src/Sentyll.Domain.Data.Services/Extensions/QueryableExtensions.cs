namespace Sentyll.Domain.Data.Services.Extensions;

internal static class QueryableExtensions
{
    public static async Task<Result<PaginationResult<TSource>>> PaginateAsync<TSource>(
        this IQueryable<TSource> queryable,
        IPaginationOptions options,
        CancellationToken cancellationToken = default)
    {
        //TODO: ADD VALIDATION LOGIC HERE FOR PAGINATION OPTIONS.
        
        var paginatedResult = await queryable
            .Skip((options.PageNumber - 1) * options.PageSize)
            .Take(options.PageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var totalRecords = await queryable
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PaginationResult<TSource>()
        {
            PageSize = options.PageSize,
            PageNumber = options.PageNumber,
            Total = totalRecords,
            NextPageNumber = (totalRecords - options.PageNumber * options.PageSize) > 0 ? options.PageNumber + 1 : null,
            Data = paginatedResult
        };
    }
    
    public static IOrderedQueryable<TSource> Sort<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, object>> orderBy,
        bool isAsc = true
    ) => isAsc ? 
        source.OrderBy(orderBy) : 
        source.OrderByDescending(orderBy);

    public static IQueryable<TSource> WhereEnabled<TSource>(this IQueryable<TSource> source) 
        where TSource : ActivatableEntity
        => source.Where(activatableEntity => activatableEntity.IsEnabled);
    
}