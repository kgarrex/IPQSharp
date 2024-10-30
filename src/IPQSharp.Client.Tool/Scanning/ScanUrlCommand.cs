using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using IPQSharp;
using IPQSharp.UrlScanner;

public class ScanUrlCommand : IPQSCommand<ScanUrlCommand.Settings>
{
  public class Settings : IPQSSettings
  {
    [CommandArgument(0, "<URL>")]
    [Description("The URL to scan")]
    public string Url { get; init; }
    
    [CommandOption("-s|--strictness"), Range(0,3)]
    public int Strictness { get; init; } = 0;

    [CommandOption("-f|--fast")]
    [Description("Whether or not to do a fast check")]
    public bool DoQuickCheck { get; init; } = false;

    [CommandOption("-t|--timeout")]
    [Description("The timeout for the scanner")]
    public int Timeout { get; init; } = 2;
  }

  public override async Task<int> ExecuteAsync
  (CommandContext context, ScanUrlCommand.Settings settings)
  {
    var request = new UrlScanRequest(settings.ApiKey, settings.Url)
    {
      Strictness = (StrictnessLevel)settings.Strictness,
      DoQuickCheck = settings.DoQuickCheck,
      Timeout = settings.Timeout
    };
    Console.WriteLine($"Scanning url for {settings.Url}...");
    UrlScanResult result = await request.SendAsync();
    WriteOutput<UrlScanResult>(result);
    return (int)GetExitCodeFromResult(result);
  }
}
