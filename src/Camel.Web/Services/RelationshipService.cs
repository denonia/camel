using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Web.Dtos;
using Camel.Web.Enums;
using Camel.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Web.Services;

public class RelationshipService : IRelationshipService
{
    private readonly ApplicationDbContext _dbContext;

    public RelationshipService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<FriendDto>> GetFriendsAsync(int userId)
    {
        var friends = await _dbContext.Relationships
            .Where(r => r.FirstUserId == userId && r.Type == RelationshipType.Friend)
            .Select(r => new FriendDto
            {
                Id = r.SecondUserId,
                UserName = r.SecondUser.UserName!,
                Mutual = false
            }).ToListAsync();

        var addedByIds = await _dbContext.Relationships
            .Where(r => r.SecondUserId == userId && r.Type == RelationshipType.Friend)
            .Select(r => r.FirstUserId)
            .ToListAsync();

        foreach (var friend in friends.Where(friend => addedByIds.Contains(friend.Id)))
            friend.Mutual = true;

        return friends;
    }

    public async Task<FriendType> GetFriendTypeAsync(int userId, int friendId)
    {
        var friended =
            await _dbContext.Relationships.AnyAsync(r => r.FirstUserId == userId && r.SecondUserId == friendId);

        if (!friended)
            return FriendType.None;
        
        var friendedBy =
            await _dbContext.Relationships.AnyAsync(r => r.SecondUserId == userId && r.FirstUserId == friendId);

        return friendedBy ? FriendType.MutualFriend : FriendType.Friend;
    }

    public async Task AddFriendAsync(int userId, int friendId)
    {
        var relationship = new Relationship(userId, friendId, RelationshipType.Friend);
        _dbContext.Relationships.Add(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriendAsync(int userId, int friendId)
    {
        await _dbContext.Relationships
            .Where(r => r.FirstUserId == userId && r.SecondUserId == friendId)
            .ExecuteDeleteAsync();
    }
}