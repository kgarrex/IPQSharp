using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using IPQSharp;

namespace IPQSharp.PhoneValidation;

public enum StrictnessLevel
{
  Low,
  Medium,
  High
}

public class PhoneValidationRequest : IPQSRequest
{
  public PhoneValidationRequest(string apiKey, string phone) : base(apiKey)
  {
  
  }

  public string? Phone { get; set; }

  /// <value>
  /// How in depth (strict) do you want this reputation check to be? Stricter checks
  /// may provide a higher false-positive rate. We recommend starting at "0", the
  /// lowest strictness setting, and increasing to "1" and "2" depending on your
  /// levels of fraud.
  /// </value>
  public StrictnessLevel Strictness { get; set; } = StrictnessLevel.Low;

  /// <value>
  /// You can optionally provide us with the default country or countries this phone
  /// number is suspected to be associated with. Our system will prefer to use a
  /// country on this list for verification or will require a country to be specified
  /// in the event the phone number is less than 10 digits.
  /// </value>
  public string Country { get; set; } = "ZZ";

  /// <value>
  /// Please contact support to activate this feature for more advanced active line
  /// checks through our HLR lookup service. This feature provides greater accuracy
  /// for identifying active or disconnected phone numbers including landline,
  /// mobile, and VOIP services. The "active_status" field is also populated when
  /// this feature is enabled.
  /// </value>
  public bool DoEnhancedLineCheck { get; set; } = false;

  /// <value>
  /// Please contact support to activate enhanced name appending for a phone number.
  /// Our standard phone validation service already includes extensive name
  /// appending data.
  /// </value>
  public bool DoEnhancedNameCheck { get; set; } = false;

  public async Task<PhoneValidationResult>
  SendAsync(CancellationToken token = default)
  {
    var response = await "https://www.ipqualityscore.com/api/json/phone"
      .WithHeader("IPQS-KEY", ApiKey)
      .AppendPathSegment(Phone)
      .PostUrlEncodedAsync(body: new
      {
        strictness          = this.Strictness,
        country             = this.Country,
        enhanced_line_check = this.DoEnhancedLineCheck,
	enhanced_name_check = this.DoEnhancedNameCheck
      }, cancellationToken: token);
    var result = await response.GetJsonAsync<PhoneValidationResult>();
    return result;
  }
}

public static class PhoneValidationRequestExtensions
{

}
