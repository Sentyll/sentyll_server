using System.IO.Compression;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Failures;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Failures;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;

public static class SchedulerJobRequestExtensions
{
    
    private static readonly byte[] GZipSignature = [0x1f, 0x8b, 0x08, 0x00];

    public static Result<byte[]> CreateSchedulerJobRequest<T>(this T data)
        => Result
            .Success(JsonSerializer.Serialize(data))
            .Map((serializedData) =>
            {
                byte[] compressedBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    {
                        using var streamWriter = new StreamWriter(gzipStream);

                        streamWriter.Write(serializedData);
                    }
                    compressedBytes = memoryStream.ToArray();
                }

                return compressedBytes;
            })
            .Map((compressedBytes) => compressedBytes.Concat(GZipSignature).ToArray());

    public static Result<T> ReadSchedulerJobRequest<T>(this byte[] gzipBytes)
        => ReadSchedulerJobRequestAsString(gzipBytes)
            .Bind((stream) => JsonSerializer.Deserialize<T>(stream) ?? Result.Failure<T>(JsonFailures.Null));

    public static Result<string> ReadSchedulerJobRequestAsString(this byte[] gzipBytes)
        => Result
            .SuccessIf(
                gzipBytes.TakeLast(GZipSignature.Length).SequenceEqual(GZipSignature),
                SchedulerJobRequestFailures.NotGZipCompressed.ToString()
            )
            .Bind(() =>
            {
                var compressedBytes = gzipBytes
                    .Take(gzipBytes.Length - GZipSignature.Length)
                    .ToArray();

                using var memoryStream = new MemoryStream(compressedBytes);
                using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                using var streamReader = new StreamReader(gzipStream);

                return Result.Success(streamReader.ReadToEnd());
            });
}