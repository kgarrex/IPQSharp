using System.ComponentModel.DataAnnotations;
using IPQSharp;
using IPQSharp.PhoneValidation;

public class ValidatePhoneCommand : IPQSCommand<ValidatePhoneCommand.Settings>
{
  public class Settings : IPQSSettings
  {
    [CommandArgument(0, "<PHONE>")]
    public string? Phone { get; set; }

    [CommandOption("-c|--country")]
    public string? Country { get; set; }

    [CommandOption("--line-check")]
    public bool DoEnhancedLineCheck { get; set; } = false;

    [CommandOption("-s|--strictness")]
    public int? Strictness { get; set; }

    [CommandOption("--name-check")]
    public bool DoEnhancedNameCheck { get; set; } = false;
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ValidatePhoneCommand.Settings settings)
  {
    throw new NotImplementedException();
    var request = new PhoneValidationRequest(settings.ApiKey, settings.Phone)
    {
      Strictness = (StrictnessLevel)settings.Strictness,
      Country = settings.Country,
      DoEnhancedLineCheck = settings.DoEnhancedLineCheck,
      DoEnhancedNameCheck = settings.DoEnhancedNameCheck
    };
    Console.WriteLine($"Validating phone for {settings.Phone}...");
    PhoneValidationResult result = await request.SendAsync();
    WriteOutput<PhoneValidationResult>(result);
    return (int)GetExitCodeFromResult(result);
  }
}
