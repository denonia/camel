using Camel.Bancho.Models;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Services;

public class ChatService : IChatService
{
    private readonly Dictionary<string, ChatChannel> _channels = [];

    public ChatService()
    {
        _channels["#osu"] = new ChatChannel("#osu", "The primary channel", true);
        _channels["#camel"] = new ChatChannel("#camel", "Camel", true);
        _channels["#mapping"] = new ChatChannel("#mapping", "For the mappers", true);
    }

    public IEnumerable<ChatChannel> AutoJoinChannels() => _channels.Values.Where(c => c.AutoJoin);

    public bool JoinChannel(string channelName, UserSession user)
    {
        if (_channels.TryGetValue(channelName, out var channel) && channel.AddParticipant(user))
        {
            user.PacketQueue.WritePacket(new ChannelJoinSuccessPacket(channel.Name));

            foreach (var participant in channel.Participants)
                participant.PacketQueue.WriteChannelInfo(channel.Name, channel.Topic, channel.ParticipantCount);
            
            return true;
        }
        
        return false;
    }

    public bool JoinSpectatorChannel(UserSession target, UserSession user)
    {
        var channelName = $"#spectator_{target.Username}";

        if (_channels.ContainsKey(channelName))
            return JoinChannel(channelName, user);
        
        var newChannel = new ChatChannel(channelName, $"{target.Username}'s spectator channel", false);
        _channels[channelName] = newChannel;
        JoinChannel(channelName, target);
        return JoinChannel(channelName, user);
    }

    public bool JoinMultiplayerChannel(Match match, UserSession user)
    {
        var channelName = $"#multiplayer_{match.Id}";
        if (_channels.ContainsKey(channelName))
            return JoinChannel(channelName, user);
        
        var newChannel = new ChatChannel(channelName, "Multi room channel", false);
        _channels[channelName] = newChannel;
        return JoinChannel(channelName, user);
    }

    public bool LeaveChannel(string channelName, UserSession user)
    {
        return _channels.TryGetValue(channelName, out var channel) && channel.RemoveParticipant(user);
    }

    public bool LeaveSpectatorChannel(UserSession target, UserSession user)
    {
        var channelName = $"#spectator_{target.Username}";
        return LeaveChannel(channelName, user);
    }

    public bool LeaveMultiplayerChannel(Match match, UserSession user)
    {
        var channelName = $"#multiplayer_{match.Id}";
        return LeaveChannel(channelName, user);
    }

    public bool SendMessage(string channelName, string message, UserSession user)
    {
        if (channelName == "#spectator")
        {
            var userName = user.Spectating?.Username ?? user.Username;
            channelName = $"#spectator_{userName}";
        }
        else if (channelName == "#multiplayer")
        {
            if (user.Match is null) 
                return false;
            channelName = $"#multiplayer_{user.Match.Id}";
        }

        return _channels.TryGetValue(channelName, out var channel) && channel.SendMessage(message, user);
    }
}