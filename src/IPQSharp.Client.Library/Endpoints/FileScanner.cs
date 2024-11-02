using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace IPQSharp.FileScanner;

public enum ScanType
{
  Scan,
  Lookup
}


public class FileScanRequest : IPQSRequest
{
  public FileScanRequest(string apiKey) : base(apiKey)
  {}

  public bool DoFullScan { get; set; } = true;

  public FileStream? FileStream { get; set; }
  public Uri? FileUrl { get; set; }

  /// <summary>
  /// Send a Malware File Scanner request.
  /// </summary>
  public async Task<FileScanResult>
  SendAsync(CancellationToken token = default)
  {
    string fileContent;
    if(null != FileStream)
      using (StreamReader reader = new StreamReader(this.FileStream))
      {
        fileContent = reader.ReadToEnd();
      }
    else if(null != FileUrl)
      fileContent = FileUrl.ToString();
    else
      throw new FileScanException();

    var response = await Flurl.Url.Parse("https://www.ipqualityscore.com/api/json/malware")
      .AppendPathSegment(DoFullScan ? "scan" : "lookup")
      .AppendPathSegment(ApiKey)
      .PostUrlEncodedAsync(body: new
      {
        file = fileContent
      }, cancellationToken: token);
    var result = await response.GetJsonAsync<FileScanResult>();
    return result;
  }
}

public class FileScanResult : IPQSResult
{
  /// <value>
  /// The SHA256 hash of the provided file.
  /// </value>
  [JsonPropertyName("file_hash")]
  public string? FileHash { get; set; }

  /// <value>
  /// The type of scan performed. Can be either "scan" or "lookup".
  /// </value>
  [JsonPropertyName("type")]
  public string? ScanType { get; set; }

  /// <value>
  /// Depends on type of scan, possible values: "completed", "cached", or "pending".
  /// </value>
  [JsonPropertyName("status")]
  public string? Status { get; set; }

  /// <value>
  /// If the status is pending, request this url to fetch the result.
  /// </value>
  [JsonPropertyName("update_url")]
  public string? UpdateUrl { get; set; }

  /// <value>
  /// Result of the scan, which lists if any threat engines detected a virus,
  /// rootkit, ransomware, keylogger, or similar type of malicious file.
  /// </value>
  [JsonPropertyName("result")]
  public ScanResult[]? Result { get; set; }

  public class ScanResult
  {
    /// <value> Name of IPQS threat engine </value>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <value> Whether the engine detected a threat or not. </value>
    [JsonPropertyName("detected")]
    public bool? Detected { get; set; }
 
    /// <value> The errors detected by the engine. </value>
    [JsonPropertyName("error")]
    public string? Error { get; set; }
  }
}

public class FileScanException : IPQSException
{}
