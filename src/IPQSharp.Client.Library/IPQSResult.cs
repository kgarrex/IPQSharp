using NetJSON;

namespace IPQSharp;
public class IPQSResult
{
  ///<value>
  ///A generic status message, either success or some form of an error notice.
  ///</value>
  [NetJSONPropertyAttribute("message")]
  public string? Message { get; set; }

  ///<value>Was the request successful?</value>
  [NetJSONPropertyAttribute("success")]
  public bool Success { get; set; } = false;

  /**
   * <value>
   * A unique identifier for this request that can be used to lookup the request details
   * or send a postback conversion notice.
   * </value>
   */
  [NetJSONPropertyAttribute("request_id")]
  public string? RequestId { get; set; }

  ///<value>Array of errors which occurred while attempting to process this request.</value>
  [NetJSONPropertyAttribute("errors")]
  public string[]? Errors { get; set; }

}
