using IPQSharp;

public class ProxyDetectionRequestTests
{
  private string FAKE_API_KEY = "fakeapikey";

  private void IPTest(string ip)
  {
    // Act
    Action action = () =>
    {
      using var req = new ProxyDetectionRequest(FAKE_API_KEY, ip);
    };

    // Assert
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Instantiate_WithBadIpAddressString_ThrowsException()
  {
    IPTest("1.2.3.");
    IPTest("fe80:2030:31:24");
  }
}
