using Microsoft.AspNetCore.Mvc;

namespace Camel.WebApi.Controllers;

public class BanchoController : ControllerBase
{
    [HttpPost("/")]
    public async Task<IActionResult> Index()
    {
        var data = new byte[]
        {
            0x18, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0B, 0x0D, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x2C, 0x20, 0x77, 0x6F,
            0x72, 0x6C, 0x64, 0x21,
            
            0x05, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
        };
        Response.Headers["cho-token"] = "login-failed";

        return File(data, "application/octet-stream");
    }

    [HttpGet("web/bancho_connect.php")]
    public IActionResult BanchoConnect() => Ok();
}