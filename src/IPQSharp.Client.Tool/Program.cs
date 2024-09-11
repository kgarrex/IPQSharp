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
    config.AddCommand<DetectCommand>("detect")
      .WithAlias("proxy")
      .WithDescription("Get the overall fraud score")
      .WithExample("detect", "--ip", "0.0.0.0")
      .WithExample("detect", "--strictness", "[0-3]")
      ;

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

