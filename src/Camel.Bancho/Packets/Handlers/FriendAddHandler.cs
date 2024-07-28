using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientFriendAdd)]
public class FriendAddHandler : IPacketHandler<int>
{
    private readonly IUserSessionService _userSessionService;
    private readonly ApplicationDbContext _dbContext;

    public FriendAddHandler(IUserSessionService userSessionService, ApplicationDbContext dbContext)
    {
        _userSessionService = userSessionService;
        _dbContext = dbContext;
    }
    
    public async Task HandleAsync(int userId, UserSession userSession)
    {
        var user = _userSessionService.GetOnlineUsers().SingleOrDefault(u => u.User.Id == userId);
        if (user is null)
            return;

        var relationship = _dbContext.Relationships
            .AsTracking()
            .SingleOrDefault(r => r.FirstUserId == userSession.User.Id && r.SecondUserId == userId);

        if (relationship is not null)
            relationship.Type = RelationshipType.Friend;
        else
        {
            var newRelationship = new Relationship(userSession.User.Id, userId, RelationshipType.Friend);
            await _dbContext.Relationships.AddAsync(newRelationship);
        }

        await _dbContext.SaveChangesAsync();
    }
}