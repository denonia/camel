using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct UserPresencePacket : IWritePacket
{
    public int Id { get; }
    public string Name { get; }
    public byte UtcOffset { get; }
    public byte CountryCode { get; }
    public byte BanchoPrivileges { get; }
    public float Longitude { get; }
    public float Latitude { get; }
    public int GlobalRank { get; }

    public UserPresencePacket(int id, string name, byte utcOffset, byte countryCode, byte banchoPrivileges,
        float longitude, float latitude, int globalRank)
    {
        Id = id;
        Name = name;
        UtcOffset = utcOffset;
        CountryCode = countryCode;
        BanchoPrivileges = banchoPrivileges;
        Longitude = longitude;
        Latitude = latitude;
        GlobalRank = globalRank;
    }

    public void WriteToStream(IPacketStream stream)
    {
        using var ms = new MemoryStream();
        
        ms.Write(Id);
        ms.WriteBanchoString(Name);
        ms.WriteByte(UtcOffset);
        ms.WriteByte(CountryCode);
        ms.WriteByte(BanchoPrivileges);
        ms.Write(Longitude);
        ms.Write(Latitude);
        ms.Write(GlobalRank);
        
        var packet = new Packet(PacketType.ServerUserPresence, ms.ToArray());
        stream.Write(packet);
    }
}