namespace IPQSharp;

public class IPQSException : Exception
{
  public IPQSException()
  {}

  public IPQSException(string message) : base(message)
  {}

  public IPQSException(string message, Exception inner) : base(message, inner)
  {}
}

public class PhoneNumberException : IPQSException
{}
