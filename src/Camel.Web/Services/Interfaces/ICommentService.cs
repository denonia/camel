using Camel.Core.Entities;
using Camel.Web.Dtos;

namespace Camel.Web.Services.Interfaces;

public interface ICommentService
{
    Task<IList<CommentDto>> GetProfileCommentsAsync(int userId);
    Task<Comment?> GetCommentAsync(int commentId);
    Task PostCommentAsync(Comment comment);
    Task DeleteCommentAsync(int commentId);
}