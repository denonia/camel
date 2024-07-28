using System.Security.Claims;
using Camel.Core.Data;
using Camel.Core.Enums;
using Camel.Web.Dtos;
using Camel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace Camel.Web.Pages;

[Authorize]
public class Friends : PageModel
{
    private readonly IRelationshipService _relationshipService;
    private readonly ApplicationDbContext _dbContext;

    public IList<FriendDto> FriendList { get; set; }

    public int FriendCount => FriendList.Count;
    public int MutualCount => FriendList.Count(f => f.Mutual);

    [BindProperty] [Required] public int FriendId { get; set; }

    public string AvatarUrl(int id) => $"https://a.allein.xyz/{id}";


    public Friends(IRelationshipService relationshipService)
    {
        _relationshipService = relationshipService;
    }

    public async Task OnGetAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        FriendList = await _relationshipService.GetFriendsAsync(userId);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (ModelState.IsValid)
            await _relationshipService.RemoveFriendAsync(userId, FriendId);
        
        return RedirectToPage();
    }
}