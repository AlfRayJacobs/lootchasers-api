using System.Text.RegularExpressions;

namespace LootchasersAPI.Services
{
    public static class ValueConverter
    {
        public static long ParseStackValue(string? input)
        {
            var regexMatches = Regex.Matches(input!, @"\d+[KMB.]?");
            input = regexMatches.Count > 0 ? string.Concat(regexMatches.Select(x => x.Value)) : "";

            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            input = input.Trim().ToUpperInvariant();

            if(input.EndsWith("B"))
            {
                if (double.TryParse(input.TrimEnd('B'), out var bVal))
                    return (long)(bVal * 1_000_000_000);
            }
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
