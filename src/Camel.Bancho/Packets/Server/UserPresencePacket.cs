﻿using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct UserPresencePacket : IPacket
{
    public PacketType Type => PacketType.ServerUserPresence;
    
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

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Name);
        writer.Write(UtcOffset);
        writer.Write(CountryCode);
        writer.Write(BanchoPrivileges);
        writer.Write(Longitude);
        writer.Write(Latitude);
        writer.Write(GlobalRank);
    }
}