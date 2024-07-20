using Camel.Bancho.Models;

namespace Camel.Bancho.Services.Interfaces;

public interface IChatService
{
    IEnumerable<ChatChannel> AllChannels();
    ChatChannel? GetChannel(string channelName);
    bool JoinChannel(string channelName, UserSession user);
    bool LeaveChannel(string channelName, UserSession user);
    bool SendMessage(string channelName, string message, UserSession user);
}