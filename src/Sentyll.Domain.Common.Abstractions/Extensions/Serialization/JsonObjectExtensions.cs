using System.Text.Json.Nodes;

namespace Sentyll.Domain.Common.Abstractions.Extensions.Serialization;

internal static class JsonObjectExtensions
{
    public static JsonObject ConvertToJsonObject(this IReadOnlyDictionary<string, object?> flatDict)
    {
        var root = new JsonObject();

        foreach (var (key, value) in flatDict)
        {
            var parts = key.Split('.');
            JsonNode current = root;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                // If it's the last part, assign the value
                if (i == parts.Length - 1)
                {
                    if (current is JsonObject obj)
                    {
                        obj[part] = value.ConvertToJsonNode();
                    }
                    else if (current is JsonArray arr && int.TryParse(part, out int arrIdx))
                    {
                        EnsureArraySize(arr, arrIdx);
                        arr[arrIdx] = value.ConvertToJsonNode();
                    }

                    continue;
                }

                // Handle next as array index
                if (int.TryParse(parts[i + 1], out int nextArrayIndex))
                {
                    if (current is JsonObject obj)
                    {
                        if (obj[part] is not JsonArray nextArr)
                        {
                            nextArr = new JsonArray();
                            obj[part] = nextArr;
                        }

                        current = nextArr;
                    }
                    else if (current is JsonArray arr && int.TryParse(part, out int currentIndex))
                    {
                        EnsureArraySize(arr, currentIndex);
                        if (arr[currentIndex] is not JsonArray nextArr)
                        {
                            nextArr = new JsonArray();
                            arr[currentIndex] = nextArr;
                        }

                        current = nextArr;
                    }

                    continue;
                }

                // Handle part as array index
                if (int.TryParse(part, out int index))
                {
                    if (current is JsonArray arr)
                    {
                        EnsureArraySize(arr, index);
                        if (arr[index] is not JsonObject obj)
                        {
                            obj = new JsonObject();
                            arr[index] = obj;
                        }

                        current = arr[index]!;
                    }

                    continue;
                }

                // Default: traverse or create object
                if (current is JsonObject currObj)
                {
                    if (currObj[part] is not JsonObject nextObj)
                    {
                        nextObj = new JsonObject();
                        currObj[part] = nextObj;
                    }

                    current = nextObj;
                }
            }
        }

        return root;
    }

    private static void EnsureArraySize(JsonArray array, int index)
    {
        while (array.Count <= index)
        {
            array.Add(null);
        }
    }
}