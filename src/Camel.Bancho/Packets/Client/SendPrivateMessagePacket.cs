namespace Camel.Bancho.Packets.Client;

public readonly struct SendPrivateMessagePacket
{
    public string Sender { get; }
    public string Text { get; }
    public string Recipient { get; }
    public int SenderId { get; }

    public SendPrivateMessagePacket(string sender, string text, string recipient, int senderId)
    {
        Sender = sender;
        Text = text;
        Recipient = recipient;
        SenderId = senderId;
    }
    
    public static SendPrivateMessagePacket ReadFromStream(PacketBinaryReader reader)
    {
        return new SendPrivateMessagePacket(
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadInt32());
    }
}