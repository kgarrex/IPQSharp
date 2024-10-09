using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using NetJSON;

namespace IPQSharp.ProxyDetection;

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


public enum ConnectionType
{
  Residential,
  Corporate,
  Education,
  Mobile,
  DataCenter
}

public enum AbuseVelocityType
{
  None,
  Low,
  Medium,
  High
}

public class ProxyDetectionResult : IPQSResult
{
  /// <value>
  /// Is this IP address suspected to be a proxy? (SOCKS, Elite, Anonymous, VPN, Tor, etc.)
  /// </value>
  [NetJSONPropertyAttribute("proxy")]
  public bool? IsProxy { get; set; }

  /// <value>Hostname of the IP address if one is available.</value>
  [NetJSONPropertyAttribute("host")]
  public string? Hostname { get; set; }

  [NetJSONPropertyAttribute("ISP")]
  public string? InternetServiceProvider { get; set; }

  /// <value>
  /// Org is one is known. Can be parent company or sub company of the listed ISP. Otherwise "N/A".
  /// </value>
  public string? Organization { get; set; }

  [NetJSONPropertyAttribute("ASN")]
  public int? AutonomousSystemNumber { get; set; }

  [NetJSONPropertyAttribute("country_code")]
  public string? CountryCode { get; set; }

  [NetJSONPropertyAttribute("region")]
  public string? Region { get; set; }

  [NetJSONPropertyAttribute("timezone")]
  public string? TimeZone { get; set; }

  /// <value>
  /// Latitude of IP address if available or "N/A" if unknown.
  /// </value>
  [NetJSONPropertyAttribute("latitude")]
  public float? Latitude { get; set; }

  /// <value>
  /// Longitude of IP address if available or "N/A" if unknown.
  /// </value>
  [NetJSONPropertyAttribute("longitude")]
  public float? Longitude { get; set; }

  /// <value>
  /// Postal code of the IP address if available or "N/A".
  /// </value>
  [NetJSONPropertyAttribute("zip_code")]
  public string? ZipCode { get; set; }

  /**
   * <value>
   * Is this IP associated with being a confirmed crawler from a mainstream search
   * engine such as Googlebot, Bingbot, Yandex, etc. based on hostname or IP
   * address verification.
   * </value>
   */
  [NetJSONPropertyAttribute("is_crawler")]
  public bool? IsCrawler { get; set; }

  [NetJSONPropertyAttribute("connection_type")]
  public string? ConnectionType { get; set; }

  /**
   * <value>
   * This value will indicate if there has been any recently verified abuse across
   * the network for this IP address. Abuse could be a confirmed chargeback, account
   * takeover attack, compromised device, fake application or registration, digital
   * impersonation (stolen user data), bot attack, or similar malicious behavior within
   * the past few days.
   * </value>
   */
  [NetJSONPropertyAttribute("recent_abuse")]
  public bool? HasRecentAbuse { get; set; }

  /**
   * <value>
   * How frequently the IP address is engaging in abuse across the IPQS threat network.
   * Values can be "high", "medium", "low", or "none". Can be used in combination with
   * the Fraud Score to identify bad behavior.
   * </value>
   */
  [NetJSONPropertyAttribute("abuse_velocity")]
  public string? AbuseVelocity { get; set; }


  /**
   * <value>
   * Indicates if bots or non-human traffic has recently used this IP address to engage
   * in automated fraudulent behavior. Provides stronger confidence that the IP address
   * is suspicious.
   * </value>
   */
  [NetJSONPropertyAttribute("bot_status")]
  public bool? IsBot { get; set; }

  /**
   * <value>
   * Is this IP suspected of being a VPN connection? This can include data center ranges
   * which can become active VPNs at any time. The "proxy" status will always be
   * true when this value is true.
   * </value>
   */
  [NetJSONPropertyAttribute("vpn")]
  public bool? IsVpn { get; set; }

  [NetJSONPropertyAttribute("tor")]
  public bool? IsTor { get; set; }

  [NetJSONPropertyAttribute("active_vpn")]
  public bool? IsActiveVpn { get; set; }

  [NetJSONPropertyAttribute("active_tor")]
  public bool? IsActiveTor { get; set; }

