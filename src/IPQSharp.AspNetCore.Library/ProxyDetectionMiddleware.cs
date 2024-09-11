using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using IPQSharp;

namespace IPQSharp.AspNetCore;

public class IPQSOptions
{
  public string ApiKey { get; set; }
  public bool Enabled { get; set; } = false;
}

public class ProxyDetectionOptions : IPQSOptions
{
  public static readonly string SectionName = "ProxyDetector";
  public int Strictness { get; set; }
  public bool DoFastLookup { get; set; }
  public bool IsMobileDevice { get; set; } = false;
  public bool AllowPublicAccessPoints { get; set; } = true;
  public bool HaveLighterPenalties { get; set; }
  public int TransactionStrictness { get; set; }
}


public partial class ProxyDetectionMiddleware : IMiddleware
{
  private readonly ILogger<ProxyDetectionMiddleware> _logger;
  private readonly ProxyDetectionOptions _options;
  private readonly IHttpClientFactory _httpFactory;

  public ProxyDetectionMiddleware(
    ILogger<ProxyDetectionMiddleware> logger,
    ProxyDetectionOptions options,
    IHttpClientFactory httpFactory)
  {
    _logger = logger;
    _options = options;
    _httpFactory = httpFactory;
  }

  [LoggerMessage(
    EventId = 0,
    Level   = LogLevel.Debug,
    Message = "Form Content: {contentString}")]
  private partial void LogTestContent(string contentString);


  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    if(!_options.Enabled)
    {
      await next.Invoke(context);
      return;
    }
    var ip = context.Connection.RemoteIpAddress;
    var userAgent =
      !StringValues.IsNullOrEmpty(context.Request.Headers.UserAgent) ?
      context.Request.Headers.UserAgent[0] : null;
    var userLanguage =
      !StringValues.IsNullOrEmpty(context.Request.Headers.ContentLanguage) ?
      context.Request.Headers.ContentLanguage[0] : null;

    var request = new ProxyDetectionRequest(_options.ApiKey, ip)
      .WithStrictnessLevelLow()
      .WithFastLookup()
      .WithUserAgent(userAgent)
      .WithUserLanguage(userLanguage);
    var result = await request.SendAsync();

    // if(proxyDetected)
    // {
    // TODO logic to return 403 forbidden
    // }
    await next.Invoke(context);
  }
}

public static partial class Extensions
{
  public static void AddProxyDetection(this IServiceCollection services)
  {
    services.AddHttpClient("ProxyDetection", http =>
    {
      http.BaseAddress = new Uri("https://www.ipqualityscore.com/api/json/ip/");
      http.Timeout = TimeSpan.FromMinutes(2);
      //_http.DefaultRequestHeaders.Add("IPS-KEY", _ipqsKey);
    });
    services.AddTransient<ProxyDetectionMiddleware>(provider =>
    {
      var logger = provider.GetRequiredService<ILogger<ProxyDetectionMiddleware>>();
      var httpFactory = provider.GetRequiredService<IHttpClientFactory>();
      var options = provider
        .GetRequiredService<IConfiguration>()
        .GetSection(ProxyDetectionOptions.SectionName)
        .Get<ProxyDetectionOptions>();
      return new ProxyDetectionMiddleware(logger: logger, options: options, httpFactory: httpFactory);
    });
  }

  public static void AddProxyDetection(this IServiceCollection services, IConfiguration section)
  {
    services.AddHttpClient("ProxyDetection", http =>
    {
      http.BaseAddress = new Uri("https://www.ipqualityscore.com/api/json/ip/");
      http.Timeout = TimeSpan.FromMinutes(2);
      //_http.DefaultRequestHeaders.Add("IPS-KEY", _ipqsKey);
    });
    services.AddOptions<ProxyDetectionOptions>()
      .Bind(section)
      .ValidateDataAnnotations()
      .ValidateOnStart();
    services.AddTransient<ProxyDetectionMiddleware>(provider =>
    {
      var logger = provider.GetRequiredService<ILogger<ProxyDetectionMiddleware>>();
      var httpFactory = provider.GetRequiredService<IHttpClientFactory>();
      var options = provider.GetRequiredService<IOptions<ProxyDetectionOptions>>().Value;
      return new ProxyDetectionMiddleware(logger: logger, options: options, httpFactory: httpFactory);
    });
  }

  public static IApplicationBuilder UseProxyDetection(this IApplicationBuilder app) =>
    app.UseMiddleware<ProxyDetectionMiddleware>();
}

public class IPQSService
{
  private readonly HttpClient _http;

  /*
  public IPQSService(HttpClient http)
  {
    _http = http;
    _http.BaseAddress = new Uri("https://www.ipqualityscore.com/api/json/ip/"),
  }

  public 
  */
}
