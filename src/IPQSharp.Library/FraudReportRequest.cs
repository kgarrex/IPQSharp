using System.Net;

namespace IPQSharp.Internals;

public class FraudReportRequest : IDisposable
{
  public IPAddress? IpAddress { get; set; }
  public string? Email { get; set; }
  public string? RequestId { get; set; }
  public PhoneNumber? Phone { get; set; }
  public record PhoneNumber(string Number, string CountryCode);

  public TransactionInfo BillingInfo { get; set; } = new TransactionInfo();
  public TransactionInfo ShippingInfo { get; set; } = new TransactionInfo();

  /// <value>The customer's username.</value>
  public string? Username { get; set; }

  /// <value>
  /// For security reasons and following industry best practices, a SHA256
  /// hash of the user's password for better user analysis.
  /// </value>
  public string? PasswordHash { get; set; }

  /// <value>
  /// First six digits of the credit or debit card, referred to ask the
  /// Bank Identification Number.
  /// </value>
  public string CreditCardBin { get; set; }

  /// <value>
  /// For security reasons and following industry best practices, a SHA256
  /// hash of the credit card number is accepted to check against
  /// blacklisted cards.
  /// </value>
  public string CreditCardHash { get; set; }

  /// <value>
  /// Two letter format of the credit card's expiration month. For example,
  /// May would be "05".
  /// </value>
  public byte CreditCardExpiryMonth { get; set; }

  /// <value>
  /// Two letter format of the credit card's expiration year. For example,
  /// 2022 would be "22".
  /// </value>
  public short CreditCardExpiryYear { get; set; }

  /// <value>
  /// One letter Address Verification Service (AVS) response code provided
  /// by the credit card processor or bank.
  /// </value>
  public char CreditCardAVSCode { get; set; }

  /// <value>
  /// One letter Card Verification Value (CVV2) response code provided by the
  /// credit card processor or bank.
  /// </value>
  public char CreditCardCVVCode { get ;set; }

  /// <value>Total balance of the entire order without currency symbols.</value>
  public decimal OrderAmount { get; set; }

  /// <value>Quantity of items for this order.</value>
  public int OrderQuantity { get; set; }

  /// <value>Is this a recurring order that automatically rebills?</value>
  public bool IsRecurring { get; set; }

  /// <value>
  /// If this is a recurring order, then how many times has this recurring order
  /// rebilled? For example, if this is the third time the user is being billed, please
  /// enter this value as "3". If this is the initial recurring order, please leave the
  /// value as blank or enter "1".
  /// </value>
  public int RecurringTimes { get; set; }


  public class CreditCardInfo
  {
  
  }

  public class TransactionInfo 
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    public Address? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
  }

  public void Dispose(){}
}