  /**
   * <value>
   * Is this user agent a mobile browser? (will always be false if the user agent
   * is not passed in the API request)
   * </value>
   */
  [NetJSONPropertyAttribute("mobile")]
  public bool? IsMobileBrowser { get; set; }
  /**
   * <value>
   * The overall fraud score of the user based on the IP, user agent, language, and
   * any other optionally passed variables. Fraud Scores >= 75 are suspcious, but not
   * necessarily fraudulent. IPQS recommends flagging or blocking traffic with Fraud
   * Scores >= 90, but you may find it beneficial to use a higher or lower threshold.
   * </value>
   */
  [NetJSONPropertyAttribute("fraud_score")]
  public float? FraudScore { get; set; }
  /**
   * <value>
   * Enterprise Data Point - Identifies IP addresses with a consistent history of
   * abusive behavior across 6 months or more. This data point can be helpful in
   * identifying anonymous IP addresses which are frequently used for malicious behavior,
   * compared to an IP address that may be briefly compromised by malware and only
   * temporarily active in a botnet or residential proxy network.
   * </value>
   */
  [NetJSONPropertyAttribute("fraudulent_abuser")]
  public bool? IsFraudulentAbuser { get; set; }
  /**
   * <value>
   * Enterprise Data Point - Confirms if this IP address has engaged in malicious
   * abuse such as phishing, brute forcing, DDoS, credential stuffing & account
   * takeover, scraping, form submission spam, and similar attacks. This data point
   * has a high correlation with anonymous proxies, open proxies, public VPNs, and
   * easily accessible anonymizers.
   * </value>
   */
  [NetJSONPropertyAttribute("high_risk_attacks")]
  public bool? IsHighRiskAttacker { get; set; }
  /**
   * <value>
   * Enterprise Data Point - Designates IP addresses which are likely to have more
   * than a few users active on the IP address at the same time, such as mobile networks,
   * corporate exit points, and similar connections. This can also include libraries,
   * coffee shops, hotel lobbies, dormitories, hospitals and medical centers, company
   * VPNs, etc.
   * </value>
   */
  [NetJSONPropertyAttribute("shared_connection")]
  public bool? IsSharedConnection { get; set; }
  /**
   * <value>
   * Enterpise Data Point - Indicates IP addresses with dynamic assignment protocols,
   * which means that a user on this IP address will likely be assigned a different
   * IP address by this provider in the near future.
   * </value>
   */
  [NetJSONPropertyAttribute("dynamic_connection")]
  public bool? IsDynamicConnection { get; set; }
  /**
   * <value>
   * Enterprise Data Point - Indicates a verified online security scanner or endpoint
   * by a trusted security vendor such as Tenable, Qualys, and similar providers.
   * </value>
   */
  [NetJSONPropertyAttribute("security_scanner")]
  public bool? IsSecurityScanner { get; set; }
  /**
   * <value>
   * Enterprise Data Point - Identifies company networks and corporate access points
   * which have low abuse rates and high security protocols. IP addresses on these networks
   * may still be compromised by malware, however the network overall will be considered
   * trusted is this value is true.
   * </value>
   */
  [NetJSONPropertyAttribute("trusted_network")]
  public bool? IsTrustedNetwork { get; set; }

  [NetJSONPropertyAttribute("operating_system")]
  public string? OperatingSystem { get; set; }

  [NetJSONPropertyAttribute("browser")]
  public string? Browser { get; set; }

  [NetJSONPropertyAttribute("device_brand")]
  public string? DeviceBrand { get; set; }

  [NetJSONPropertyAttribute("device_model")]
  public string? DeviceModel { get; set; }

  /**
   * <value>
   * Additional scoring variables for risk analysis are available when
   * <see href="https://www.ipqualityscore.com/documentation/proxy-detection-api/transaction-scoring">
   * transaction scoring data</see> is passed through the API request. These variables are also
   * useful for scoring user data such as physical addresses, phone numbers, usernames, and
   * transaction details. The data points below are populated when at least 1 transaction data
   * paramaeter is present in the initial API request. The following transaction variables are "null"
   * when the necessary transaction parameters are not passed with the initial API request.
   * For instance, not passing the "billing_email" will return "valid_billing_email" as null.
   * </value>
   */
  [NetJSONPropertyAttribute("transaction_details")]
  public object? TransactionDetails { get; set; } // TODO finis writing out this object
}

public class ErrorResponse
{

}
