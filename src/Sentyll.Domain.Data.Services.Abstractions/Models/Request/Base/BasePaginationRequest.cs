using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sentyll.Domain.Data.Services.Abstractions.Models.Request.Base;

public abstract record BasePaginationRequest() : IPaginationOptions
{
    [Range(1, 100)] 
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = PagingDefaults.PageSize;
    
    [Range(1, uint.MaxValue)]
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = PagingDefaults.PageNumber;
    
    [JsonPropertyName("orderBy")]
    public abstract string OrderBy { get; set; }

    [JsonPropertyName("orderAsc")]
    public bool OrderAsc { get; set; } = PagingDefaults.DefaultSortDirection;
    
    [JsonPropertyName("searchText")]
    public string? SearchText { get; set; }
    
};