using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("a.camel.local")]
public class AvatarController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AvatarController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet("/{userId}")]
    public IActionResult Avatar(int userId)
    {
        var dataDir = _configuration.GetRequiredSection("DATA_PATH").Value!;
        var path = Path.Combine(Path.GetFullPath(dataDir), "a", userId + ".jpg");

        if (!System.IO.File.Exists(path))
            return NotFound();

        var fs = new FileStream(path, FileMode.Open);
        
        return File(fs, "image/jpeg");
    }
}