public class ReportPhoneCommand : AsyncCommand<ReportPhoneCommand.Settings>
{
  public class Settings : ReportFraudSettings
  {
    [CommandArgument(0, "<NUMBER>")]
    public string Number { get; set; }

    [CommandArgument(1, "[COUNTRY]")]
    public string CountryCode { get; set; }
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ReportPhoneCommand.Settings settings)
  {
    throw new NotImplementedException();
  }
}
