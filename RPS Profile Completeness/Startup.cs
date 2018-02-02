using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using RPS.Data.Elasticsearch;
using RPS.Domain.Data;
using RPS.Domain.Snakes;
using RPS.Presentation.Middleware;
using RPS.Presentation.Server.Data;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace RPS.Presentation
{
  public class Startup
  {

    public static void Main(string[] args)
    {


      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
        .Enrich.WithMachineName()
        .Enrich.WithMemoryUsage()
        .Enrich.WithEnvironment("OS")
        .Enrich.WithProperty(new KeyValuePair<string, object>("applicationId", "RPSProfile Completeness"))
   
        .Enrich.FromLogContext()
          //   .WriteTo
          //      .ApplicationInsightsEvents("<MyApplicationInsightsInstrumentationKey>")
        .WriteTo.RollingFile("logs/log-{Date}.txt", retainedFileCountLimit:10, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}{NewLine}Memory: {MemoryUsage}{NewLine}")
        .WriteTo.RollingFile(new JsonFormatter(), "logs/log-json-{Date}.txt")
        .WriteTo.Console()
        .CreateLogger();

      try
      {
        Log.Information("Starting web host");
        var host = new WebHostBuilder()
          .UseKestrel()
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseIISIntegration()
          .UseStartup<Startup>()
          .Build();

        host.Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddMvc();
      services.AddNodeServices();

      var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = "spa.db" };
      var connectionString = connectionStringBuilder.ToString();

      services.AddDbContext<SpaDbContext>(options =>
          options.UseSqlite(connectionString));

      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "RPS API", Version = "v1" });
      });

         services.Configure<ElasticSearchConfiguration>(Configuration.GetSection("SnakeDataRepository"));

      // services.AddScoped<ISearchProvider<>, ElasticSearchProvider>();
      
   //   services.AddScoped<ElasticSearchConfiguration>();
      services.AddScoped<IElasticSearchContext,ElasticSearchContext>();
   //   services.Add(ServiceDescriptor.Singleton<IElasticClient>(ElasticSearchContext.GetClient()));
      services.AddScoped<ISnakeDataRepository, SnakeDataRepository>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SpaDbContext context)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseStaticFiles();
      app.UseRemoteIpAddressLoggingMiddleware();
      app.UseMiddleware<HttpContextLoggingMiddleware>();
      app.UseMiddleware<UserLoggingMiddleware>();

      DbInitializer.Initialize(context);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
        {
          HotModuleReplacement = true,
          HotModuleReplacementEndpoint = "/dist/"
        });
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPS API V1");
        });

        // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.


        app.MapWhen(x => !x.Request.Path.Value.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase), builder =>
        {
          builder.UseMvc(routes =>
          {
            routes.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Home", action = "Index" });
          });
        });
      }
      else
      {
        app.UseMvc(routes =>
        {
          routes.MapRoute(
           name: "default",
           template: "{controller=Home}/{action=Index}/{id?}");

          routes.MapRoute(
           "Sitemap",
           "sitemap.xml",
           new { controller = "Home", action = "SitemapXml" });

          routes.MapSpaFallbackRoute(
            name: "spa-fallback",
            defaults: new { controller = "Home", action = "Index" });

        });
        app.UseExceptionHandler("/Home/Error");
      }
    }
  }
}
