using System.Text.Json;

namespace LootchasersAPI.Services
{
    public static class JsonParser
    {
        public static string? GetNodeFromJson(string jsonContent, string node)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    if (doc.RootElement.TryGetProperty(node, out JsonElement element))
                        return element.GetString();
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
        public static long ParseStackValue(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            input = input.Trim().ToUpperInvariant();

            if (input.EndsWith("M"))
            {
                if (double.TryParse(input.TrimEnd('M'), out var mVal))
                    return (long)(mVal * 1_000_000);
            }
            else if (input.EndsWith("K"))
            {
                if (double.TryParse(input.TrimEnd('K'), out var kVal))
                    return (long)(kVal * 1_000);
            }
            else if (long.TryParse(input.Replace(",", ""), out var plainVal))
            {
                return plainVal;
            }

            return 0;
        }
    }
}
