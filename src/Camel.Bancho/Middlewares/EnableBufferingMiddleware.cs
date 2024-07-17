using Microsoft.AspNetCore.Http.Features;

namespace Camel.Bancho.Middlewares;

public class EnableBufferingMiddleware
{
    private readonly RequestDelegate _next;

    public EnableBufferingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<EnableBufferingAttribute>();
        if (attribute != null)
        {
            context.Request.EnableBuffering();
        }
 
        await _next(context);
    }
}