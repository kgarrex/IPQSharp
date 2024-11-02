using Spectre.Console.Cli;

public class CommandInterceptor : ICommandInterceptor
{
  public void Intercept(CommandContext context, CommandSettings settings)
  {
    IPQSSettings ipqsSettings = settings as IPQSSettings;
    // if the api key is missing in the settings, we shall search for it
    // in the environment variables.
    // if its not in the environment, we shall throw an exception
    ipqsSettings.ApiKey =
      ipqsSettings.ApiKey ?? 
      System.Environment.GetEnvironmentVariable("IPQS_API_KEY");

    if(null == ipqsSettings.ApiKey) throw new Exception("Missing Api Key");
  }
}
