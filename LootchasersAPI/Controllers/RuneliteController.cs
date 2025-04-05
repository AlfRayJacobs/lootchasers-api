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

    public Dictionary<string, string> WebHooks = new(){
        { "LOOT", "https://discord.com/api/webhooks/1357558534207705099/PQkqeNWdFs-hvea1LDHvUHktIQDLixuNN-O5IF9tq1DDbW_zaWecMnAFyexxf8SknFZk" },
        { "COLLECTION", "https://discord.com/api/webhooks/1357559441880387764/dEetoH1yBjycE7rN4WLa66uDi0zBxpKjlLidprN_OoU0K8EwFNeP2_uhKz-A2Re78hBL" },
        { "CLUE", "https://discord.com/api/webhooks/1357559343326953622/RYTA2QkJSAcVGY3jieurkdbeCSUEhRwkuvQtYxsDugSuJGnw5LTiPvIY77uJCnGFKtmD" },
        { "PLAYER_KILL", "https://discord.com/api/webhooks/1357559093048512613/neNGF392D7AOcb2MOt4PJx-uG8uHAJAxSNoxQ_cugTQCjPcnw_aq_H58gsYJRdn9Y7Z_" },
        { "DEATH", "https://discord.com/api/webhooks/1357558802982899812/rDdt_j69xwhAEfa3S3j3C8pWRTAAv-eYOWFtLUk2OZSRGea0FpxOYO6pn8bWZm0bzL0N" },
        { "LEVEL", "https://discord.com/api/webhooks/1358118755108126732/dx1_agH0YIYPWP7rLRXmkOWDFP3wT5mwOC8SyO5LAxQErXeUcW85amHkYRqoSfJnXah2"}
    };

    [HttpPost(Name = "Runelite")]
    [DisableRequestTimeout]
    public async Task<IActionResult> Runelite([FromForm] IFormCollection form)
    {
        var payloadJson = form["payload_json"];
        Console.WriteLine($"Received JSON Payload: {payloadJson}");
        
        if (JsonParser.GetNodeFromJson(payloadJson!, "clanName") != "LootChasers")
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

        var type = JsonParser.GetNodeFromJson(payloadJson!, "type");
        if (!WebHooks.TryGetValue(type ?? "NONE", out var hookUrl))
            return BadRequest("Invalid webhook type");

        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.PostAsync(hookUrl, multipartContent);

        return Ok(new
        {
            Message = "Payload and file received successfully.",
            Payload = payloadJson,
            Response = response
        });
    }
}
