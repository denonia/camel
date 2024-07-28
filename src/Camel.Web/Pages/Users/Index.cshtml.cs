using System.ComponentModel.DataAnnotations;
using Azure;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Camel.Web.Dtos;
using Camel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IScoreService = Camel.Web.Services.Interfaces.IScoreService;

namespace Camel.Web.Pages.Users;

public class Index : PageModel
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScoreService _scoreService;
    private readonly IRankingService _rankingService;
    private readonly ICommentService _commentService;
    private readonly UserManager<User> _userManager;

    public Index(ApplicationDbContext dbContext, IScoreService scoreService, IRankingService rankingService,
        ICommentService commentService, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _scoreService = scoreService;
        _rankingService = rankingService;
        _commentService = commentService;
        _userManager = userManager;
    }

    public User? RequestedUser { get; set; }
    public Stats? Stats { get; set; }
    public Profile? Profile { get; set; }
    public IEnumerable<ProfileScore> Scores { get; set; }
    public IList<CommentDto> Comments { get; set; }
    public int Rank { get; set; }

    public string AvatarUrl(int id) => $"https://a.allein.xyz/{id}";

    [BindProperty] public int? CommentId { get; set; }
    [BindProperty] [MaxLength(300)] public string? Comment { get; set; }

    public async Task<IActionResult> OnGetAsync(int userId)
    {
        RequestedUser = await _dbContext.Users
            .Include(u => u.Stats)
            .Include(u => u.Profile)
            .SingleOrDefaultAsync(u => u.Id == userId);
        if (RequestedUser is null)
            return NotFound();

        Stats = RequestedUser.Stats.SingleOrDefault(s => s.Mode == GameMode.Standard);
        Scores = await _scoreService.GetUserBestScoresAsync(userId, GameMode.Standard);
        Comments = await _commentService.GetProfileCommentsAsync(userId);
        Rank = await _rankingService.GetGlobalRankPpAsync(userId, GameMode.Standard);
        Profile = RequestedUser.Profile;

        if (Profile is null)
        {
            Profile = new Profile { Id = RequestedUser.Id };
            _dbContext.Profiles.Add(Profile);
            await _dbContext.SaveChangesAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int userId)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Comment))
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            var comment = new Comment
            {
                AuthorId = user.Id,
                UserId = userId,
                Text = Comment
            };

            await _commentService.PostCommentAsync(comment);
        }

        if (CommentId is not null)
        {
            var comment = await _commentService.GetCommentAsync(CommentId.Value);
            if (comment is null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user is null || comment.AuthorId != user.Id && comment.UserId != user.Id)
                return Unauthorized();

            await _commentService.DeleteCommentAsync(comment.Id);
        }

        return RedirectToPage();
    }
}