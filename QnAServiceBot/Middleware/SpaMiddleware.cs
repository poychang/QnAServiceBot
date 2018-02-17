using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    /// <summary>SPA 中介程序的擴充方法</summary>
    public static class SpaExtensions
    {
        /// <summary>存取 SPA 網頁資源</summary>
        public static IApplicationBuilder UseSpaStaticFiles(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SpaMiddleware>();
        }
    }

    /// <summary>SPA 中介程序</summary>
    public class SpaMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>建構式</summary>
        public SpaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>任務調用</summary>
        /// <remarks>使用 index.html 存取網頁資源</remarks>
        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            if (context.Response.StatusCode == 404 &&                                                       // 該資源不存在
                !System.IO.Path.HasExtension(context.Request.Path.Value) &&                                 // 網址最後沒有帶副檔名
                !context.Request.Path.Value.StartsWith("/api", StringComparison.CurrentCultureIgnoreCase)   // 網址不是 /api 開頭
                )
            {
                context.Request.Path = "/index.html";   // 將網址改成 /index.html
                context.Response.StatusCode = 200;      // 並將 HTTP 狀態碼修改為 200 成功

                await _next.Invoke(context);
            }
        }
    }
}
