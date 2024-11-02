using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace IPQSharp.EmailValidation;

public enum StrictnessLevel
{
  Low,
  Medium,
  High
}

public enum AbuseStrictnessLevel
{
  Low,
  Medium,
  High
}

public class EmailValidationRequest : IPQSRequest
{
  public EmailValidationRequest(string apiKey, string email) : base(apiKey)
  {
    Email = email;
  }

  public string Email { get; set; }

  /**
   * <value>
   * When this parameter is enabled our API will not perform an SMTP check with the
   * mail service provider, which greatly increases the API speed. A syntax check
   * and DNS check (MX records, A records) are still performed on the email address
   * as well as our email risk scoring which detects disposable email addresses.
   * This option is intended for services that require decision making in a time
   * sensitive manner.
   * </value>
   */
  public bool DoFastLookup { get; set; } = false;

  /**
   * <value>
   * Maximum number of seconds to wait for a reply from a mail service provider. If
   * your implementation requirements do not need an immediate response, we
   * recommend bumping this value to 20. Any results which experience a connection
   * timeout return the "time_out" variable as true. Default value is 7 seconds.
   * </value>
   */
  public int Timeout { get; set; } = 7;


  /**
   * <value>
   * Force analyze if the email address's domain has a typo and should be corrected
   * to a popular mail service. By default, this test is currently only performed
   * when the email is invalid or if the "recent_abuse" status is true.
   * </value>
   */
  public bool DoSuggestDomain { get; set; } = false;

  /**
   * <value>
   * Sets how strictly spam traps and honeypots are detected by our system,
   * depending on how comfortable you are with identifying mails suspected of being
   * a spam trap. 0 is the lowest level which will only return spam traps with high
   * confidence. Strictness levels above 0 will return increasingly more strict
   * results, with level 2 providing the greatest detection rates.
   * </value>
   */
  public StrictnessLevel Strictness { get; set; } = StrictnessLevel.Low;

  /**
   * <value>
   * Set the strictness level for machine learning pattern recognition of abusive
   * email addresses with the "recent_abuse" data point. Default level of 0
   * provides good coverage, however if you are filtering account applications and
   * facing advanced fraudsters then we recommend increasing this value to level 1
   * or 2.
   * </value>
   */
  public AbuseStrictnessLevel AbuseStrictness { get; set; } = AbuseStrictnessLevel.Low;

  public async Task<EmailValidationResult>
  SendAsync(CancellationToken token = default)
  {
    var response = await "https://www.ipqualityscore.com/api/json/email"
      .WithHeader("IPQS-KEY", ApiKey)
      .AppendPathSegment(Email)
      .PostUrlEncodedAsync(body: new
      {
        fast              = this.DoFastLookup,
        timeout           = this.Timeout,
        suggest_domain    = this.DoSuggestDomain,
        strictness        = this.Strictness,
        abuse_strictness  = this.AbuseStrictness
      }, cancellationToken: token);
    var result = await response.GetJsonAsync<EmailValidationResult>();
    return result;
  }
}

public static class EmailValidationRequestExtensions
{

}


public enum DeliverabilityLevel
{
  Low,
  Medium,
  High
}

public enum DomainVelocityLevel
{
  None,
  Low,
  Medium,
  High
}

public enum UserActivityLevel
{
  None,
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


public class EmailValidationResult : IPQSResult
{
  /// <value> Does this email address appear valid? </value>
  public bool? IsValid { get; set; }

  /// <value>
  /// Is this email suspected of belonging to a temporary or disposable
  /// email service? Usually associated with fraudsters and scammers.
  /// </value>
  public bool? IsDisposable { get; set; }

  /// <value>
  /// Did the email verification connection to the mail service provider timeout
  /// during the verification? If so, we recommend increasing the "timeout"
  /// variable above the default 7 second value so more time can be spent
  /// during the mailbox verification request to mail servers. Lookups that
  /// timeout with a "valid" result as false are most likely false and should not be
  /// trusted.
  /// </value>
  public bool? IsTimedOut { get; set; }

  /// <value>
  /// How likely is this email to be delivered to the user and land in their
  /// mailbox.
  /// </value>
  public DeliverabilityLevel Deliverability { get; set; }
  
