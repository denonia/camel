namespace Camel.Bancho.Packets;

public interface IPacketHandler
{
    void Handle(Stream stream);
}