using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace IPQSharp.Internals;

public class IPQSAccount : IIPQSAccount 
{

  public async Task<IPQSResult>
  ReportFraud(
    Action<FraudReportRequest> makeReport,
    CancellationToken token = default)
  {
    using var report = new FraudReportRequest();
    makeReport(report);
    var request = "https://www.ipqualityscore.com/api/json/report"
      .BeforeCall(call =>
      {
      })
      .AppendPathSegment(IPQS.ApiKey)
      .SetQueryParams(values: new
      {
        ip                  = report.IpAddress,
        email               = report.Email,
        request_id          = report.RequestId,
        phone               = report.Phone?.Number,
        country             = report.Phone?.CountryCode,
        billing_first_name  = report.BillingInfo?.FirstName,
        billing_last_name   = report.BillingInfo?.LastName,
        billing_company     = report.BillingInfo?.Company,
        billing_address_1   = report.BillingInfo?.Address?.Line1,
        billing_address_2   = report.BillingInfo?.Address?.Line2,
        billing_city        = report.BillingInfo?.Address?.City,
        billing_region      = report.BillingInfo?.Address?.Region,
        billing_postcode    = report.BillingInfo?.Address?.Postcode,
        billing_country     = report.BillingInfo?.Address?.Country,
        billing_email       = report.BillingInfo?.Email,
        billing_phone       = report.BillingInfo?.Phone,
        shipping_first_name = report.ShippingInfo?.FirstName,
        shipping_last_name  = report.ShippingInfo?.LastName,
        shipping_company    = report.ShippingInfo?.Company,
        shipping_address_1  = report.ShippingInfo?.Address?.Line1,
        shipping_address_2  = report.ShippingInfo?.Address?.Line2,
        shipping_city       = report.ShippingInfo?.Address?.City,
        shipping_region     = report.ShippingInfo?.Address?.Region,
        shipping_postcode   = report.ShippingInfo?.Address?.Postcode,
        shipping_country    = report.ShippingInfo?.Address?.Country,
        shipping_email      = report.ShippingInfo?.Email,
        shipping_phone      = report.ShippingInfo?.Phone,
        username            = report.Username,
        password_hash       = report.PasswordHash,
        credit_card_bin     = report.CreditCardBin,
        credit_card_hash    = report.CreditCardHash,
        credit_card_expiration_month = report.CreditCardExpiryMonth,
        credit_card_expiration_year  = report.CreditCardExpiryYear,
        avs_code            = report.CreditCardAVSCode,
        cvv_code            = report.CreditCardCVVCode,
        order_amount        = report.OrderAmount,
        order_quantity      = report.OrderQuantity,
        recurring           = report.IsRecurring,
        recurring_times     = report.RecurringTimes
      });
      var result = await request.GetJsonAsync<IPQSResult>(cancellationToken: token);
      return result;
  }
}
