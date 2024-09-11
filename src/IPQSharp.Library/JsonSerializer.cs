using System.IO;
using Flurl.Http.Configuration;

namespace IPQSharp.Internal;
public class JsonSerializer : ISerializer
{
  private NetJSON.NetJSONSettings _settings;
  public JsonSerializer()
  {
    _settings = new NetJSON.NetJSONSettings();
  }

  public string Serialize(object obj)
  {
    return NetJSON.NetJSON.Serialize(obj, _settings);
  }

  public T Deserialize<T>(string json)
  {
    return NetJSON.NetJSON.Deserialize<T>(json, _settings);
  }

  public T Deserialize<T>(Stream stream)
  {
    throw new NotImplementedException();
  }
}
