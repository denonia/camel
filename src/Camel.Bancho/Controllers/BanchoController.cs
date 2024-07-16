using System.Text;
using Camel.Bancho.Packets;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

public class BanchoController : ControllerBase
{
    [HttpPost("/")]
    public IActionResult Index()
    {
        Response.Headers["cho-token"] = "login-failed";

        var ms = new MemoryStream();
        var ps = new PacketStream(ms);
        ps.WriteNotification("WHAT'S UP, PEOPLE?!");
        ps.WriteUserId(-1);

        return File(ms.ToArray(), "application/octet-stream");
    }

    [HttpGet("web/bancho_connect.php")]
    public IActionResult BanchoConnect() => Ok();
}