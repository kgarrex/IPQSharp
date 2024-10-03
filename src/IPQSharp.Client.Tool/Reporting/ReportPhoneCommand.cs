using System.ComponentModel;
using IPQSharp;

public class ReportPhoneCommand : IPQSCommand<ReportPhoneCommand.Settings>
{
  public class Settings : ReportFraudSettings
  {
    [CommandArgument(0, "<NUMBER>")]
    [Description("The phone number to report. Must include calling code (ex: +1) if the country code argument is not provided")]
    public string Number { get; set; }

    [CommandArgument(1, "[COUNTRY]")]
    [Description("The 2-char country code of the phone number. Must be provided if number does not include calling code (ex: +1).")]
    public string CountryCode { get; set; }
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ReportPhoneCommand.Settings settings)
  {
    throw new NotImplementedException();
    var request = new FraudReportRequest(settings.ApiKey)
    {
    
    };
  }
}
