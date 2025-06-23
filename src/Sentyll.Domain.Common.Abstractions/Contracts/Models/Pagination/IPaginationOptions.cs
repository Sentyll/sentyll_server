namespace Sentyll.Domain.Common.Abstractions.Contracts.Models.Pagination;

public interface IPaginationOptions
{
    int PageSize { get; set; }
    int PageNumber { get; set; }
    string OrderBy { get; set; }
    bool OrderAsc { get; set; }
}