using System;
using Microsoft.Extensions.Hosting;
using DotNetEnv;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Unsafe;

DotNetEnv.Env.Load();
var builder = Host.CreateApplicationBuilder(args);
//builder.Configuration.AddEnvironmentVariables();

var host = builder.Build();

host.Start();
{
  DotNetEnv.Env.Load();
  DotNetEnv.Env.TraversePath().Load();

  var app = new CommandApp();
  app.Configure(config =>
  {
    #if DEBUG
      config.PropagateExceptions();
      //config.ValidateExamples();
    #endif

    config.SetApplicationName("IPQS");
    config.UseAssemblyInformationalVersion();
    config.SetInterceptor(new CommandInterceptor());

    config.SetExceptionHandler((ex, resolver) =>
    {
      AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
      return -99;
    });

    config
      .AddCommand<DetectProxyCommand>("detect")
      .WithAlias("proxy")
      .WithExample("detect", "--ip", "0.0.0.0")
      .WithExample("detect", "--strictness", "[0-3]")
      .WithDescription("Get the overall fraud score for an IP address");


    config
      .SafetyOff()
      .AddBranch("report", typeof(ReportFraudSettings), reportBranch =>
      {
        reportBranch.AddCommand("email", typeof(ReportEmailCommand))
          .WithDescription("Report an email as fraudulent");

        reportBranch.AddCommand("ip", typeof(ReportIpAddressCommand))
          .WithDescription("Report an ip address as fraudulent");

        reportBranch.AddCommand("phone", typeof(ReportPhoneCommand))
          .WithDescription("Report a phone number as fraudulent");

        reportBranch.AddCommand("request", typeof(ReportRequestCommand))
          .WithDescription("Report a previous request as fraudulent");

        reportBranch.SetDescription("Report data points to an IPQS account");
      });


    config
      .SafetyOff()
      .AddBranch("validate", typeof(IPQSSettings), validateBranch =>
      {
        validateBranch.AddCommand("email", typeof(ValidateEmailCommand))
          .WithDescription("Validate an email address");

        validateBranch.AddCommand("phone", typeof(ValidatePhoneCommand))
          .WithDescription("Validate a phone number");

        validateBranch.SetDescription("Validate an email or phone number");
      });


    config
      .SafetyOff()
      .AddBranch("scan", typeof(IPQSSettings), scanBranch =>
      {
        scanBranch.AddCommand("file", typeof(ScanFileCommand))
          .WithExample("file", ".\\myfile.txt")
          .WithDescription("Scan a file in real time to detect viruses and malicious files");

        scanBranch.AddCommand("url", typeof(ScanUrlCommand))
          .WithExample("url", "http:\\url.com")
          .WithDescription("Scan urls in real time to detect suspicious urls.");

        scanBranch.SetDescription("Scan a file or url for malicious intent");
      });


    /*
    config.AddCommand<ManageAccountCommand>("account")
      .WithAlias("manage")
      .WithDescription("Manage an IPQualityScore account")
      ;
    */
  });
  app.Run(args);
}

host.StopAsync();

