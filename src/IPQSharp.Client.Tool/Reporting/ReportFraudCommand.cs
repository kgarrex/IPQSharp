using System.Threading;
using System.Threading.Tasks;

public class ReportFraudSettings : IPQSSettings
{
  [CommandOption("--billing-first-name")]
  public string BillingFirstName { get; set; }
}
