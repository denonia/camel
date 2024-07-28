using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Web.Dtos;
using Camel.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Web.Services;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _dbContext;

    public CommentService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<CommentDto>> GetProfileCommentsAsync(int userId)
    {
        return await _dbContext.Comments
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.PostedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                AuthorName = c.Author.UserName!,
                AuthorId = c.AuthorId,
                Text = c.Text,
                PostedAt = c.PostedAt
            })
            .ToListAsync();
    }

    public async Task<Comment?> GetCommentAsync(int commentId)
    {
        return await _dbContext.Comments.SingleOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task PostCommentAsync(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        await _dbContext.Comments
            .Where(c => c.Id == commentId)
            .ExecuteDeleteAsync();
    }
}