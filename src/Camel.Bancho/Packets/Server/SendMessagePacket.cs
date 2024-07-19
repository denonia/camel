using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public struct SendMessagePacket : IPacket
{
    public PacketType Type => PacketType.ServerSendMessage;
    
    public string Sender { get; }
    public string Message { get; }
    public string Recipient { get; }
    public int SenderId { get; }

    public SendMessagePacket(string sender, string message, string recipient, int senderId)
    {
        Sender = sender;
        Message = message;
        Recipient = recipient;
        SenderId = senderId;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Sender);
        writer.Write(Message);
        writer.Write(Recipient);
        writer.Write(SenderId);
    }
}