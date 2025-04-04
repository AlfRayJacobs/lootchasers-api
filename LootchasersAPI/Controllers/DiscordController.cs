using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LootchasersAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscordController(HttpClient client, IHttpContextAccessor contextAccessor) : ControllerBase
{
    public readonly HttpClient _client = client;
    public readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    [HttpPost(Name = "Discord")]
    [DisableRequestTimeout]
    public IActionResult Discord()
    {
        _contextAccessor.HttpContext!.Response.Redirect("https://discord.com/invite/lootchasers");
        return Ok();
    }
}
