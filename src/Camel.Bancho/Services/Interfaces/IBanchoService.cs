using Camel.Bancho.Packets;

namespace Camel.Bancho.Services.Interfaces;

public interface IBanchoService
{
    Task<string?> HandleLoginRequestAsync(PacketQueue pq, byte[] requestBytes, string ipAddress);
}