public class ErrorResponse
{
  public bool Success { get; set; } = false;
  public string Message { get; set; } = String.Empty;
  public string RequestId { get; set; } = String.Empty;
}
