namespace Camel.Bancho.Models;

public class ChatChannel
{
    private readonly List<UserSession> _participants = [];

    private readonly string name;
    public string Name
    {
        get => name.StartsWith("#spectator") ? "#spectator" : name;
        private init => name = value;
    }

    public string Topic { get; }
    public bool AutoJoin { get; }

    public IEnumerable<UserSession> Participants => _participants;
    public int ParticipantCount => _participants.Count;

    public ChatChannel(string name, string topic, bool autoJoin)
    {
        Name = name;
        Topic = topic;
        AutoJoin = autoJoin;
    }

    public bool AddParticipant(UserSession user)
    {
        // TODO permissions or whatever
        _participants.Add(user);
        return true;
    }

    public bool RemoveParticipant(UserSession userSession)
    {
        return _participants.Remove(userSession);
    }

    public bool SendMessage(string message, UserSession sender)
    {
        if (!_participants.Contains(sender))
            return false;

        foreach (var participant in _participants)
        {
            if (participant != sender)
                participant.PacketQueue.WriteSendMessage(sender.Username, message, Name, sender.User.Id);
        }

        return true;
    }
}