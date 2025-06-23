namespace Sentyll.Domain.Common.Abstractions.Models.Results;

public struct PaginationResult<T>
{
    
    public int PageSize { get; set; }
    
    public int PageNumber { get; set; }
    
    public int? NextPageNumber { get; set; }
    
    public int Total { get; set; }
    
    public List<T> Data { get; set; }
    
}