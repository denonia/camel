using System.ComponentModel.DataAnnotations;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Camel.Web.Dtos;
using Camel.Web.Enums;
using Camel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IScoreService = Camel.Web.Services.Interfaces.IScoreService;

namespace Camel.Web.Pages.Users;

public class Index : PageModelBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScoreService _scoreService;
    private readonly IRankingService _rankingService;
    private readonly ICommentService _commentService;
    private readonly IRelationshipService _relationshipService;

    public Index(ApplicationDbContext dbContext, IScoreService scoreService, IRankingService rankingService,
        ICommentService commentService, IRelationshipService relationshipService)
    {
        _dbContext = dbContext;
        _scoreService = scoreService;
        _rankingService = rankingService;
        _commentService = commentService;
        _relationshipService = relationshipService;
    }

    public User? RequestedUser { get; set; }
    public Stats? Stats { get; set; }
    public Profile? Profile { get; set; }
    public IEnumerable<ProfileScore> Scores { get; set; }
    public IList<CommentDto> Comments { get; set; }
    public FriendType FriendType { get; set; }
    public int Rank { get; set; }

    [BindProperty] public int? CommentId { get; set; }
    [BindProperty] [MaxLength(300)] public string? Comment { get; set; }
    [BindProperty] public bool AddFriend { get; set; }
    [BindProperty] public bool RemoveFriend { get; set; }

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

        if (Authenticated)
        {
            FriendType = await _relationshipService.GetFriendTypeAsync(CurrentUserId, userId);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int userId)
    {
        if (!Authenticated)
            return Unauthorized();

        if (ModelState.IsValid && !string.IsNullOrEmpty(Comment))
        {
            var comment = new Comment
            {
                AuthorId = CurrentUserId,
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

            if (comment.AuthorId != CurrentUserId && comment.UserId != CurrentUserId)
                return Unauthorized();

            await _commentService.DeleteCommentAsync(comment.Id);
        }

        if (AddFriend)
            await _relationshipService.AddFriendAsync(CurrentUserId, userId);

        if (RemoveFriend)
            await _relationshipService.RemoveFriendAsync(CurrentUserId, userId);

        return RedirectToPage();
    }
}