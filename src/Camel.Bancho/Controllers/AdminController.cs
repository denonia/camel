using Camel.Bancho.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("osu.ppy.sh", "osu.camel.local")]
public class AdminController : ControllerBase
{
    private readonly IUserSessionService _userSessionService;

    public AdminController(IUserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
    }

    [HttpGet("/sessions")]
    public IActionResult ActiveSessions()
    {
        return Ok(_userSessionService.GetOnlineUsers());
    }
}