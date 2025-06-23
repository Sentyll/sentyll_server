namespace Sentyll.Domain.Data.Services.Abstractions.Failures;

public static class PaginationFailures
{
    private const string Code = "PAGIN";
    
    public static readonly Failure UsePagination = new Failure(Code, "0001", "Unsafe collection size requested, refactor query to include pagination");
}