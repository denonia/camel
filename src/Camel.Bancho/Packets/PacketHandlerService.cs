using System.Reflection;

namespace Camel.Bancho.Packets;

public class PacketHandlerService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<PacketType, Type> _handlers = new();

    public PacketHandlerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IPacketHandler)))
            .Where(t => t.GetCustomAttributes(typeof(PacketHandlerAttribute)).Any());

        foreach (var handler in handlers)
        {
            var attr = (PacketHandlerAttribute)handler.GetCustomAttribute(typeof(PacketHandlerAttribute))!;
            _handlers[attr.Type] = handler;
        }
    }

    public void Handle(PacketType type, Stream stream)
    {
        if (_handlers.TryGetValue(type, out var handler))
        {
            var handlerInstance = (IPacketHandler)ActivatorUtilities.CreateInstance(_serviceProvider, handler);
            handlerInstance.Handle(stream);
        }
    }
}