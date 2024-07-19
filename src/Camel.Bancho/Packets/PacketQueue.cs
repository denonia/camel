using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Server;
using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets;

public class PacketQueue
{
    private readonly Queue<IWritePacket> _packetQueue = new();

    public PacketQueue()
    {
    }

    public IEnumerable<IWritePacket> PendingPackets()
    {
        while (_packetQueue.Count > 0)
        {
            yield return _packetQueue.Dequeue();
        }
    }

    public void WritePacket(IWritePacket packet) =>
        _packetQueue.Enqueue(packet);

    public void WriteNotification(string text) =>
        _packetQueue.Enqueue(new NotificationPacket(text));

    public void WriteUserId(int id) =>
        _packetQueue.Enqueue(new UserIdPacket(id));

    public void WriteProtocolVersion(int version) =>
        _packetQueue.Enqueue(new ProtocolVersionPacket(version));

    public void WritePrivileges(int privileges) =>
        _packetQueue.Enqueue(new PrivilegesPacket(privileges));

    public void WriteChannelInfo(string name, string topic, int playerCount) =>
        _packetQueue.Enqueue(new ChannelInfoPacket(name, topic, playerCount));

    public void WriteChannelInfoEnd() =>
        _packetQueue.Enqueue(new ChannelInfoEndPacket());

    public void WriteRestart(int delayMs) =>
        _packetQueue.Enqueue(new RestartPacket(delayMs));

    public void WriteUserPresence(int id, string name, byte utcOffset, byte countryCode, byte banchoPrivileges,
        float longitude, float latitude, int globalRank) =>
        _packetQueue.Enqueue(new UserPresencePacket(id, name, utcOffset, countryCode, banchoPrivileges,
            longitude, latitude, globalRank));

    public void WriteUserPresence(UserSession userSession, int rank) =>
        WriteUserPresence(userSession.User.Id, userSession.User.UserName,
            (byte)userSession.UtcOffset, 222, 0, 0, 0, rank);

    public void WriteUserStats(int id, ClientAction action, string infoText, string mapMd5, int mods, GameMode mode,
        int mapId,
        long rankedScore, float accuracy, int plays, long totalScore, int rank, short pp) =>
        _packetQueue.Enqueue(new UserStatsPacket(id, action, infoText, mapMd5, mods, mode, mapId,
            rankedScore, accuracy, plays, totalScore, rank, pp));

    public void WriteUserStats(UserSession userSession, int rank)
    {
        var stats = userSession.User.Stats.Single(s => s.Mode == userSession.Status.Mode);

        WriteUserStats(userSession.User.Id,
            userSession.Status.Action, userSession.Status.InfoText, userSession.Status.MapMd5, userSession.Status.Mods,
            stats.Mode, userSession.Status.MapId,
            stats.RankedScore, stats.Accuracy / 100.0f, stats.Plays,
            stats.TotalScore, rank, stats.Pp);
    }

    public void WriteSendMessage(string sender, string message, string recipient, int senderId) =>
        _packetQueue.Enqueue(new SendMessagePacket(sender, message, recipient, senderId));
}