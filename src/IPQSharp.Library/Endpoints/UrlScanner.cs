using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace IPQSharp.UrlScanner;

public enum StrictnessLevel
{
  Low,
  Medium,
  High
}

public enum DomainTrustLevel
{
  NotRated,
  Malicious,
  Suspicious,
  Neutral,
  Positive,
  Trusted
}


public class UrlScanRequest : IPQSRequest
{
  public UrlScanRequest(string apiKey) : base(apiKey)
  {}
 
  public Uri? Url { get; set; }

  /// <value>
  /// How strict would we scan this URL? Stricter checks may provide a higher
  /// false-positive rate. We recommend defaulting to level "0", the lowest
  /// strictness setting, and increasing to "1" or "2" depending on your
  /// levels abuse.
  /// </value>
  [JsonPropertyName("strictness")]
  public StrictnessLevel Strictness { get; set; } = StrictnessLevel.Low;

  /// <value>
  /// When enabled, the API will provide quicker response times using lighter
  /// checks and analysis. This setting defaults to false.
  /// </value>
  [JsonPropertyName("fast")]
  public bool? DoQuickCheck { get; set; } = false;

  /// <value>
  /// Maximum number of seconds to perform live page scanning and follows redirects.
  /// Recommended to set this value to 5 if a quick response is not needed.
  /// </value>
  [JsonPropertyName("timeout")]
  public int? Timeout { get; set; } = 2; 

  public async Task<UrlScanResult> SendAsync(CancellationToken token = default)
  {
    var url = Flurl.Url.Parse("https://www.ipqualityscore.com/api/json/url")
      .AppendPathSegment(ApiKey)
      .AppendPathSegment(Url.ToString());
    var result = await url.GetJsonAsync<UrlScanResult>();
    return result;
  }
}

public class UrlScanResult : IPQSResult
{
  /// <value>
  /// Is this domain suspected of being unsafe due to phishing, malware, spamming,
  /// or abusive behavior? View the confidence level by analyzing the "risk_score".
  /// </value>
  [JsonPropertyName("unsafe")]
  public bool? IsUnsafe { get; set; }

  /// <value>
  /// Domain name of the final destination URL of the scanned link, after following
  /// all redirects. This value will display sub domains.
  /// </value>
  [JsonPropertyName("domain")]
  public string? DomainName { get; set; }

  /// <value>
  /// Parent domain to identify the root level domain of the final destination URL.
  /// This value exludes sub domains.
  /// </value>
  [JsonPropertyName("root_domain")]
  public string? RootDomainName { get; set; }

  /// <value>
  /// The IP address corresponding to the server of the domain name.
  /// </value>
  [JsonPropertyName("ip_address")]
  public string? ServerIPAddress { get; set; }
  
  /// <value>
  /// The country corresponding to the server's IP address.
  /// </value>
  [JsonPropertyName("country_code")]
  public string? CountryCode { get; set; }
  
  /// <value>
  /// The 2-letter ISO code corresponding to the content's language on this URL/domain.
  /// </value>
  [JsonPropertyName("language_code")]
  public string? LanguageCode { get; set; }

  /// <value>
  /// The server banner of the domain's IP address. For example: "nginx/1.16.0". Value
  /// will be "N/A" if unavailable.
  /// </value>
  [JsonPropertyName("server")]
  public string? ServerName { get; set; }

  /// <value>
  /// MIME type of URL's content. For example "text/html;charset=UTF-8". Value will
  /// be "N/A" if unavailable.
  /// </value>
  [JsonPropertyName("content_type")]
  public string? ContentType { get; set; }

  /// <value>
  /// The IPQS risk score which estimates the confidence level for malicious URL
  /// detection. Risk score 85+ are high risk, while Risk Score = 100 are confirmed
  /// as accurate.
  /// </value>
  [JsonPropertyName("risk_score")]
  public int? RiskScore { get; set; }

  /// <value>
  /// HTTP status code of the URL's response. This value should be "200" for a valid
  /// website. Value is "0" if URL is unreachable.
  /// </value>
  [JsonPropertyName("status_code")]
  public int? HttpStatusCode { get; set; }

  /// <value>
  /// Total number of bytes to download the URL's content. Value is "0" if URL is
  /// unreachable.
  /// </value>
  [JsonPropertyName("page_size")]
  public int? PageSize { get; set; }

  /// <value>
  /// Estimated popularity rank of website globally. Value is "0" if the domain
  /// is unranked or has low traffic.
  /// </value>
  [JsonPropertyName("domain_rank")]
  public int? DomainRank { get; set; }

  /// <value>
  /// The domain of the URL has valid DNS records.
  /// </value>
  [JsonPropertyName("dns_valid")]
  public bool? IsDNSValid { get; set; }

