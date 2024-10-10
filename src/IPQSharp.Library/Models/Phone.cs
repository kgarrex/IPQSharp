using InternalPhoneNumber = PhoneNumbers.PhoneNumber;
using PhoneNumbers;
using PhoneNumbers.Extensions;

namespace IPQSharp.Internal;
public record Phone
{
  public string Number { get; }
  public string CountryCode { get; }

  private PhoneNumbers.PhoneNumber _phoneNumber;

  /// <summary>
  /// Initialize a phone number 
  /// </summary>
  public Phone(string number)
  {
    var pn = PhoneNumberUtil.GetInstance().Parse(number, "ZZ");
    //PhoneNumber.TryParse(number, "ZZ", out 
  }

  public Phone(string number, string countryCode)
  {
    Number = number;
    CountryCode = countryCode;
  }
}

public class PhoneException : IPQSException
{
  public string Message => "There was an issue with the provided phone number.";
}


