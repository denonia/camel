namespace Camel.Bancho.Packets.Client;

public readonly struct UserStatsRequestPacket
{
    public int[] UserIds { get; }
    
    public UserStatsRequestPacket(int[] userIds)
    {
        UserIds = userIds;
    }


    public static UserStatsRequestPacket ReadFromStream(PacketBinaryReader reader)
    {
        var length = reader.ReadInt16();

        var userIds = new int[length];
        for (var i = 0; i < length; i++)
            userIds[i] = reader.ReadInt32();
        
        return new UserStatsRequestPacket(userIds);
    }
}