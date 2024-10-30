using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using IPQSharp;
using IPQSharp.FileScanner;

public class ScanFileCommand : IPQSCommand<ScanFileCommand.Settings>
{
  public class Settings : IPQSSettings
  {
    [CommandArgument(0, "<FILE>")]
    [Description("The File to scan")]
    public string File { get; init; }

    [CommandOption("--full-scan")]
    [Description("Tells the scanner to do a full or lookup scan")]
    public bool DoFullScan { get; set; } = true;
  }

  public override async Task<int> ExecuteAsync
  (CommandContext context, ScanFileCommand.Settings settings)
  {
    var request = new FileScanRequest(settings.ApiKey)
    {
      DoFullScan = settings.DoFullScan
    };
    Console.WriteLine($"Scanning file for {settings.File}...");
    FileScanResult result = await request.SendAsync();
    WriteOutput<FileScanResult>(result);
    return (int)GetExitCodeFromResult(result);
  }
}
