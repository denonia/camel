using Camel.Bancho.Models;

namespace Camel.Bancho.Services.Interfaces;

public interface IChatService
{
    IEnumerable<ChatChannel> AutoJoinChannels();
    bool JoinChannel(string channelName, UserSession user);
    bool JoinSpectatorChannel(UserSession target, UserSession user);
    bool LeaveChannel(string channelName, UserSession user);
    bool LeaveSpectatorChannel(UserSession target, UserSession user);
    bool SendMessage(string channelName, string message, UserSession user);
}