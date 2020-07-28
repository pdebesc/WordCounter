using System.Collections.Generic;

namespace WordCounter.ArgumentParser
{
  interface IInputData
  {
    IEnumerable<string> GetInputData { get; }
  }
}