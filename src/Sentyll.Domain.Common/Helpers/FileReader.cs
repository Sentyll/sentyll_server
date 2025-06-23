using Sentyll.Domain.Common.Failures;

namespace Sentyll.Domain.Common.Helpers;

public static class FileReader
{

    public static Result<string> ReadFile(params string[] paths)
        => FormatPath(paths)
            .Ensure(filePath => File.Exists(filePath), FileReaderFailures.FileNotFound)
            .Map(filePath => File.ReadAllText(filePath));

    public static Result<List<string>> ReadFiles(params string[] paths)
        => FormatPath(paths)
            .Ensure(folderPath => Directory.Exists(folderPath), FileReaderFailures.DirectoryNotFound)
            .Map(folderPath => Directory.GetFiles(folderPath))
            .Ensure(files => files.Length != 0, FileReaderFailures.NoFilesInDirectory)
            .Map(files => files.Select(File.ReadAllText).ToList());

    private static Result<string> FormatPath(string[] paths)
        => Result
            .FailureIf(!paths.Any(), FileReaderFailures.FilePathEmpty)
            .Map(() => string.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, "Content\\", string.Join("\\", paths)));

}