  /// <value>
  /// Is this email likely to be a "catch all" where the mail server verifies all
  /// emails tested against it as valid? It is difficult to determine if the address
  /// is truly valid in these scenarios, since the email's server will not confirm
  /// the account's status.
  /// </value>
  public bool? IsCatchAll { get; set; }

  /// <value>
  /// Was this email address associated with a recent database leak from a third
  /// party? Leaked accounts pose a risk as they may have become compromised during
  /// a database breach.
  /// </value>
  public bool? IsLeaked { get; set; }

  /// <value>
  /// This value indicates if the mail server is currently replying with a temporary
  /// mail server error or if the email verification system is unable to verify
  /// the email address due to a broken SMTP handshake. This status will also be
  /// true for "catch all" email addresses as defined below. If this value is true,
  /// then we suspect the "valid" result may be tainted and there is not a guarantee
  /// that the email address is truly valid. This status is rarely true for popular
  /// mailbox providers and typically only returns as true for a small percentage
  /// of business mail servers.
  /// </value>
  public bool? IsSuspicious { get; set; }

  /// <value>
  /// Validity score of the email server's SMTP setup. Range: "-1" - "3". Scores
  /// above "-1" can be associated with a valid email.
  /// -1 = invalid email address
  ///  0 = mail server exists, but is rejecting all mail
  ///  1 = mail server exists, but is showing a temporary error
  ///  2 = mail server exists, but accepts all email
  ///  3 = mail server exists and has verified the email address.
  /// </value>
  public int SmtpScore { get; set; }

  /// <value>
  /// Overall email validity score. Range: "0" - "4". Scores above "1" can be
  /// associated with a valid email.
  /// 0 = invalid email address
  /// 1 = dns valid, unreachable email server
  /// 2 = dns valid, temporary mail rejection error
  /// 3 = dns valid, accepts all mail
  /// 4 = dns valid, verified email exists
  /// </value>
  public int OverallScore { get; set; }


  /// <value>
  /// Suspected first name based on email. Returns "CORPORATE" if the email is
  /// suspected of being a generic company email. Returns "UKNOWN" if the first
  /// name was not determinable.
  /// </value>
  public string? FirstName { get; set; }

  /// <value>
  /// Is this email from common free email providers? (gmail, yahoo, hotmail, etc.)
  /// </value>
  public bool? IsCommonProvider { get; set; }

  /// <value>
  /// Is this email suspected as being a catch all or shared email for a domain?
  /// ("admin@", "webmaster@", "newsletter@", "sales@", "contact@", etc.)
  /// </value>
  public bool? IsGeneric { get; set; }

  /// <value>
  /// Does this email's hostname have valid DNS entries? Partial indication of a
  /// valid email.
  /// </value>
  public bool? IsDnsValid { get; set; }

  /// <value>
  /// Is this email believed to be a "honeypot" or "SPAM trap"? Bulk mail sent to
  /// these emails increases your risk of being blacklisted by large ISPs and ending
  /// up in the spam folder.
  /// </value>
  public bool? IsHoneypot { get; set; }

  /// <value>
  /// Intelligent confidence level of the email address being an active SPAM trap.
  /// Values can be "high", "medium", "low", or "none". We recommend scrubbing
  /// emails with a "high" status, typically for any promotional mailings. This data
  /// is meant to provide a more accurate result for the "frequent_complainer" and
  /// "honeypot" data points, which collect data from spam complaints, spam traps,
  /// and similar techniques.
  /// </value>
  public string? SpamTrapScore { get; set; } 

  /// <value>
  /// This value will indicate if there has been any recently verified abuse across
  /// our network for this email adress. Abuse could be a confirmed chargeback, fake
  /// signup, compromised device, fake app install, or similar malicious behavior
  /// within the past few days.
  /// </value>
  public bool? HasRecentAbuse { get; set; }

  /// <value>
  /// The overall Fraud Score of the user based on the email's reputation and recent
  /// behavior across the IPQS threat network. Fraud Scores >= 75 are suspicious,
  /// but not necessarily fraudulent.
  /// </value>
  public float? FraudScore { get; set; }

