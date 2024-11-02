using System.ComponentModel;
using Spectre.Console.Cli;

public class IPQSSettings : CommandSettings
{
  [CommandOption("-k|--apikey <API-KEY>")]
  [Description("The IPQS Api Key")]
  public string ApiKey { get; set; }
}

public enum IPQSExitCode
{
  Success,
}
