using Camel.Web.Dtos;
using Camel.Web.Enums;

namespace Camel.Web.Services.Interfaces;

public interface IRelationshipService
{
    Task<IList<FriendDto>> GetFriendsAsync(int userId);
    Task<FriendType> GetFriendTypeAsync(int userId, int friendId);
    Task AddFriendAsync(int userId, int friendId);
    Task RemoveFriendAsync(int userId, int friendId);
}