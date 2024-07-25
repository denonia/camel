using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Packets;

public class PacketQueue
{
    private readonly Queue<Packet> _packetQueue = new();

    public PacketQueue()
    {
    }

    public bool Any() => _packetQueue.Count > 0;

    public IEnumerable<Packet> PendingPackets()
    {
        while (_packetQueue.Count > 0)
        {
            yield return _packetQueue.Dequeue();
        }
    }

    private void WritePacket(PacketType type, byte[] data) =>
        _packetQueue.Enqueue(new Packet(type, data));

    private void WritePacket(PacketType type) => WritePacket(type, Array.Empty<byte>());

    private void WritePacket(PacketType type, int num) =>
        _packetQueue.Enqueue(new Packet(type, BitConverter.GetBytes(num)));

    private void WritePacket(PacketType type, IPacketPayload payload)
    {
        using var ms = new MemoryStream();
        using var bw = new PacketBinaryWriter(ms);
        payload.WriteToStream(bw);
        _packetQueue.Enqueue(new Packet(type, ms.ToArray()));
    }

    private void WritePacket(PacketType type, string str)
    {
        using var ms = new MemoryStream();
        using var bw = new PacketBinaryWriter(ms);
        bw.Write(str);

        _packetQueue.Enqueue(new Packet(type, ms.ToArray()));
    }

    public void WriteNotification(string text) => WritePacket(PacketType.ServerNotification, text);

    public void WriteUserId(int id) => WritePacket(PacketType.ServerUserId, id);

    public void WriteProtocolVersion(int version) => WritePacket(PacketType.ServerProtocolVersion, version);

    public void WritePrivileges(Privileges privileges) => WritePacket(PacketType.ServerPrivileges, (int)privileges);

    public void WriteChannelInfo(ChannelInfo channelInfo) => WritePacket(PacketType.ServerChannelInfo, channelInfo);

    public void WriteChannelInfo(string name, string topic, int playerCount) =>
        WriteChannelInfo(new ChannelInfo(name, topic, playerCount));

    public void WriteChannelInfoEnd() => WritePacket(PacketType.ServerChannelInfoEnd);

    public void WriteChannelJoinSuccess(string channelName) =>
        WritePacket(PacketType.ServerChannelJoinSuccess, channelName);

    public void WriteRestart(int delayMs) => WritePacket(PacketType.ServerRestart, delayMs);

    public void WriteUserPresence(UserPresence userPresence) =>
        WritePacket(PacketType.ServerUserPresence, userPresence);

    public void WriteUserPresence(UserSession userSession, int rank) =>
        WriteUserPresence(new UserPresence(userSession.User.Id, userSession.User.UserName,
            (byte)userSession.UtcOffset, (byte)userSession.Location.CountryId, 0,
            (float)userSession.Location.Longitude, (float)userSession.Location.Latitude, rank));

    public void WriteUserStats(UserStats userStats) =>
        WritePacket(PacketType.ServerUserStats, userStats);

    public void WriteUserStats(UserSession userSession, int rank)
    {
        var stats = userSession.Stats.Single(s => s.Mode == userSession.Status.Mode);

        WriteUserStats(new UserStats(userSession.User.Id,
            userSession.Status.Action, userSession.Status.InfoText, userSession.Status.MapMd5, userSession.Status.Mods,
            stats.Mode, userSession.Status.MapId,
            stats.RankedScore, stats.Accuracy / 100.0f, stats.Plays,
            stats.TotalScore, rank, stats.Pp));
    }

    public void WriteSendMessage(Message message) => WritePacket(PacketType.ServerSendMessage, message);

    public void WriteSendMessage(string sender, string text, string recipient, int senderId) =>
        WritePacket(PacketType.ServerSendMessage, new Message(sender, text, recipient, senderId));

    public void WriteSpectatorJoined(int userId) => WritePacket(PacketType.ServerSpectatorJoined, userId);
    public void WriteSpectatorLeft(int userId) => WritePacket(PacketType.ServerSpectatorLeft, userId);
    public void WriteSpectatorCantSpectate(int userId) => WritePacket(PacketType.ServerSpectatorCantSpectate, userId);
    public void WriteFellowSpectatorJoined(int userId) => WritePacket(PacketType.ServerFellowSpectatorJoined, userId);
    public void WriteFellowSpectatorLeft(int userId) => WritePacket(PacketType.ServerFellowSpectatorLeft, userId);

    public void WriteSpectateFrames(ReplayFrameBundle frameBundle) =>
        WritePacket(PacketType.ServerSpectateFrames, frameBundle);

    public void WriteUserLogout(int userId) => WritePacket(PacketType.ServerUserLogout, userId);

    public void WriteGetAttention() => WritePacket(PacketType.ServerGetAttention);
    
    public void WriteNewMatch(MatchState state) => WritePacket(PacketType.ServerNewMatch, state);
    public void WriteMatchJoinSuccess(MatchState state) => WritePacket(PacketType.ServerMatchJoinSuccess, state);
    public void WriteMatchJoinFail() => WritePacket(PacketType.ServerMatchJoinFail);
    public void WriteUpdateMatch(MatchState state) => WritePacket(PacketType.ServerUpdateMatch, state);
}