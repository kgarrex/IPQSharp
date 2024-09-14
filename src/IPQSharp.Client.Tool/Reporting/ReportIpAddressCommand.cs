public class ReportIpAddressCommand : AsyncCommand<ReportIpAddressCommand.Settings>
{
  public class Settings : ReportFraudSettings
  {
    [CommandArgument(0, "<IP>")]
    public string IpAddress { get; set; }
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ReportIpAddressCommand.Settings settings)
  {
    throw new NotImplementedException(); 
  }
}


