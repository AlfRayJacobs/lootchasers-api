using LootchasersAPI.Interfaces;
using LootchasersAPI.Services;

namespace LootchasersAPI.Classes.RuleSets
{
    public class LootRuleset : IRuleSet
    {
        private readonly int MIN_VALUE = 1_000_000;

        public bool ShouldSendNotification(string jsonContent) => GetItemCost(jsonContent) > MIN_VALUE;

        private long GetItemCost(string jsonContent)
        {
            var valueFromJson = JsonParser.GetNodeFromJsonEmbedValues(jsonContent, "Total Value");

            if (valueFromJson is null)
                return 0;

            return ValueConverter.ParseStackValue(valueFromJson);
        }        
    }

    public class ClueRuleset : IRuleSet
    {
        private readonly int MIN_VALUE = 1_000_000;

        public bool ShouldSendNotification(string jsonContent) => GetClueValue(jsonContent) > MIN_VALUE;

        private long GetClueValue(string jsonContent)
        {
            var items = JsonParser.GetItemsFromJson(jsonContent);
            return items is null ? 0 : (long)items.Sum(x => x.Quantity * x.PriceEach);
        }
    }

    public class ChatRuleset : IRuleSet
    {
        private readonly List<string> AllowedUsername = new()
        {
            "D M Fhe",
            "AtomicEscape"
        };

        public bool ShouldSendNotification(string jsonContent)
        {
            var user = JsonParser.GetNodeFromJsonExtraValues(jsonContent, "source");
            var chatType = JsonParser.GetNodeFromJsonExtraValues(jsonContent, "type");
            var chatMessage = JsonParser.GetNodeFromJsonExtraValues(jsonContent, "message");

            Console.WriteLine("Chat message received");

            if (chatMessage is null || !chatMessage.Contains("Event"))
                return false;

            if (user is null || chatType != "PUBLICCHAT")
            {
                Console.WriteLine($"Chat message failed due to user ({user}) or chat type ({chatType})");
                return false;
            }

            Console.WriteLine("Chat message should send");
            return AllowedUsername.Contains(user);
        }
    }
}
