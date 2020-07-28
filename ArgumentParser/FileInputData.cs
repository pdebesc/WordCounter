using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordCounter.ArgumentParser
{
  class FileInputData : IInputData
  {
    readonly string inputDirectoryPath;

    public FileInputData(string inputDirectoryPath)
    {
      this.inputDirectoryPath = inputDirectoryPath;
    }

    public IEnumerable<string> GetInputData => new DirectoryInfo(inputDirectoryPath)
      .EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly)
      .Select(x => x.FullName);
  }
}