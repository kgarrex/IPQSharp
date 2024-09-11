namespace IPQSharp;
public abstract class IPQSRequest
{
  public string ApiKey { get; }

  public IPQSRequest(string apiKey)
  {
    ApiKey = apiKey;
  }
}