  /// <value>
  /// Is this URL suspected of being malicious or used for phishing or abuse? Use
  /// in conjunction with the "risk_score" as a confidence level.
  /// </value>
  [JsonPropertyName("suspicious")]
  public bool? IsSuspicious { get; set; }

  /// <value>
  /// Is this URL associated with malicious phishing behavior?
  /// </value>
  [JsonPropertyName("phishing")]
  public bool? IsPhishing { get; set; }

  /// <value>
  /// Is this URL associated with malware or viruses?
  /// </value>
  [JsonPropertyName("malware")]
  public bool? IsMalware { get; set; }

  /// <value>
  /// Is the domain of this URL currently parked with a for sale notice?
  /// </value>
  [JsonPropertyName("parking")]
  public bool? IsParked { get; set; }

  /// <value>
  /// Is the domain of this URL associated with email SPAM or abusive email
  /// addresses?
  /// </value>
  [JsonPropertyName("spamming")]
  public bool? IsSpam { get; set; }

  /// <value>
  /// Is this URL or domain hosting dating or adult content?
  /// </value>
  [JsonPropertyName("adult")]
  public bool? IsAdultContent { get; set; }

  /// <value>
  /// Website classification and category related to the content and industry of
  /// the site. Over 70 categories are available including "Video Streaming",
  /// "Trackers", "Gaming", "Privacy", "Advertising", "Hacking", "Malicious",
  /// "Phishing", etc. The value will be "N/A" if unknown.
  /// </value
  [JsonPropertyName("category")]
  public string? Category { get; set; }

  /// <value>
  /// Risk classification of the URL's domain based on past abuse issues and
  /// positive behavior signals. Values include: "trusted", "positive", "neutral",
  /// "suspicious", "malicious", or "not rated".
  /// </value>
  [JsonPropertyName("domain_trust")]
  public string? DomainTrust { get; set; }

  /// <value>
  /// Returns the URL's title meta tag as text.
  /// </value>
  [JsonPropertyName("page_title")]
  public string? PageTitle { get; set; }

  /// <value>
  /// Indicates if a URL shortener was found in the link or redirect of the URL's
  /// path.
  /// </value>
  [JsonPropertyName("short_link_redirect")]
  public bool? IsShortLink { get; set; }

  /// <value>
  /// Identifies free content hosting services like Weebly, Blogspot, and others
  /// which are more prone to hosting malicious content by abusive user. These sites
  /// are typically suspended very quickly and serve content on a sub domain of a
  /// popular website. Cybercriminals favor these sites since the overall domain
  /// reputation will be high.
  /// </value>
  [JsonPropertyName("hosted_content")]
  public bool? IsHostedContent { get; set; }

  /// <value>
  /// Signals that the domain belongs to a risky TLD extension frequently associated
  /// with malware, scams, or phishing.
  /// </value>
  [JsonPropertyName("risky_tld")]
  public bool? IsRiskyTLD { get; set; }

  /// <value>
  /// Confirms is the domain has a proper SPF DNS record.
  /// </value>
  [JsonPropertyName("spf_record")]
  public bool? IsSPFRecord { get; set; }

  /// <value>
  /// Confirms if the domain has a proper DMARC DNS record.
  /// </value>
  [JsonPropertyName("dmarc_record")]
  public bool? IsDMARCRecord { get; set; }

  /// <value>
  /// Comma separated list of technologies found on the URL, such as WordPress,
  /// Shopify, Cloudflare, Google Analytics, Google Ads, and similar popular services.
  /// </value>
  [JsonPropertyName("technologies")]
  public string[]? Technologies { get; set; }

  /// <value>
  /// The age of the URL
  /// </value>
  [JsonPropertyName("domain_age")]
  public object? DomainAge { get; set; }

  /// <value>
  /// Does the URL redirect to another domain when loaded in a browser?
  /// </value>
  [JsonPropertyName("redirected")]
  public bool? HasRedirect { get; set; }

  /// <value>
  /// List of MX records associated with the URL's domain name.
  /// </value>
  [JsonPropertyName("mx_records")]
  public string[]? MXRecords { get; set; }

  /// <value>
  /// List of A records associated with the URL's domain name.
  /// </value>
  [JsonPropertyName("a_records")]
  public string[]? ARecords { get; set; }

  /// <value>
  /// List of NS records associated with the URL's domain name.
  /// </value>
  [JsonPropertyName("ns_records")]
  public string[]? NSRecords { get; set; }

  /// <value>
  /// Original URL which was analyzed for malware, phishing, abuse, etc. before
  /// any redirections.
  /// </value>
  [JsonPropertyName("scanned_url")]
  public string? OriginUrl { get; set; }

  /// <value>
  /// Destination URL after all redirections during our real-time link scan.
  /// </value>
  [JsonPropertyName("final_url")]
  public string? FinalUrl { get; set; }
}
