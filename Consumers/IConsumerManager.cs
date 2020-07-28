using System.Collections.Generic;
using System.Threading.Tasks;

using WordCounter.Generated;
using WordCounter.Words;

namespace WordCounter.Consumers
{
  interface IConsumerManager
  {
    IEnumerable<Task> ConsumeAsync();

    IReadOnlyDictionary<FileNameSuffix, IWordManager> GetFinishedWork();
  }
}