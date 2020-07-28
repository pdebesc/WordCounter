using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

using WordCounter.ArgumentParser;

namespace WordCounter.Producers
{
  class ProducerManager : IProducerManager
  {
    readonly int numberOfProducers;
    readonly ChannelWriter<string> writer;
    readonly IProducerCreator producerCreator;

    public ProducerManager(int numberOfProducers, ChannelWriter<string> writer, IProducerCreator producerCreator)
    {
      this.numberOfProducers = numberOfProducers;
      this.writer = writer;
      this.producerCreator = producerCreator;
    }

    public IEnumerable<Task> ProduceAsync(IInputData inputData)
    {
      var tasks = new List<Task>(numberOfProducers);
      var partitions = SplitWork(inputData.GetInputData);
      for (var i = 0; i < numberOfProducers; i++)
      {
        var id = i;
        var task = Task.Run(
          async () =>
            {
              var producer = producerCreator.Create(writer);
              await producer.AddAsync(partitions[id]);
            });
        tasks.Add(task);
      }

      return tasks;
    }

    IList<IEnumerator<string>> SplitWork(IEnumerable<string> work)
    {
      return Partitioner.Create(work).GetPartitions(numberOfProducers);
    }
  }
}