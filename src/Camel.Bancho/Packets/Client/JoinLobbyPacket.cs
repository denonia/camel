namespace Camel.Bancho.Packets.Client;

public readonly struct JoinLobbyPacket
{
    public static JoinLobbyPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new JoinLobbyPacket();
    }
}