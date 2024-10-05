namespace IPQSharp.PhoneValidation;

public enum LineType
{
  Unknown,
  TollFree,
  Wireless,
  Landline,
  Satellite,
  VOIP,
  PremiumRate,
  Pager
}

public class PhoneValidationResult : IPQSResult
{
  /// <value>
  /// Is the phone number properly formatted and considered valid based
  /// on assigned phone numbers available to carriers in that country?
  /// </value>
  public bool? IsValid { get; set; }

  /// <value>
  /// Is this phone number a live usable phone number that is currently active?
  /// This feature requires a separate data set which provides subscriber status
  /// details shared directly from the Telecom provider. Contact your account
  /// manager or support to enable this add-on feature, which determines if a 
  /// phone number is reachable or disconnected using our HLR lookup service and
  /// carrier signals.
  /// </value>
  public bool? IsActive { get; set; }

  /// <value>
  /// The phone number formatted in the international dialing code. N/A if not
  /// formattable.
  /// </value>
  public string? InternationalFormat { get; set; }

  /// <value>
  /// The phone number formatted in the country's local routing rule with area
  /// code. N/A if not formattable.
  /// </value>
  public string? LocalFormat { get; set; }

  /// <value>
  /// The IPQS risk score which estimates how likely a phone number is to be
  /// fraudulent. Scores 85+ are risky while Fraud Scores 90+ are high risk.
  /// </value>
  public int? FraudScore { get; set; }

  /// <value>
  /// Has this phone number been associated with recent or ongoing fraud?
  /// </value>
  public bool? HasRecentAbuse { get; set; }

  /// <value>
  /// Is this phone number a Voice Over Internet Protocol (VOIP) or digital phone
  /// number?
  /// </value>
  public bool? IsVOIP { get; set; }

  /// <value>
  /// Is this this phone number associated with a prepaid service plan?
  /// </value>
  public bool? IsPrepaid { get; set; }

  /// <value>
  /// Is this phone number associated with fraudulent activity, scams, robo calls,
  /// fake accounts, or other unfriendly behavior?
  /// </value>
  public bool? IsRisky { get; set; }

  /// <value>
  /// The owner name of the phone number such as the first or last name or business
  /// name assigned to the phone number. Multiple name will be returned in comma
  /// separated format. Value is "N/A" if unknown.
  /// </value>
  public string? OwnerName { get; set; }

  /// <value>
  /// The carrier (service provider) this phone number has been assigned to or "N/A"
  /// if unknown.
  /// </value>
  public string? CarrierName { get; set; }

  /// <value>
  /// The type of line this phone number is associated with (Toll Free, Wireless,
  /// Landline, Satellite, VOIP, Premium Rate, Pager) or "N/A" if unknown. Line Type
  /// can be play an important role for understanding phone number reputation.
  /// </value>
  public LineType? LineType { get; set; }

  /// <value>
  /// The two character country code for this phone number.
  /// </value>
  public string? Country { get; set; }

  /// <value>
  /// Region (state) of the phone number if available or "N/A" if unknown.
  /// </value>
  public string? Region { get; set; }

  /// <value>
  /// City of the phone number if available or "N/A" if unknown.
  /// </value>
  public string? City { get; set; }

  /// <value>
  /// Indicates whether the phone number's dialing code matches any of the
  /// provided country code(s).
  /// </value>
  public bool? IsAccurateCountryCode { get; set; }

  /// <value>
  /// Zip or Postal code of the phone number if available or "N/A" if unknown.
  /// </value>
  public string? ZipCode { get; set; }

  /// <value>
  /// Timezone of the phone number if available or "N/A" if unknown.
  /// </value>
  public string? Timezone { get; set; }

  /// <value>
  /// The 1 to 4 digit dialing code for this phone number or null if unknown.
  /// </value>
  public int? DialingCode { get; set; }

  /// <value>
  /// Indicates if the phone number is listed on any Do Not Call (DNC) lists. Only
  /// supported in US and CA. This data may not be 100% up to date with the latest
  /// DNC blacklists. Contact your account manager to enable better DNC data and
  /// TCPA litigator removal.
  /// </value>
  public bool? IsDoNotCall { get; set; }

  /// <value>
  /// Has this phone number recently been exposed in an online database breach or
  /// act of compromise.
  /// </value>
  public bool? IsLeaked { get; set; }

  /// <value>
  /// Indicates if the phone number has recently been reported for spam or
  /// harassing calls/texts.
  /// </value>
  public bool? IsSpammer { get; set; }

  /// <value>
  /// Additional details on the status of the subscriber connection when enhanced
  /// active line checks are enabled. Contact your account manager or support to
  /// to enable this add-on feature, which determines if a phone number is reachable
  /// or disconnected using our HLR lookup service and carrier signals. The values
  /// can be:
  /// "Active Line"
  /// "Active Line - High Confidence"
  /// "Active Line - Medium Confidence"
  /// "Active Line - Low Confidence"
  /// "Disconnected Line"
  /// "Phone Turned Off"
  /// "Inconclusive Status"
  /// of "N/A" if unknown. The confidence level is included for active lines
  /// whenever possible.
  /// </value>
  public string? ActiveStatus { get; set; }

  /// <value>
  /// Frequency at which this phone number makes legitimate purchases, account
  /// registrations, and engages in legitimate user behavior online. Values can be
  /// "high", "medium", "low", or "none". Value of "high" or "medium" are strong
  /// signals of healthy usage. New phone numbers without a history of legitimate
  /// behavior will have a value as "none". This field is restricted to higher plan
  /// tiers.
  /// </value>
  public string? UserActivity { get; set; }

  /// <value>
  /// Mobile Country Codes (MCC) identify the country of a mobile phone number
  /// subscriber. This provides a corresponding number to a specific country, to
  /// facilitate routing for wireless calls and SMS messages. The MCC value is "N/A"
  /// when unknown or not available, such as for landline and toll-free numbers.
  /// </value>
  public string? MobileCountryCode { get; set; }

  /// <value>
  /// Mobile Network Codes (MNC) identify the mobile carrier of a phone number
  /// subscriber. This data provides a corresponding number to a specific telecom
  /// provider, such as Orange, Vodafone, etc. to facilitate routing for wireless
  /// calls and SMS messages. The MNC values is "N/A" when unknown or not available,
  /// such as for landline and toll-free numbers.
  /// </value>
  public string? MobileNetworkCode { get; set; }

  /// <value>
  /// Displays email addresses linked to the phone number, if available in our
  /// data sources. Match rates vary by country and line type. This field is
  /// restricted to upgraded plans. Object value contains, "status", and "emails"
  /// as an array.
  /// </value>
  public AssociatedEmailAddresses AssociatedEmailAddresses { get; set; }

  /// <value>
  /// Additional scoring variables for risk analysis are available when transaction
  /// scoring data is passed through the API request. These variables are also
  /// useful for scoring user data such as physical address, phone numbers,
  /// usernames, and transaction details.
  /// </value>
  public TransactionDetails TransactionDetails { get; set; }
}

public class AssociatedEmailAddresses
{
  public string Status { get; set; }
  public string[] Emails { get; set; }
}

public class TransactionDetails
{
  
}
