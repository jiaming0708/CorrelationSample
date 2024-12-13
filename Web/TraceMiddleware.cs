namespace Web;

public class TraceMiddleware
{
    private readonly RequestDelegate _next;

    public TraceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        const string xCorrelationId = "x-correlation-id";
        // 檢查是否已有追蹤 ID，若無則使用 TraceIdentifier
        if (!context.Request.Headers.TryGetValue(xCorrelationId, out var value))
        {
            value = context.TraceIdentifier;
            context.Request.Headers[xCorrelationId] = value;
        }

        context.Response.Headers[xCorrelationId] = value;

        await _next(context);
    }
}