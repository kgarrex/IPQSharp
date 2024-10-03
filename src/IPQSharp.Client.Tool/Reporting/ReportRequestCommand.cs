public class ReportRequestCommand : IPQSCommand<ReportRequestCommand.Settings>
{
  public class Settings : ReportFraudSettings
  {
    [CommandArgument(0, "<REQUEST_ID>")]
    public string? RequestId { get; set; }
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ReportRequestCommand.Settings settings)
  {
    throw new NotImplementedException();
  }
}
