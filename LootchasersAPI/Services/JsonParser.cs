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
    }
}
