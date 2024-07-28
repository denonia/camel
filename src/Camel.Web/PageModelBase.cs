using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Camel.Web;

public class PageModelBase : PageModel
{
    public bool Authenticated => User.Identity?.IsAuthenticated ?? false;
    public int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "-1");
        
    public string AvatarUrl(int id) => $"https://a.allein.xyz/{id}";
}