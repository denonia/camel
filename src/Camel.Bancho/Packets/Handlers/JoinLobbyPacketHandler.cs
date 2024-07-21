using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientJoinLobby)]
public class JoinLobbyPacketHandler : IPacketHandler<JoinLobbyPacket>
{
    public async Task HandleAsync(JoinLobbyPacket packet, UserSession userSession)
    {
    }
}