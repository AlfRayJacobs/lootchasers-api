using System.Text;

namespace LootchasersAPI.Services
{
    public static class MessageHandler
    {

        public static async Task SendMessageAsync(string webhookUrl, string message, HttpClient client)
        {
            var payload = new { content = message  };

            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(webhookUrl, content);
            Console.WriteLine($"Response from chat endpoint. {response.StatusCode.ToString()}");
        }

        public static async Task SendPollAsync(string webhookUrl, string question, string[] options, HttpClient client)
        {
            var embed = new
            {
                title = "Poll: " + question,
                description = string.Join("\n", options),
                color = 3447003
            };

            var payload = new
            {
                content = "Please vote below:",
                embeds = new[] { embed }
            };

            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using (client)
            {
                await client.PostAsync(webhookUrl, content);
            }
        }
    }
}
