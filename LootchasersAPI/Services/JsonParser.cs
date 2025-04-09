using System.Text.Json;
using static LootchasersAPI.Services.JsonParser;

namespace LootchasersAPI.Services
{
    public static class JsonParser
    {
        public record ClueItem(int Id, int Quantity, decimal PriceEach, string Name);

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

        public static List<ClueItem>? GetItemsFromJson(string jsonContent)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    if (doc.RootElement.TryGetProperty("extra", out JsonElement extraElement))
                    {
                        if (extraElement.TryGetProperty("items", out JsonElement itemsArray))
                        {
                            var itemsList = new List<ClueItem>();

                            foreach (JsonElement item in itemsArray.EnumerateArray())
                            {
                                int id = item.GetProperty("id").GetInt32();
                                int quantity = item.GetProperty("quantity").GetInt32();
                                decimal priceEach = item.GetProperty("priceEach").GetDecimal();
                                string name = item.GetProperty("name").GetString() ?? string.Empty;

                                var clueItem = new ClueItem(id, quantity, priceEach, name);
                                itemsList.Add(clueItem);
                            }
                            return itemsList;
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
