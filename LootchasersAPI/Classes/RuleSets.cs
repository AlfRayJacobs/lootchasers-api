using LootchasersAPI.Interfaces;
using LootchasersAPI.Services;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LootchasersAPI.Classes.RuleSets
{
    public class LootRuleset : IRuleSet
    {
        private readonly int MIN_VALUE = 100_000;

        public bool ShouldSendNotification(string jsonContent) => GetItemCost(jsonContent) > MIN_VALUE;

        private long GetItemCost(string jsonContent)
        {
            var valueFromJson = JsonParser.GetNodeFromJsonEmbedValues(jsonContent, "Total Value");

            if (valueFromJson is null)
                return 0;

            return ValueConverter.ParseStackValue(valueFromJson);
        }        
    }
}
