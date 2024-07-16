﻿using System.Reflection;
using Camel.Bancho.Enums;
using Camel.Bancho.Models;

namespace Camel.Bancho.Packets;

public class PacketHandlerService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PacketHandlerService> _logger;
    private readonly Dictionary<PacketType, Type> _handlers = new();

    public PacketHandlerService(IServiceProvider serviceProvider, ILogger<PacketHandlerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        var handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPacketHandler<>)))
            .Where(t => t.GetCustomAttributes(typeof(PacketHandlerAttribute)).Any());

        foreach (var handler in handlers)
        {
            var attr = (PacketHandlerAttribute)handler.GetCustomAttribute(typeof(PacketHandlerAttribute))!;
            _handlers[attr.Type] = handler;
        }
    }

    public void Handle(PacketType type, Stream stream, UserSession userSession)
    {
        if (type == PacketType.ClientPing)
            return;
        
        if (_handlers.TryGetValue(type, out var handler))
        {
            var packetType = handler.GetInterfaces()
                .Single(i => i.GetGenericTypeDefinition() == typeof(IPacketHandler<>))
                .GetGenericArguments().Single();

            // TODO: ensure ReadFromStream exists
            var packet = packetType.GetMethod("ReadFromStream").Invoke(null, [stream]);
            
            var handlerInstance = ActivatorUtilities.CreateInstance(_serviceProvider, handler);
            handler.GetMethod("Handle")!.Invoke(handlerInstance, [packet, userSession]);
        }
        else
        {
            _logger.LogWarning("Missing packet handler for {}", type);
        }
    }
}