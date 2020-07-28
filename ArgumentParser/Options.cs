using CommandLine;

namespace WordCounter.ArgumentParser
{
  public class Options
  {
    [Option('i', "Input", Required = true, HelpText = "The path to the input folder.")]
    public string Input { get; set; }
  }
}