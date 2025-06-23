namespace Sentyll.Domain.Common.Mappers.Pagination;

public static class PaginationResultMapperExtensions
{

    public static PaginationResult<TDestination> ToPaginationResult<TSource, TDestination>(
        this PaginationResult<TSource> paginatedResponse,
        List<TDestination> updatedData)
        => new()
        {
            PageSize = paginatedResponse.PageSize,
            PageNumber = paginatedResponse.PageNumber,
            NextPageNumber = paginatedResponse.NextPageNumber,
            Total = paginatedResponse.Total,
            Data = updatedData
        };
    
    public static PaginationResult<TDestination> ToPaginationResult<TSource, TDestination>(
        this PaginationResult<TSource> paginatedResponse,
        Func<TSource, TDestination> mapAction)
        => new()
        {
            PageSize = paginatedResponse.PageSize,
            PageNumber = paginatedResponse.PageNumber,
            NextPageNumber = paginatedResponse.NextPageNumber,
            Total = paginatedResponse.Total,
            Data = paginatedResponse.Data.Select(mapAction).ToList()
        };
    
}