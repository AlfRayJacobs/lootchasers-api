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

            var regexMatches = Regex.Matches(valueFromJson!, @"\d+[KMB]?");
            return regexMatches.Count > 0 ? ParseStackValue(regexMatches.First().Value) : 0;
        }

        private long ParseStackValue(string? input)
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
