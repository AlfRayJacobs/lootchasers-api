using LootchasersAPI.Classes.RuleSets;
using LootchasersAPI.Interfaces;
using LootchasersAPI.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace LootchasersAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RuneliteController : ControllerBase
{
    #pragma warning disable IDE1006 // Naming Styles
    private record Payload(string content);
    #pragma warning restore IDE1006 // Naming Styles
    private readonly IHttpClientFactory _httpClientFactory;
    
    public RuneliteController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private record Type (string name, string webhook, IRuleSet? rules);

    private List<Type> InputTypes = new()
    {
        new("LOOT", "https://discord.com/api/webhooks/1357558534207705099/PQkqeNWdFs-hvea1LDHvUHktIQDLixuNN-O5IF9tq1DDbW_zaWecMnAFyexxf8SknFZk", new LootRuleset()),
        new("COLLECTION", "https://discord.com/api/webhooks/1357559441880387764/dEetoH1yBjycE7rN4WLa66uDi0zBxpKjlLidprN_OoU0K8EwFNeP2_uhKz-A2Re78hBL", null),
        new("CLUE", "https://discord.com/api/webhooks/1357559343326953622/RYTA2QkJSAcVGY3jieurkdbeCSUEhRwkuvQtYxsDugSuJGnw5LTiPvIY77uJCnGFKtmD", new ClueRuleset()),
        new("PLAYER_KILL", "https://discord.com/api/webhooks/1357559093048512613/neNGF392D7AOcb2MOt4PJx-uG8uHAJAxSNoxQ_cugTQCjPcnw_aq_H58gsYJRdn9Y7Z", null),
        new("DEATH", "https://discord.com/api/webhooks/1357558802982899812/rDdt_j69xwhAEfa3S3j3C8pWRTAAv-eYOWFtLUk2OZSRGea0FpxOYO6pn8bWZm0bzL0N", null),
        new("LEVEL", "https://discord.com/api/webhooks/1358118755108126732/dx1_agH0YIYPWP7rLRXmkOWDFP3wT5mwOC8SyO5LAxQErXeUcW85amHkYRqoSfJnXah2", null),
        new("PET", "https://discord.com/api/webhooks/1358216916929478777/ZOEBzf7sXPn9tUXoy85qjy9FasyIJGjgPOSk5PwkorzyE2J0uLzVMhFUzEuH-urYEVkV", null),
        new("KILL_COUNT", "https://discord.com/api/webhooks/1358217056184569956/bgSw8Mc1Nm7OZIY9uMGFBWkmVw-NNr3TJOZQolzlnBMc2nNktvePNsvpPHhrMqxqfOwE", null),
        new("COMBAT_ACHIEVEMENT", "https://discord.com/api/webhooks/1358217142667055447/ysUDBFmAs336mSUx0egcOm-6K_tUyuxLNLKE-5Dr6KrcWc2cL1HJqC0WcCqYIGfhk3-0", null)
    };

    [HttpPost(Name = "Runelite")]
    [DisableRequestTimeout]
    public async Task<IActionResult> Runelite([FromForm] IFormCollection form)
    {
        var payloadJson = form["payload_json"];

        var type = JsonParser.GetNodeFromJson(payloadJson!, "type");
        var inputType = InputTypes.FirstOrDefault(x => x.name == type);
        var user = JsonParser.GetNodeFromJson(payloadJson!, "playerName");

        if (inputType is null)
        {
            Console.WriteLine($"Received webhook request from {user} with invalid type of {type}");
            return BadRequest("Invalid webhook type");
        }

        if (inputType.rules is not null && !inputType.rules.ShouldSendNotification(payloadJson!))
        {
            Console.WriteLine($"Received webhook request from {user} for type {type} which didn't pass ruleset");
            return BadRequest("Request did not pass the ruleset provided");
        }

        var clanName = JsonParser.GetNodeFromJson(payloadJson!, "clanName");
        if (clanName is not null && clanName != "LootChasers")
            return BadRequest("Invalid clan");

        var file = form.Files["file"];
        using var multipartContent = new MultipartFormDataContent();
        var jsonContent = new StringContent(payloadJson!, System.Text.Encoding.UTF8, "application/json");
        multipartContent.Add(jsonContent, "payload_json");

        using var stream = file!.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        multipartContent.Add(fileContent, "file", file.FileName);

        var imageByteArray = await fileContent.ReadAsByteArrayAsync();

        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.PostAsync(inputType.webhook, multipartContent);

        Console.WriteLine($"Successfully transmitted {type} request from {user}");

        return Ok(new
        {
            Message = "Payload and file received successfully.",
            Payload = payloadJson,
            Response = response
        });
    }
}
