using EmailValidation;

namespace IPQSharp;
public record Email
{
  public string Address { get; set; }

  public Email(string address)
  {
    if(false == EmailValidator.Validate(address, true, true))
    {
      throw new EmailException();
    }
    Address = address;
  }
}

public class EmailException : IPQSException
{
  public string Message => "There was an issue with the provided email address";
}
