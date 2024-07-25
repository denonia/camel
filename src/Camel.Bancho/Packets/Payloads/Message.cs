namespace Camel.Bancho.Packets.Payloads;

public readonly struct Message : IPacketPayload
{
    public string Sender { get; }
    public string Text { get; }
    public string Recipient { get; }
    public int SenderId { get; }

    public Message(string sender, string text, string recipient, int senderId)
    {
        Sender = sender;
        Text = text;
        Recipient = recipient;
        SenderId = senderId;
    }
    
    public static Message ReadFromStream(PacketBinaryReader reader)
    {
        return new Message(
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadInt32());
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Sender);
        writer.Write(Text);
        writer.Write(Recipient);
        writer.Write(SenderId);
    }
}