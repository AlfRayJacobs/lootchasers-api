using LootchasersAPI.Interfaces;
using System.Text.Json;

namespace LootchasersAPI.Classes.RuleSets
{
    public class LootRuleset : IRuleSet
    {
        private readonly int MIN_VALUE = 100000;

        public bool ShouldSendNotification(string jsonContent) => GetItemCost(jsonContent) > MIN_VALUE;

        private decimal? GetItemCost(string jsonContent)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    JsonElement itemsArray = doc.RootElement.GetProperty("extra").GetProperty("items");

                    if (itemsArray.GetArrayLength() > 0)
                    {
                        JsonElement item = itemsArray[0];

                        decimal priceEach = item.GetProperty("priceEach").GetDecimal();
                        int quantity = item.GetProperty("quantity").GetInt32();

                        decimal totalCost = priceEach * quantity;
                        return totalCost;
                    }
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }
    }
}
