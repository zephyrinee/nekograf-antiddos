using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SecurityModule
{
    public class DDoSMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ProtectionService protectionService;
        
        public DDoSMiddleware(RequestDelegate next, ProtectionService service)
        {
            this.next = next;
            protectionService = service;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var allowed = protectionService.ProcessRequest(ipAddress);
                
                if (!allowed)
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("Too many requests");
                    return;
                }
            }
            
            await next(context);
        }
    }
}