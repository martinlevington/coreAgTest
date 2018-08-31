using Microsoft.AspNetCore.Builder;

namespace RPS.Presentation.Middleware
{
  public static class MiddlewareExtensions
  {
    public static IApplicationBuilder UseRemoteIpAddressLoggingMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<UserLoggingMiddleware>();
    }
  }
}
