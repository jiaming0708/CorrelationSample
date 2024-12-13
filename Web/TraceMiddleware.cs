namespace Web;

public class TraceMiddleware(RequestDelegate next)
{
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

        await next(context);
    }
}