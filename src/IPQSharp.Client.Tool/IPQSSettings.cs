using Spectre.Console.Cli;

public class IPQSSettings : CommandSettings
{
  [CommandOption("-k|--apikey <API-KEY>")]
  public string ApiKey { get; init; }
}

public enum IPQSExitCode
{
  Success,
}
