using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

public class ReportFraudSettings : IPQSSettings
{
  [CommandOption("--billing-first-name")]
  public string BillingFirstName { get; set; }

  [CommandOption("--username")]
  public string Username { get; set; }

  [CommandOption("--password")]
  public string PasswordHash { get; set; }

  [CommandOption("--ccbin")]
  public string CreditCardBin { get; set; }

  [CommandOption("--cchash")]
  public string CreditCardHash { get; set; }

  [CommandOption("--ccavs")]
  public string CreditCardAVSCode { get; set; }

  [CommandOption("--cccvv")]
  public string CreditCardCVVCode { get; set; }

  [CommandOption("--ccmonth")]
  public byte CreditCardExpiryMonth { get; set; }

  [CommandOption("--ccyear")]
  public byte CreditCardExpiryYear { get; set; }

  [CommandOption("--amount")]
  public decimal OrderAmount { get; set; }

  [CommandOption("--quantity")]
  public int OrderQuantity { get; set; }

  [CommandOption("--recurring")]
  [Description("Is this a recurring order that automatically rebills?")]
  public bool IsRecuring { get; set; }

  [CommandOption("--recurring-times")]
  [Description("The number of times a recurring order is rebilled")]
  public int RecurringTimes { get; set; }
}
