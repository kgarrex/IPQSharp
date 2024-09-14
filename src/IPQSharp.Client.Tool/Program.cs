using System;
using Microsoft.Extensions.Hosting;
using DotNetEnv;
using Spectre.Console.Cli;

DotNetEnv.Env.Load();
var builder = Host.CreateApplicationBuilder(args);
//builder.Configuration.AddEnvironmentVariables();

var host = builder.Build();

host.Start();
{
  var app = new CommandApp();
  app.Configure(config =>
  {
    config.AddCommand<DetectProxyCommand>("detect")
      .WithAlias("proxy")
      .WithExample("detect", "--ip", "0.0.0.0")
      .WithExample("detect", "--strictness", "[0-3]")
      .WithDescription("Get the overall fraud score");

    config.AddBranch<ReportFraudSettings>("report", report =>
      {
        report.AddCommand<ReportEmailCommand>("email")
          .WithDescription("Report an email as fraudulent");

        report.AddCommand<ReportIpAddressCommand>("ip")
          .WithDescription("Report an ip address as fraudulent");

        report.AddCommand<ReportPhoneCommand>("phone")
          .WithDescription("Report a phone number as fraudulent");

        report.AddCommand<ReportRequestCommand>("request")
          .WithDescription("Report a previous request as fraudulent");
      })
      .WithAlias("fraud");

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

