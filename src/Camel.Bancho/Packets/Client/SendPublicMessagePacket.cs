namespace Camel.Bancho.Packets.Client;

public readonly struct SendPublicMessagePacket
{
    public string Sender { get; }
    public string Text { get; }
    public string Recipient { get; }
    public int SenderId { get; }

    public SendPublicMessagePacket(string sender, string text, string recipient, int senderId)
    {
        Sender = sender;
        Text = text;
        Recipient = recipient;
        SenderId = senderId;
    }
    
    public static SendPublicMessagePacket ReadFromStream(PacketBinaryReader reader)
    {
        return new SendPublicMessagePacket(
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadInt32());
    }
}