  /// <value>
  /// Indicates if this email frequently unsubscribes from marketing lists or
  /// reports spam complaints.
  /// </value>
  public bool? IsFrequentComplainer { get; set; }

  /// <value>
  /// Default value is "N/A". Indicates if this email's domain should in fact be
  /// corrected to a popular mail service. This field is useful for catching user
  /// typos. For Example, an email address with "gmai.com", would display a
  /// suggested domain of "gmail.com". This feature supports all major mail service
  /// providers.
  /// </value>
  public string? SuggestedDomain { get; set; }

  /// <value>
  /// Indicates the level of legitimate user interacting with the email address
  /// domain. Values can be "high", "medium", "low", or "none". Domains like
  /// "IBM.com", "Microsoft.com", "Gmail.com", etc. will have "high" scores as this
  /// value represents popular domains. New domains or domains that are not
  /// frequently visited by legitimate users will have a value as "none". This field
  /// is restricted to upgraded plans.
  /// </value>
  public string? DomainVelocity { get; set; }

  /// <value>
  /// Risk classification of the email's domain based on past abuse issues and 
  /// positive behavior signals.
  /// </value>
  public string? DomainTrust { get; set; }

  /// <value>
  /// Frequency at which this email address makes legitimate purchases, account
  /// registrations, and engages in legitimate user behavior online. Values of "high"
  /// or "medium" are strong signlas of healthy usage. New email addresses without
  /// a history of legitimate behavior will have a value as "none". This field is
  /// restricted to higher plan tiers.
  /// </value>
  public string? UserActivity { get; set; }

  public AssociatedNames? AssociatedNames { get; set; }
  
  public AssociatedPhoneNumbers? AssociatedPhoneNumbers { get; set; }

  /// <value>
  /// List of MX records associated with the email's domain name.
  /// </value>
  public string[] MXRecords { get; set; }
  
  /// <value>
  /// List of A records associated with the email's domain name.
  /// </value>
  public string[] ARecords { get; set; }

  /// <value>
  /// Signals that the domain belongs to a risky TLD extension frequently associated
  /// with malware, scams, abuse, or phishing.
  /// </value>
  public bool? IsRiskyTLD { get; set; }

  /// <value>
  /// Confirms if the domain has a proper SPF DNS record.
  /// </value>
  public bool? HasSPFRecord { get; set; }

  /// <value>
  /// Confirms if the domain has a proper DMARC DNS record.
  /// </value>
  public bool? HasDMARCRecord { get; set; }

  public FirstSeen FirstSeen { get; set; }

  public DomainAge DomainAge { get; set; }

  /// <value>
  /// Sanitized email address with all aliases and masking removed, such as multiple
  /// periods for Gmail.com.
  /// </value>
  public string? SanitizedEmail { get; set; }
}


public class AssociatedNames
{
  public string Status { get; set; }
  public string[] Names { get; set; }
}

public class AssociatedPhoneNumbers
{
  public string Status { get; set; }
  public string[] PhoneNumbers { get; set; }
}

public class FirstSeen
{
  /// <value>
  /// A human description of the email address age, using an estimation of the
  /// email creation date when IPQS first discovered this email address.
  /// (Ex: 3 months ago)
  /// </value>
  public string? Human { get; set; } 

  /// <value>
  /// The unix time since epoch when this email was first analyzed by IPQS.
  /// (Ex: 1568061634)
  /// </value>
  public int? Timestamp { get; set; }

  /// <value>
  /// The time this email was first analyzed by IPQS in ISO8601 format
  /// (Ex: 2019-09-09T16:40:34-04:00)
  /// </value>
  public string Datetime { get; set; }
}

public class DomainAge
{
  /// <value>
  /// A human description of when this domain was registered.
  /// (Ex: 3 months ago)
  /// </value>
  public string? Human { get; set; }

  /// <value>
  /// The unix time since epoch when this domain was first registed.
  /// (Ex: 1568061634)
  /// </value>
  public int? Timestamp { get; set; }

  /// <value>
  /// The time this domain was registered in ISO8601 format.
  /// (Ex: 2019-09-09T16:40:34-04:00)
  /// </value>
  public string? Datetime { get; set; }
}
