using CommandLine;

namespace WordCounter.ArgumentParser
{
  class CommandLineArgumentHandler : ICommandLineArgumentHandler
  {
    public bool TryParse(string[] args, out IProgramArguments programArguments)
    {
      programArguments = null;
      var result = Parser.Default.ParseArguments<Options>(args);
      if (result.Tag == ParserResultType.NotParsed)
        return false;

      IProgramArguments programArgs = null;
      result.WithParsed(options => programArgs = new ProgramArguments(options));
      programArguments = programArgs;
      return true;
    }
  }
}