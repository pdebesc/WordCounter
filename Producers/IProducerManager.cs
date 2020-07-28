using System.Collections.Generic;
using System.Threading.Tasks;

using WordCounter.ArgumentParser;

namespace WordCounter.Producers
{
  interface IProducerManager
  {
    IEnumerable<Task> ProduceAsync(IInputData inputData);
  }
}