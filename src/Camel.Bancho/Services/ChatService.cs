using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Services;

public class ChatService : IChatService
{
    private readonly Dictionary<string, ChatChannel> _channels = [];

    public ChatService()
    {
        _channels["#osu"] = new ChatChannel("#osu", "The primary channel");
        _channels["#test2"] = new ChatChannel("#test2", "Testing");
    }

    public IEnumerable<ChatChannel> AllChannels() => _channels.Values;
    
    public ChatChannel? GetChannel(string channelName)
    {
        return _channels.GetValueOrDefault(channelName);
    }

    public bool JoinChannel(string channelName, UserSession user)
    {
        return _channels.TryGetValue(channelName, out var channel) && channel.AddParticipant(user);
    }

    public bool LeaveChannel(string channelName, UserSession user)
    {
        return _channels.TryGetValue(channelName, out var channel) && channel.RemoveParticipant(user);
    }

    public bool SendMessage(string channelName, string message, UserSession user)
    {
        return _channels.TryGetValue(channelName, out var channel) && channel.SendMessage(message, user);
    }
}