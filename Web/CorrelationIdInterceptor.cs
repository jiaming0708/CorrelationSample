namespace Web;

public class CorrelationIdInterceptor(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 從當前的 HTTP Context 中取得 x-Correlation-id
        const string xCorrelationId = "x-correlation-id";
        var correlationId = httpContextAccessor.HttpContext?.Request.Headers[xCorrelationId].FirstOrDefault();

        // 如果不存在，則使用 TraceIdentifier
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = httpContextAccessor.HttpContext?.TraceIdentifier;
        }

        // 加入到 Request Header 中
        request.Headers.Add(xCorrelationId, correlationId);

        // 呼叫下一層處理邏輯
        return await base.SendAsync(request, cancellationToken);
    }
}