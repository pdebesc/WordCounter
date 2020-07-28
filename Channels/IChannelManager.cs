using System.Collections.Generic;
using System.Threading.Tasks;

using WordCounter.ArgumentParser;
using WordCounter.Generated;
using WordCounter.Words;

namespace WordCounter.Channels
{
  interface IChannelManager
  {
    IEnumerable<Task> ProduceAsync(IInputData inputData);
    IEnumerable<Task> ConsumeAsync();
    Task<IReadOnlyDictionary<FileNameSuffix, IWordManager>> GetResultAsync(IEnumerable<Task> producers, IEnumerable<Task> consumers);
  }
}