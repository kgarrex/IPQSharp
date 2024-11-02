using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace IPQSharp.AllowList;

public enum ProxyValueType
{
  IPAddress,
  CIDR,
  ISP
}

public enum DeviceTrackerValueType
{
  DeviceId,
  IPAddress,
  CIDR,
  ISP
}

public enum MobileTrackerValueType
{
  
}

public enum Type 
{
  Proxy,
  DeviceTracker,
  MobileTracker,
  Email,
  Url,
  Phone,
  Custom
}

public class AllowListCreateRequest : IPQSRequest
{
  public AllowListCreateRequest(string apiKey) : base(apiKey)
  {}

  public async Task<IPQSResult> SendAsync(CancellationToken token = default)
  {
    var response = await "https://www.ipqualityscore.com/api/json/allowlist/create"
      .AppendPathSegment(ApiKey)
      .PostUrlEncodedAsync(body: new
      {
        value = "1.1.1.1",
        value_type = "ip",
        type = "proxy",
        reason = "The reason this entry is on the AllowList.",
      }, cancellationToken: token);
    var result = await response.GetJsonAsync<IPQSResult>();
    return result;
  }
}

public class AllowListResult : IPQSResult
{

}
