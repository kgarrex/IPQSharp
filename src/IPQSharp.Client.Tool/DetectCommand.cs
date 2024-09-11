using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Spectre.Console.Cli;

using IPQSharp.Internal;
using IPQSharp;

public class IPQSSettings : CommandSettings
{
  [CommandOption("-k|--apikey <API-KEY>")]
  public string ApiKey { get; init; }
}

public class DetectCommand : AsyncCommand<DetectCommand.Settings>
{
  public class Settings : IPQSSettings
  {
    [CommandArgument(0, "<IP>")] 
    [Description("An IP address to detect")]
    public string IpAddress { get; init; }

    [CommandOption("-s|--strictness"), Range(0,3)]
    public int Strictness { get; init; } = 0;

    [CommandOption("-a|--user-agent")]
    public string? UserAgent { get; init;}

    [CommandOption("-l|--language")]
    public string? UserLanguage { get; init;}

    [CommandOption("-f|--fast")]
    public bool DoFastLookup { get; init; } = false;

    [CommandOption("-m|--mobile")]
    public bool IsMobileDevice { get; init; } = false;

    [CommandOption("-p|--public")]
    public bool AllowPublicAccessPoints { get; init; } = true;

    [CommandOption("-n|--light-penalties")]
    public bool HaveLighterPenalties { get; init; } = true;

    [CommandOption("-t|--transaction-strictness"), Range(0, 2)]
    public int TransactionStrictness { get; init; } = 0;
  }

  public override async Task<int> ExecuteAsync
  (CommandContext context, DetectCommand.Settings settings)
  {
    Console.WriteLine(settings.IpAddress);
    return 0;
    var request = new ProxyDetectionRequest(settings.ApiKey, settings.IpAddress)
    {
      Strictness = (StrictnessLevel)settings.Strictness,
      UserAgent = settings.UserAgent,
      UserLanguage = settings.UserLanguage,
      DoFastLookup = settings.DoFastLookup,
      IsMobileDevice = settings.IsMobileDevice,
      AllowPublicAccessPoints = settings.AllowPublicAccessPoints,
      HaveLighterPenalties = settings.HaveLighterPenalties,
      TransactionStrictness = (TransactionStrictnessLevel)settings.TransactionStrictness,
    };
    var result = await request.SendAsync();
    return 0;
  }
}
