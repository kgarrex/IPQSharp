using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using IPQSharp;

namespace IPQSharp.CreditCardUsage;

public class CreditCardUsageRequest : IPQSRequest
{
  public CreditCardUsageRequest(string apiKey) : base(apiKey)
  {}

  public async Task<CreditCardUsageResult>
  SendAsync(CancellationToken token = default)
  {
    var url = Flurl.Url.Parse("https://www.ipqualityscore.com/api/json/account")
      .AppendPathSegment(ApiKey);
    var result = await url.GetJsonAsync<CreditCardUsageResult>();
    return result;
  }
}

public class CreditCardUsageResult : IPQSResult
{
  /// <value> The total number of credits available to an account </value>
  [JsonPropertyName("credits")]
  public int Credits { get; set; }

  [JsonPropertyName("usage")]
  public int Usage { get; set; }

  [JsonPropertyName("proxy_usage")]
  public int ProxyUsage { get; set; }

  [JsonPropertyName("email_usage")]
  public int EmailUsage { get; set; }

  [JsonPropertyName("mobile_sdk_usage")]
  public int MobileSdkUsage { get; set; }

  [JsonPropertyName("phone_usage")]
  public int PhoneUsage { get; set; }

  [JsonPropertyName("url_usage")]
  public int UrlUsage { get; set; }

  [JsonPropertyName("fingerprint_usage")]
  public int FingerprintUsage { get; set; }
}
