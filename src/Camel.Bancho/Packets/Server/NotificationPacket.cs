using System.Text;
using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct NotificationPacket : IPacket
{
    public PacketType Type => PacketType.ServerNotification;
    
    public string Text { get; }

    public NotificationPacket(string text)
    {
        Text = text;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Text);
    }
}