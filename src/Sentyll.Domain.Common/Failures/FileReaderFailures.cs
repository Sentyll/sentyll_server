using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Domain.Common.Failures;

public static class FileReaderFailures
{
    private const string Code = "FLRDR";
    
    public static readonly Failure FilePathEmpty = new(Code, "0001","The file path provided cannot be empty");
    public static readonly Failure FileNotFound = new(Code, "0002","The requested file was not found or registered properly");
    public static readonly Failure DirectoryNotFound = new(Code, "0003","The requested Folder was not found or registered properly");
    public static readonly Failure NoFilesInDirectory = new(Code, "0004","No Files were found in the requested folder");
}