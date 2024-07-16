using System.Text;
using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct NotificationPacket : IWritePacket
{
    public string Text { get; }

    public NotificationPacket(string text)
    {
        Text = text;
    }

    public void WriteToStream(IPacketStream stream)
    {
        using var ms = new MemoryStream();
        ms.WriteBanchoString(Text);

        var packet = new Packet(PacketType.ServerNotification, ms.ToArray());
        stream.Write(packet);
    }
}