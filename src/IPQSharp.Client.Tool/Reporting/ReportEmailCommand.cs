public class ReportEmailCommand : AsyncCommand<ReportEmailCommand.Settings>
{
  public class Settings : ReportFraudSettings
  {
    [CommandArgument(0, "<EMAIL>")]
    public string Email { get; set; }
  }

  public class TransactionInfo
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    //public Address? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
  }

  public override async System.Threading.Tasks.Task<int> ExecuteAsync
  (CommandContext context, ReportEmailCommand.Settings settings)
  {
    throw new NotImplementedException(); 
  }
}
