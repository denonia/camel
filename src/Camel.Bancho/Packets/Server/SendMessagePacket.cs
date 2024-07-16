namespace Camel.Bancho.Packets.Server;

public struct SendMessagePacket : IWritePacket
{
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
    
    public void WriteToStream(IPacketStream stream)
    {
        using var ms = new MemoryStream();
        ms.WriteBanchoString(Sender);
        ms.WriteBanchoString(Message);
        ms.WriteBanchoString(Recipient);
        ms.Write(BitConverter.GetBytes(SenderId));
        
        var packet = new Packet(PacketType.ServerSendMessage, ms.ToArray());
        stream.Write(packet);
    }
}