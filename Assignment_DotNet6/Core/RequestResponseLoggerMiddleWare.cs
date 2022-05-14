using Newtonsoft.Json;
using System.Diagnostics;

namespace Assignment_DotNet6.Core
{
    public class RequestResponseLoggerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestResponseLoggerMiddleWare(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RequestResponseLoggerMiddleWare>();
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;
            Stopwatch watch = null;
            try
            {
                var newcontext = context;
                newcontext.Request.EnableBuffering();
                var requestText = await new StreamReader(newcontext.Request.Body).ReadToEndAsync();
               
                newcontext.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(requestText) && !requestText.ToLower().Contains("swagger") && !context.Request.Path.Value.ToLower().Contains("/swagger/"))
                    _logger.LogDebug($"Request: Endpoint:{context.Request.Path}, Request Body: {requestText}, Request remote IP Address :{context.Connection.RemoteIpAddress}");

                watch = Stopwatch.StartNew();
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;
                    await _next(context);
                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                    if (!string.IsNullOrEmpty(responseBody) && !responseBody.ToLower().Contains("swagger") && !context.Request.Path.Value.ToLower().Contains("/swagger/"))
                    {
                        if (context.Response.StatusCode == 200)
                            _logger.LogDebug($"Response: Endpoint:{context.Request.Path}, Response Body: {responseBody}, ResponseTime: {watch?.ElapsedMilliseconds}(ms)");
                        else
                            _logger.LogError($"Response: Endpoint:{context.Request.Path}, Error Body: {responseBody}, ResponseTime: {watch?.ElapsedMilliseconds}(ms)");

                    }

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"From Endpoint:{context.Request.Path}, Error: {ex.Message}, ResponseTime: {watch?.ElapsedMilliseconds}(ms)");
            }
            finally
            {
                watch?.Stop();
            }
        }
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            response.Body.Position = 0;
            return $"Response {text}";
        }
    }

    public static class RequestResponseLoggerMiddleWareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggerMiddleWare>();
        }
    }
}
