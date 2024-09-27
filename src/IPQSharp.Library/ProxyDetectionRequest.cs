using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace IPQSharp;

public enum StrictnessLevel
{
  Low,
  Medium,
  High,
  VeryHigh
}

public enum TransactionStrictnessLevel
{
  Low,
  Medium,
  High
}

public class ProxyDetectionRequest : IPQSRequest
{
  public ProxyDetectionRequest(string apiKey, string ipString) :
    this(apiKey, IPAddress.Parse(ipString))
  {}

  public ProxyDetectionRequest(string apiKey, IPAddress ipAddress) : base(apiKey)
  {
    IpAddress = ipAddress;
  }

  /**
   * <value>
   * The IP address to check
   * </value>
   */
  public IPAddress? IpAddress { get; }


  /**
   * <value>
   * How in depth (strict) do you want this query to be? Higher values take
   * longer to process and may provide a higher false-positive rate. We
   * recommend starting at "0", the lowest strictness setting, and increasing
   * to "1" depending on your levels of fraud. Levels 2+ are VERY strict and
   * will produce false-positives.
   * </value>
   */
  [Range(0,3)]
  public StrictnessLevel Strictness { get; set; } = StrictnessLevel.Low;


  /**
   * <value>
   * You can optionally provide us with the user agent string (browser). This
   * allows us to run additional checks to see if the user is a bot or running
   * an invalid browser. This allows us to evaluate the risk of the user as
   * judged in the "fraud_score".
   * </value>
   */
  public string? UserAgent { get; set; }


  /**
   * <value>
   * You can optionally provide us with the user's language header. This
   * allows us to evaluate the risk of the user as judged in the "fraud_score".
   * </value>
   */
  public string? UserLanguage { get; set; }


  /**
   * <value>
   * When this parameter is enabled our API will not perform certain forensic
   * checks that take longer to process. Enabling this feature greatly increases
   * the API speed without much impact on accuracy. This option is intended for
   * services that require decision making in a time sensitive manner and can
   * be used for any strictness level. Not recommended.
   * </value>
   */
  public bool DoFastLookup { get; set; } = false;


  /**
   * <value>
   * You can optionally specify that this lookup should be treated as a mobile
   * device. Recommended for mobile lookups that do not have a user agent
   * attached to the request. NOTE: This can cause unexpected and abnormal
   * results if the device is not a mobile device.
   * </value>
   */
  public bool IsMobileDevice { get; set; } = false;


  /**
   * <value>
   * Bypasses certain checks for IP addresses from education and research
   * institutions, schools, and some corporate connections to better
   * accomodate audiences that frequently use public connections.
   * </value>
   */
  public bool AllowPublicAccessPoints { get; set; } = true;


  /**
   * <value>
   * Is your scoring too strict? Enable this setting to lower detection rates and
   * Proxy Scores for mixed quality IP addresses. If you experience any false-positives
   * with your traffic then enabling this feature will provide better results.
   * </value>
   */
  public bool HaveLighterPenalties { get; set; } = true;


  /**
   * <value>
   * Adjusts the weights for penalties applied due to irregularities and fraudulent
   * patterns detected on order and transaction details that can be optionally
   * provided on each API request. This feature is only beneficial if you are
   * passing order and transaction details. A table is available further down
   * the page with supported transaction variables.
   * </value>
   */
  [Range(0,2)]
  public TransactionStrictnessLevel TransactionStrictness { get; set; } = TransactionStrictnessLevel.Low;


  public async Task<ProxyDetectionResult>
  SendAsync(CancellationToken token = default)
  {
    var response = await "https://www.ipqualityscore.com/api/json/ip"
      .WithHeader("IPQS-KEY", ApiKey)
      .PostUrlEncodedAsync(body: new
      {
        ip                         = this.IpAddress,
        strictness                 = this.Strictness,
        user_agent                 = this.UserAgent,
        user_language              = this.UserLanguage,
        fast                       = this.DoFastLookup,
        mobile                     = this.IsMobileDevice,
        allow_public_access_points = this.AllowPublicAccessPoints,
        lighter_penalties          = this.HaveLighterPenalties,
        transaction_strictness     = this.TransactionStrictness
      }, cancellationToken: token);
    var result = await response.GetJsonAsync<ProxyDetectionResult>();
    return result;
  }
}


public static class ProxyDetectionRequestExtensions
{
  public static ProxyDetectionRequest WithUserAgent(this ProxyDetectionRequest request, string userAgent)
  {
    request.UserAgent = userAgent;
    return request;
  }

  public static ProxyDetectionRequest WithStrictPenalties(this ProxyDetectionRequest request)
  {
    request.HaveLighterPenalties = false;
    return request;
  }

  public static ProxyDetectionRequest WithNoPublicAccessPoints(this ProxyDetectionRequest request)
  {
    request.AllowPublicAccessPoints = false;
    return request;
  }

  public static ProxyDetectionRequest WithMobileDevice(this ProxyDetectionRequest request)
  {
    request.IsMobileDevice = true;
    return request;
  }

  public static ProxyDetectionRequest WithFastLookup(this ProxyDetectionRequest request)
  {
    request.DoFastLookup = true;
    return request;
  }

  public static ProxyDetectionRequest WithUserLanguage(this ProxyDetectionRequest request, string language)
  {
    request.UserLanguage = language;
    return request;
  }

  public static ProxyDetectionRequest WithStrictnessLevel(this ProxyDetectionRequest request, StrictnessLevel level)
  {
    request.Strictness = level;
    return request;
  }

  public static ProxyDetectionRequest WithStrictnessLevelLow(this ProxyDetectionRequest request)
  {
    request.Strictness = StrictnessLevel.Low;
    return request;
  }
  
  public static ProxyDetectionRequest WithStrictnessLevelMedium(this ProxyDetectionRequest request)
  {
    request.Strictness = StrictnessLevel.Medium;
    return request;
  }

  public static ProxyDetectionRequest WithStrictnessLevelHigh(this ProxyDetectionRequest request)
  {
    request.Strictness = StrictnessLevel.High;
    return request;
  }

  public static ProxyDetectionRequest WithStrictnessLevelVeryHigh(this ProxyDetectionRequest request)
  {
    request.Strictness = StrictnessLevel.VeryHigh;
    return request;
  }

  public static ProxyDetectionRequest WithTransactionStrictnessLevel(this ProxyDetectionRequest request, TransactionStrictnessLevel level)
  {
    request.TransactionStrictness = level;
    return request;
  }

  public static ProxyDetectionRequest WithTransactionStrictnessLevelLow(this ProxyDetectionRequest request)
  {
    request.TransactionStrictness = TransactionStrictnessLevel.Low;
    return request;
  }

  public static ProxyDetectionRequest WithTransactionStrictnessLevelMedium(this ProxyDetectionRequest request)
  {
    request.TransactionStrictness = TransactionStrictnessLevel.Medium;
    return request;
  }

  public static ProxyDetectionRequest WithTransactionStrictnessLevelHigh(this ProxyDetectionRequest request)
  {
    request.TransactionStrictness = TransactionStrictnessLevel.High;
    return request;
  }
}
