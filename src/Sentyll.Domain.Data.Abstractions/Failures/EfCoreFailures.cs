namespace Sentyll.Domain.Data.Abstractions.Failures;

public static class EfCoreFailures
{
    private const string Code = "EFCOR";

    public static readonly Failure NotFound = new(Code, "0001", "The target entity was not found");
    public static readonly Failure NoChanges = new(Code, "0002", "No changes were detected for transaction");
    public static readonly Failure NoInsert = new(Code, "0003", "No changes were inserted");
    public static readonly Failure NoUpdate = new(Code, "0004", "No changes were updated");
    public static readonly Failure NoDelete = new(Code, "0005", "No changes were deleted");
}