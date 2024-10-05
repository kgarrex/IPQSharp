using System.ComponentModel.DataAnnotations;
using IPQSharp;
using IPQSharp.EmailValidation;

public class ValidateEmailCommand : IPQSCommand<ValidateEmailCommand.Settings>
{
  public class Settings : IPQSSettings
  {
    [CommandArgument(0, "<EMAIL>")]
    public string Email { get; set; }

    [CommandOption("-f|--fast")]
    public bool DoFastLookup { get; set; } = false;

    [CommandOption("-t|--timeout")]
    public int Timeout { get; set; } = 7;

    [CommandOption("--domain")]
    public bool DoSuggestDomain { get; set; } = false;

    [CommandOption("-s|--strictness"), Range(0,3)]
    public int Strictness { get; init; } = 0;

    [CommandOption("-a|--abuse-strictness")]
    public int AbuseStrictness { get; init; } = 0;
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ValidateEmailCommand.Settings settings)
  {
    throw new NotImplementedException();
    var request = new EmailValidationRequest(settings.ApiKey, settings.Email)
    {
      Timeout = settings.Timeout,
      Strictness = (StrictnessLevel)settings.Strictness,
      AbuseStrictness = (AbuseStrictnessLevel)settings.AbuseStrictness,
      DoFastLookup = settings.DoFastLookup,
      DoSuggestDomain = settings.DoSuggestDomain
    };
    Console.WriteLine($"Validating email for {settings.Email}...");
    EmailValidationResult result = await request.SendAsync();
    WriteOutput<EmailValidationResult>(result);
    return (int)GetExitCodeFromResult(result);
  }
}
