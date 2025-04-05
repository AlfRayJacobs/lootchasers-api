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

        public static string? GetNodeFromJsonEmbedValues(string jsonContent, string node)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    if (doc.RootElement.TryGetProperty("embeds", out JsonElement embeds))
                    {
                        foreach (var embed in embeds.EnumerateArray())
                        {
                            if (embed.TryGetProperty("fields", out JsonElement fields))
                            {
                                foreach (var field in fields.EnumerateArray())
                                {
                                    if (field.TryGetProperty("name", out JsonElement name) && name.GetString() == "Total Value")
                                    {
                                        if (field.TryGetProperty("value", out JsonElement value))
                                            return value.GetString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

    }
}
