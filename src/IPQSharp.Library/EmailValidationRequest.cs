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
