using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace IPQSharp;

public class FraudReportRequest : IPQSRequest 
{
  public FraudReportRequest(string apiKey) : base(apiKey)
  {}

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

  public async Task<IPQSResult>
  SendAsync(CancellationToken token = default)
  {
    var request = "https://www.ipqualityscore.com/api/json/report"
      .BeforeCall(call =>
      {
      })
      .AppendPathSegment(ApiKey)
      .SetQueryParams(values: new
      {
        ip                  = this.IpAddress,
        email               = this.Email,
        request_id          = this.RequestId,
        phone               = this.Phone?.Number,
        country             = this.Phone?.CountryCode,
        billing_first_name  = this.BillingInfo?.FirstName,
        billing_last_name   = this.BillingInfo?.LastName,
        billing_company     = this.BillingInfo?.Company,
        billing_address_1   = this.BillingInfo?.Address?.Line1,
        billing_address_2   = this.BillingInfo?.Address?.Line2,
        billing_city        = this.BillingInfo?.Address?.City,
        billing_region      = this.BillingInfo?.Address?.Region,
        billing_postcode    = this.BillingInfo?.Address?.Postcode,
        billing_country     = this.BillingInfo?.Address?.Country,
        billing_email       = this.BillingInfo?.Email,
        billing_phone       = this.BillingInfo?.Phone,
        shipping_first_name = this.ShippingInfo?.FirstName,
        shipping_last_name  = this.ShippingInfo?.LastName,
        shipping_company    = this.ShippingInfo?.Company,
        shipping_address_1  = this.ShippingInfo?.Address?.Line1,
        shipping_address_2  = this.ShippingInfo?.Address?.Line2,
        shipping_city       = this.ShippingInfo?.Address?.City,
        shipping_region     = this.ShippingInfo?.Address?.Region,
        shipping_postcode   = this.ShippingInfo?.Address?.Postcode,
        shipping_country    = this.ShippingInfo?.Address?.Country,
        shipping_email      = this.ShippingInfo?.Email,
        shipping_phone      = this.ShippingInfo?.Phone,
        username            = this.Username,
        password_hash       = this.PasswordHash,
        credit_card_bin     = this.CreditCardBin,
        credit_card_hash    = this.CreditCardHash,
        credit_card_expiration_month = this.CreditCardExpiryMonth,
        credit_card_expiration_year  = this.CreditCardExpiryYear,
        avs_code            = this.CreditCardAVSCode,
        cvv_code            = this.CreditCardCVVCode,
        order_amount        = this.OrderAmount,
        order_quantity      = this.OrderQuantity,
        recurring           = this.IsRecurring,
        recurring_times     = this.RecurringTimes
      });
    var result = await request.GetJsonAsync<IPQSResult>(cancellationToken: token);
    return result;
  }
}
