using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace IPQSharp.DarkWebLeak;

public enum DataType
{
  Email,
  Password,
  Username
}

public class DarkWebLeakRequest : IPQSRequest
{
  public DarkWebLeakRequest(string apiKey) : base(apiKey)
  {}

  /// <value>
  /// The type of data being checked for leaks
  /// </value>
  public DataType? Type { get; set; }

  /// <value>
  /// The data to check for leaks
  /// </value>
  public object? Data { get; set; }

  public async Task<DarkWebLeakResult> SendAsync(CancellationToken token = default)
  {
    var url = Flurl.Url.Parse("https://www.ipqualityscore.com/api/json/leaked")
      .AppendPathSegment(Type.ToString().ToLower())
      .AppendPathSegment(ApiKey)
      .AppendPathSegment(Data);
    var response = await url.GetJsonAsync<DarkWebLeakResult>();
    return response;
  }
}

public class DarkWebLeakResult : IPQSResult
{
  [JsonPropertyName("source")]
  public string[]? Source { get; set; }

  [JsonPropertyName("exposed")]
  public bool? IsExposed { get; set; }

  [JsonPropertyName("plain_text_password")]
  public bool? HasPlainTextPassword { get; set; }
}
