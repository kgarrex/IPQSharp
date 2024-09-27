using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Json;

public abstract class IPQSCommand<TSettings> : AsyncCommand<TSettings> where TSettings : CommandSettings
{
  protected IPQSExitCode GetExitCodeFromMessage(string message)
  {
    return IPQSExitCode.Success;
  }

  protected void WriteOutput<T>(T result)
  {
    var jsonString = JsonSerializer.Serialize<T>(result);
    var json = new JsonText(jsonString);
    AnsiConsole.Write(
      new Panel(json)
        .Header("Result")
        .Collapse()
        .RoundedBorder()
        .BorderColor(Color.Yellow));
  }
}
