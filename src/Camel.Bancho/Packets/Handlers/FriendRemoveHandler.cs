using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientFriendRemove)]
public class FriendRemoveHandler : IPacketHandler<int>
{
    private readonly UserSessionService _userSessionService;
    private readonly ApplicationDbContext _dbContext;

    public FriendRemoveHandler(UserSessionService userSessionService, ApplicationDbContext dbContext)
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

        if (relationship is not null && relationship.Type == RelationshipType.Friend)
        {
            _dbContext.Relationships.Remove(relationship);
            await _dbContext.SaveChangesAsync();
        }
    }
}