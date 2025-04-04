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

    public RuneliteController()
    {
    }

    public Dictionary<string, string> WebHooks = new(){
        { "GRAND_EXCHANGE", "https://discord.com/api/webhooks/1357549790371516519/8VLSpBqi1ct65vbMVivbYfhhSGAon6N1rDV-Ch_zNZifrvn1HLBX2KAnJkinzYVbex0x"},
        { "DEATH", "https://discord.com/api/webhooks/1357550217423224874/AX0j26VI2R8K3SZdT9zrlc4idwJ1-x1reSZnFwVvhQzol-L0PWCJdmUEikIJTUc5mb3P" }
    };

    [HttpPost(Name = "Runelite")]
    [DisableRequestTimeout]
    public async Task<IActionResult> Runelite([FromForm] IFormCollection form)
    {
        var payloadJson = form["payload_json"];
        Console.WriteLine($"Received JSON Payload: {payloadJson}");

        var file = form.Files["file"];

       using var multipartContent = new MultipartFormDataContent();

        var jsonContent = new StringContent(payloadJson!, System.Text.Encoding.UTF8, "application/json");
        multipartContent.Add(jsonContent, "payload_json");

        using var stream = file!.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        multipartContent.Add(fileContent, "file", file.FileName);

        var imageByteArray = await fileContent.ReadAsByteArrayAsync();

        using var httpClient = new HttpClient();

        var type = JsonParser.GetNodeFromJson(payloadJson!, "type");
        if (!WebHooks.TryGetValue(type ?? "NONE", out var hookUrl))
            return BadRequest("Invalid webhook type");

        var response = await httpClient.PostAsync(hookUrl, multipartContent);

        return Ok(new
        {
            Message = "Payload and file received successfully.",
            Payload = payloadJson,
            Response = response
        });
    }
}
