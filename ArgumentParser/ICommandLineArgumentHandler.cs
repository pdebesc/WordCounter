namespace WordCounter.ArgumentParser
{
  interface ICommandLineArgumentHandler
  {
    bool TryParse(string[] args, out IProgramArguments options);
  }
}