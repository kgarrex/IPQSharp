namespace IPQSharp;

public abstract class IPQSRequest : IDisposable
{
  public string ApiKey { get; }

  public IPQSRequest(string apiKey)
  {
    ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
  }

  public void Dispose(){}
}
