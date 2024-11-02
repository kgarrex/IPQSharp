using System.Threading;
using System.Threading.Tasks;

namespace IPQSharp.ProxyDetection
{
  public interface IProxyDetector
  {
    /// <summary>
    /// Detect a proxy, vpn, tor client
    /// </summary>
    public Task<ProxyDetectionResult> DetectProxy(
      string ipstr,
      Action<ProxyDetectionRequest> makeRequest,
      CancellationToken token = default
    );
  }
}

namespace IPQSharp
{
  public static class IPQS
  {
    public static string ApiKey { get; set; } = String.Empty;

    public static bool ThrowExceptionOnError { get; set; } = false;

    /// <value>
    /// the proxy detector
    /// </value>
    //public static IProxyDetector ProxyDetector { get; } = new ProxyDetector();

    /// <value>
    /// The IPQS Account used for account management and information
    /// </value>
    //public static IIPQSAccount Account { get; set; } = new IPQSAccount();

    //public static IFraudReporter FraudReporter { get; set; } = new FraudReporter();
  }
}
