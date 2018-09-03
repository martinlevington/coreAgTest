using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace RPS.Presentation.Middleware
{
public class UserLoggingMiddleware
  {
  private readonly RequestDelegate _next;

  public UserLoggingMiddleware(RequestDelegate next)
  {
    _next = next;
  }




    public async Task Invoke(HttpContext context)
    {
      var userName = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "unknown";

      using (LogContext.PushProperty("User", !String.IsNullOrWhiteSpace(userName) ? userName : "unknown"))
      {
        await _next.Invoke(context);
      }
    }

  }
}
