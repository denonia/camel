namespace Camel.Bancho.Packets.Client;

public readonly struct UserStatsRequestPacket
{
    public int[] UserIds { get; }
    
    public UserStatsRequestPacket(int[] userIds)
    {
        UserIds = userIds;
    }


    public static UserStatsRequestPacket ReadFromStream(Stream stream)
    {
        var length = stream.ReadInt16();

        var userIds = new int[length];
        for (var i = 0; i < length; i++)
            userIds[i] = stream.ReadInt32();
        
        return new UserStatsRequestPacket(userIds);
    }
}